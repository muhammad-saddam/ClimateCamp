using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using ClimateCamp.Application.Common;
using ClimateCamp.CarbonCompute;
using ClimateCamp.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static ClimateCamp.CarbonCompute.GHG;


namespace ClimateCamp.Application
{
    [AbpAuthorize]
    public class EmployeeCommuteAppService : AsyncCrudAppService<EmployeeCommuteData, EmployeeCommuteDataDto, Guid, EmployeeCommuteResponseDto, CreateEmployeeCommuteDataDto, CreateEmployeeCommuteDataDto>, IEmployeeCommuteAppService
    {
        private readonly IRepository<EmployeeCommuteData, Guid> _employeeCommuteRepository;
        private readonly IRepository<ActivityData, Guid> _activityRepository;
        private readonly IRepository<ActivityType, int> _activityTypeRepository;
        private readonly IRepository<Emission, Guid> _emissionsRepository;
        private readonly IRepository<EmissionsSource, int> _emissionsSourceRepository;
        private readonly IRepository<OrganizationUnit, Guid> _organizationUnitRepository;
        private IRepository<VehicleType, Guid> _vehicleTypeRepository;
        private readonly ILogger<EmployeeCommuteAppService> _logger;

        public EmployeeCommuteAppService
            (IRepository<EmployeeCommuteData, Guid> employeeCommuteRepository,
             IRepository<ActivityData, Guid> activityRepository,
             IRepository<ActivityType, int> activityTypeRepository,
             IRepository<Emission, Guid> emissionsRepository,
             IRepository<EmissionsSource, int> emissionsSourceRepository,
             IRepository<OrganizationUnit, Guid> organizationUnitRepository,
             IRepository<VehicleType, Guid> vehicleTypeRepository,
             IRepository<Unit, int> unitRepository) : base(employeeCommuteRepository)
        {
            _employeeCommuteRepository = employeeCommuteRepository;
            _activityRepository = activityRepository;
            _activityTypeRepository = activityTypeRepository;
            _emissionsRepository = emissionsRepository;
            _emissionsSourceRepository = emissionsSourceRepository;
            _organizationUnitRepository = organizationUnitRepository;
            _vehicleTypeRepository = vehicleTypeRepository;
        }

        /// <summary>
        /// Add Employee Commute activity data as well as a corresponding emission
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<EmployeeCommuteData> AddEmployeeCommuteDataAsync(ActivityDataVM input)
        {
            try
            {
                if (input.CO2eUnitId == 0) throw new UserFriendlyException("CO2e Unit Id cannot be 0");
                if (input.CO2e != null && input.CO2eUnitId == null)
                {
                    throw new UserFriendlyException("The Unit for CO2e cannot be null.", "If there is a value for CO2e, then there must be a value for CO2eUnitId as well.");
                }
                var activityTypes = _activityTypeRepository.GetAll().Where(x => x.EmissionsSourceId == input.EmissionSourceId);
                var emissionSourceName = _emissionsSourceRepository.Get(input.EmissionSourceId).Name;
                
                var activityId = Guid.NewGuid();

                float? calculatedEmissions = EmissionsCalculator.CalculateEmissions(input.Emission, input.CO2e, input.Quantity);

                var activityModel = new EmployeeCommuteData
                {
                    Id = activityId,
                    Name = input.Name,
                    Quantity = (float)input.Quantity,
                    UnitId = input.UnitId,
                    //TODO: To be revised and adjusted in case that from the front end this will be something that the user can set
                    TransactionDate = (DateTime)input.ConsumptionEnd,
                    ActivityTypeId = activityTypes.FirstOrDefault()?.Id,
                    Description = input.Description,
                    IsActive = true,
                    DataQualityType = DataQualityType.Actual,
                    //todo: confirm?
                    IndustrialProcessId = 1,
                    ConsumptionStart = (DateTime)input.ConsumptionStart,
                    ConsumptionEnd = (DateTime)input.ConsumptionEnd,
                    OrganizationUnitId = input.OrganizationUnitId,
                    isProcessed = false,
                    VehicleTypeId = (Guid)input.VehicleTypeId,
                    EmissionGroupId = input.EmissionGroupId,
                    EmissionFactorId = input.EmissionFactorId,
                    Status = input.ActivityDataStatus
                };

                var emissionsModel = new Emission
                {
                    OrganizationUnitId = input.OrganizationUnitId,
                    EmissionsDataQualityScore = GHG.EmissionsDataQualityScore.Estimated,
                    EmissionsFactorsLibraryId = ClimateCampConsts.DefaultEmissionsFactorsLibraryId,
                    ResponsibleEntityID = Guid.Empty,
                    ResponsibleEntityType = (int)GHG.ResponsibleEntityTypes.User,
                    IsActive = true,
                    CO2E = calculatedEmissions,
                    CO2EUnitId = input.CO2eUnitId,
                    CreationTime = DateTime.UtcNow,
                    ActivityDataId = activityId
                };

                var result = await _employeeCommuteRepository.InsertAsync(activityModel);
                await _emissionsRepository.InsertAsync(emissionsModel);
                await CurrentUnitOfWork.SaveChangesAsync();

                return result;
            }
            catch (UserFriendlyException userEx)
            {
                throw new UserFriendlyException(userEx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Method: AddEmployeeCommuteDataAsync - Exception: {ex}");
                return null;
            }
        }

        /// <summary>
        /// Method used to retrieve Employee Commute ActivityData, along with correlated emissions if they exist.
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="emissionSourceId"></param>
        /// <param name="consumptionStart"></param>
        /// <param name="consumptionEnd"></param>
        /// <returns></returns>
        public async Task<List<ActivityDataVM>> GetEmployeeCommuteData(Guid organizationId, int emissionSourceId, DateTime? consumptionStart, DateTime? consumptionEnd)
        {
            try
            {
                var employeeCommuteData = from ec in _employeeCommuteRepository.GetAll()
                                    join a in _activityRepository.GetAll() on ec.Id equals a.Id into eca
                                    from a in eca.DefaultIfEmpty()
                                    join at in _activityTypeRepository.GetAll() on ec.ActivityTypeId equals at.Id into ecat
                                    from at in ecat.DefaultIfEmpty()
                                    join es in _emissionsSourceRepository.GetAll() on at.EmissionsSourceId equals es.Id into ates
                                    from es in ates.DefaultIfEmpty()
                                    join ou in _organizationUnitRepository.GetAll() on a.OrganizationUnitId equals ou.Id into aou
                                    from ou in aou.DefaultIfEmpty()
                                    join e in _emissionsRepository.GetAll() on a.Id equals e.ActivityDataId into ae
                                    from e in ae.DefaultIfEmpty()
                                    where ou.OrganizationId == organizationId
                                        && es.Id == emissionSourceId
                                    select new ActivityDataVM
                                    {
                                        Id = a.Id,
                                        Name = a.Name,
                                        OrganizationUnit = ou.Name,
                                        OrganizationUnitId = ou.Id,
                                        EmissionSourceId = es.Id,
                                        IsProcessed = a.isProcessed,
                                        IsDeleted = a.IsDeleted,
                                        Quantity = a.Quantity,
                                        UnitId = (int)a.UnitId,
                                        Description = a.Description,
                                        ConsumptionStart = a.ConsumptionStart,
                                        ConsumptionEnd = a.ConsumptionEnd,
                                        TransactionDate = a.TransactionDate,
                                        CO2e = e.CO2E,
                                        CO2eUnitId = e.CO2EUnitId,
                                        VehicleTypeId = ec.VehicleTypeId,
                                        VehicleTypeName = ec.VehicleType.Name
                                    };

                if (consumptionStart != null && consumptionEnd != null)
                {
                    employeeCommuteData = employeeCommuteData.Where(x => (x.ConsumptionEnd >= consumptionStart.Value.Date && x.ConsumptionStart <= consumptionEnd.Value.Date));
                }

                var result = await employeeCommuteData.ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Method: GetEmployeeCommuteData - Exception: {ex}");
                return null;
            }
        }

        public async Task<EmployeeCommuteData> UpdateEmployeeCommuteDataAsync(ActivityDataVM input)
        {
            try
            {
                if (input.CO2eUnitId == 0) throw new UserFriendlyException("CO2e Unit Id cannot be 0");

                if (input.CO2e != null && input.CO2eUnitId == null)
                {
                    throw new UserFriendlyException("The Unit for CO2e cannot be null.", "If there is a value for CO2e, then there must be a value for CO2eUnitId as well.");
                }

                float? calculatedEmissions = EmissionsCalculator.CalculateEmissions(input.Emission, input.CO2e, input.Quantity);

                var activityTypeId = _activityTypeRepository.FirstOrDefault(x => x.EmissionsSourceId == input.EmissionSourceId).Id;
                var employeeCommuteData = _employeeCommuteRepository.Get((Guid)input.Id);

                employeeCommuteData.Name = input.Name;
                employeeCommuteData.Quantity = (float)input.Quantity;
                employeeCommuteData.UnitId = input.UnitId;
                employeeCommuteData.TransactionDate = (DateTime)input.ConsumptionEnd;
                employeeCommuteData.ActivityTypeId = activityTypeId;
                employeeCommuteData.Description = input.Description;
                employeeCommuteData.IsActive = true;
                employeeCommuteData.DataQualityType = DataQualityType.Actual;
                employeeCommuteData.IndustrialProcessId = 1;
                employeeCommuteData.ConsumptionStart = (DateTime)input.ConsumptionStart;
                employeeCommuteData.ConsumptionEnd = (DateTime)input.ConsumptionEnd;
                employeeCommuteData.OrganizationUnitId = input.OrganizationUnitId;
                employeeCommuteData.isProcessed = false;
                employeeCommuteData.VehicleTypeId = (Guid)input.VehicleTypeId;
                employeeCommuteData.EmissionGroupId = input.EmissionGroupId;
                employeeCommuteData.EmissionFactorId = input.EmissionFactorId;
                employeeCommuteData.Status = input.ActivityDataStatus;

                var emissionRow = _emissionsRepository
                    .GetAll()
                    .Where(e => e.ActivityDataId == input.Id)
                    .FirstOrDefault();

                if (emissionRow != null)
                {
                    emissionRow.OrganizationUnitId = input.OrganizationUnitId;
                    emissionRow.CO2E = calculatedEmissions;
                    emissionRow.CO2EUnitId = input.CO2eUnitId;
                }
                else
                {
                    var emissionsModel = new Emission
                    {
                        OrganizationUnitId = input.OrganizationUnitId,
                        EmissionsDataQualityScore = GHG.EmissionsDataQualityScore.Estimated,
                        EmissionsFactorsLibraryId = ClimateCampConsts.DefaultEmissionsFactorsLibraryId,
                        ResponsibleEntityID = Guid.Empty,
                        ResponsibleEntityType = (int)GHG.ResponsibleEntityTypes.User,
                        IsActive = true,
                        CO2E = calculatedEmissions,
                        CO2EUnitId = input.CO2eUnitId,
                        CreationTime = DateTime.UtcNow,
                        ActivityDataId = (Guid)input.Id
                    };

                    await _emissionsRepository.InsertAsync(emissionsModel);
                }

                var result = await _employeeCommuteRepository.UpdateAsync(employeeCommuteData);
                return result;
            }
            catch (UserFriendlyException userEx)
            {
                throw new UserFriendlyException(userEx.Message, userEx.Details);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Method: UpdateEmployeeCommuteDataAsync - Exception: {ex}");
                return null;
            }
        }

        public async Task<List<VehicleTypeGroup>> GetGroupedVehicleTypes(int transportationKind)
        {
            try
            {

                List<VehicleTypeGroup> VehicleTypeGroupList = new List<VehicleTypeGroup>();

                List<GHG.ModeOfTransport> ModeOfTransportList = Enum.GetValues(typeof(GHG.ModeOfTransport)).Cast<GHG.ModeOfTransport>().ToList();

                List<VehicleType> VehicleTypeList = _vehicleTypeRepository.GetAll().ToList();

                foreach (var ModeOfTransport in ModeOfTransportList)
                {
                    VehicleTypeGroup model = new VehicleTypeGroup();
                    var Mode = ModeOfTransport.ToString();
                    model.label = Regex.IsMatch(Mode, "[A-Z][a-z]") ? Regex.Replace(Mode, "(?<=[a-z])(?=[A-Z])", " ") : Mode; //modes of transport like PublicTransportation displayed as Public Transportation
                    model.data = (int)ModeOfTransport;
                    model.Children = VehicleTypeList.Any(x => x.ModeOfTransport == (int)ModeOfTransport) ?
                                     VehicleTypeList.Where(x => x.ModeOfTransport == (int)ModeOfTransport && x.TransportationKind == transportationKind)
                                     .Select(x => new VTChild { data = x.Id, label = x.Name })
                                     .OrderBy(x => x.label.ToLower().Contains("mixed"))
                                     .ThenBy(x => x.label.ToLower().Contains("generic")) //generic/mixed option last in the list
                                     .ThenBy(x => x.label)
                                     .ToList() : new List<VTChild>();
                    if (model.Children.Count > 0)
                    {
                        VehicleTypeGroupList.Add(model);
                    }

                }

                return VehicleTypeGroupList;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Method: GetGroupedVehicleTypes - Exception: {ex}");
                return null;
            }
        }
    }
}
