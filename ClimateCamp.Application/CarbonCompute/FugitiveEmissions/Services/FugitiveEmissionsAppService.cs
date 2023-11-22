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
using System.Threading.Tasks;
using static ClimateCamp.CarbonCompute.GHG;

namespace ClimateCamp.Application
{
    [AbpAuthorize]
    public class FugitiveEmissionsAppService : AsyncCrudAppService<FugitiveEmissionsData, FugitiveEmissionsDataDto, Guid, FugitiveEmissionsResponseDto, CreateFugitiveEmissionsDto, CreateFugitiveEmissionsDto>, IFugitiveEmissionsAppService
    {
        private readonly IRepository<FugitiveEmissionsData, Guid> _fugitiveEmissionsRepository;
        private readonly IRepository<ActivityData, Guid> _activityRepository;
        private readonly IRepository<ActivityType, int> _activityTypeRepository;
        private readonly IRepository<Emission, Guid> _emissionsRepository;
        private readonly IRepository<EmissionsSource, int> _emissionsSourceRepository;
        private readonly IRepository<OrganizationUnit, Guid> _organizationUnitRepository;
        private readonly IRepository<Unit, int> _unitRepository;
        private readonly IRepository<GreenhouseGas, Guid> _greenhouseGasesRepository;
        private readonly IRepository<EmissionGroups, Guid> _emissionGroupsRepository;
        private readonly ILogger<FugitiveEmissionsAppService> _logger;


        public FugitiveEmissionsAppService
            (IRepository<FugitiveEmissionsData, Guid> fugitiveEmissionsRepository,
             IRepository<ActivityData, Guid> activityRepository,
             IRepository<ActivityType, int> activityTypeRepository,
             IRepository<Emission, Guid> emissionsRepository,
             IRepository<EmissionsSource, int> emissionsSourceRepository,
             IRepository<OrganizationUnit, Guid> organizationUnitRepository,
             IRepository<Unit, int> unitRepository,
             IRepository<GreenhouseGas, Guid> greenhouseGasesRepository,
             IRepository<EmissionGroups, Guid> emissionGroupsRepository) : base(fugitiveEmissionsRepository)
        {
            _fugitiveEmissionsRepository = fugitiveEmissionsRepository;
            _activityRepository = activityRepository;
            _activityTypeRepository = activityTypeRepository;
            _emissionsRepository = emissionsRepository;
            _emissionsSourceRepository = emissionsSourceRepository;
            _organizationUnitRepository = organizationUnitRepository;
            _unitRepository = unitRepository;
            _greenhouseGasesRepository = greenhouseGasesRepository;
            _emissionGroupsRepository = emissionGroupsRepository;
        }

        public async Task<FugitiveEmissionsData> AddFugitiveEmissionsDataAsync(ActivityDataVM input)
        {
            try
            {
                if (input.CO2eUnitId == 0) throw new UserFriendlyException("CO2e Unit Id cannot be 0");

                if (input.CO2e != null && input.CO2eUnitId == null)
                {
                    throw new UserFriendlyException("The Unit for CO2e cannot be null.", "If there is a value for CO2e, then there must be a value for CO2eUnitId as well.");
                }

                float? calculatedEmissions = EmissionsCalculator.CalculateEmissions(input.Emission, input.CO2e, input.Quantity);

                var activityTypes = _activityTypeRepository.GetAll().Where(x => x.EmissionsSourceId == input.EmissionSourceId);
                var activityId = Guid.NewGuid();
                var activityModel = new FugitiveEmissionsData
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
                    IndustrialProcessId = 1,
                    ConsumptionStart = (DateTime)input.ConsumptionStart,
                    ConsumptionEnd = (DateTime)input.ConsumptionEnd,
                    OrganizationUnitId = input.OrganizationUnitId,
                    isProcessed = false,
                    GreenhouseGasId = (Guid)input.GreenhouseGasId,
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

                var result = await _fugitiveEmissionsRepository.InsertAsync(activityModel);
                await _emissionsRepository.InsertAsync(emissionsModel);
                await CurrentUnitOfWork.SaveChangesAsync();

                return result;
            }
            catch (UserFriendlyException userEx)
            {
                throw new UserFriendlyException(userEx.Message, userEx.Details);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Method: AddFugitiveEmissionsDataAsync - Exception: {ex}");
                return null;
            }
        }

        public async Task<List<ActivityDataVM>> GetFugitiveEmissionsData(Guid organizationId, int emissionSourceId, DateTime? consumptionStart, DateTime? consumptionEnd)
        {
            try
            {
                var fugitiveEmissionsData = from f in _fugitiveEmissionsRepository.GetAll()
                                            join a in _activityRepository.GetAll() on f.Id equals a.Id into fa
                                            from a in fa.DefaultIfEmpty()
                                            join g in _greenhouseGasesRepository.GetAll() on f.GreenhouseGasId equals g.Id into fg
                                            from g in fg.DefaultIfEmpty()
                                            join at in _activityTypeRepository.GetAll() on f.ActivityTypeId equals at.Id into tat
                                            from at in tat.DefaultIfEmpty()
                                            join es in _emissionsSourceRepository.GetAll() on at.EmissionsSourceId equals es.Id into ates
                                            from es in ates.DefaultIfEmpty()
                                            join ou in _organizationUnitRepository.GetAll() on a.OrganizationUnitId equals ou.Id into aou
                                            from ou in aou.DefaultIfEmpty()
                                            join e in _emissionsRepository.GetAll() on a.Id equals e.ActivityDataId into ae
                                            from e in ae.DefaultIfEmpty()
                                            join eg in _emissionGroupsRepository.GetAll() on a.EmissionGroupId equals eg.Id into aeg
                                            from eg in aeg.DefaultIfEmpty()
                                            where ou.OrganizationId == organizationId && es.Id == emissionSourceId
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
                                                GreenhouseGasId = f.GreenhouseGasId,
                                                GreenhouseGasCode = g.Code,
                                                GroupName = eg.Name
                                            };

                if (consumptionStart != null && consumptionEnd != null)
                {
                    fugitiveEmissionsData = fugitiveEmissionsData.Where(x => (x.ConsumptionEnd >= consumptionStart.Value.Date && x.ConsumptionStart <= consumptionEnd.Value.Date));
                }

                var result = await fugitiveEmissionsData.ToListAsync();
                return result;

            }
            catch(Exception ex)
            {
                _logger.LogError($"Method: GetFugitiveEmissionsData - Exception: {ex}");
                return null;
            }
        }


        public async Task<List<GreenhouseGasGroup>> GetGroupedGreenhouseGasses()
        {
            try
            {

                List<GreenhouseGasGroup> GreenhouseGasGroupList = new List<GreenhouseGasGroup>();

                List<GHG.GreenhouseGasCategory> GreenhouseGasCategoryList = Enum.GetValues(typeof(GHG.GreenhouseGasCategory)).Cast<GHG.GreenhouseGasCategory>().ToList();

                List<GreenhouseGas> GreenhouseGasesList = _greenhouseGasesRepository.GetAll().ToList();

                     foreach (var greenhouseGasCategory in GreenhouseGasCategoryList)
                     {
                            GreenhouseGasGroup model = new GreenhouseGasGroup();
                            model.label = greenhouseGasCategory.ToString();
                            model.data = (int)greenhouseGasCategory;
                            model.Children = GreenhouseGasesList.Any(x => x.Category == greenhouseGasCategory) ?
                                             GreenhouseGasesList.Where(x => x.Category == greenhouseGasCategory)
                                             .Select(x => new Child { data = x.Id, label = x.Code }).OrderBy(x => x.label)
                                             .ToList() : new List<Child>();
                             if (model.Children.Count > 0)
                             {
                                 GreenhouseGasGroupList.Add(model);
                             }

                     }

                    return GreenhouseGasGroupList;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Method: GetGroupedGreenhouseGasses - Exception: {ex}");
                return null;
            }
        }

       

        public async Task<FugitiveEmissionsData> UpdateFugitiveEmissionsDataAsync(ActivityDataVM input)
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
                var fugitiveEmissionsData = _fugitiveEmissionsRepository.Get((Guid)input.Id);

                fugitiveEmissionsData.Name = input.Name;
                fugitiveEmissionsData.Quantity = (float)input.Quantity;
                fugitiveEmissionsData.UnitId = input.UnitId;//TODO: To be revised and adjusted in case that from the front end this will be something that the user can set
                fugitiveEmissionsData.TransactionDate = (DateTime)input.ConsumptionEnd;
                fugitiveEmissionsData.ActivityTypeId = activityTypeId;
                fugitiveEmissionsData.Description = input.Description;
                fugitiveEmissionsData.IsActive = true;
                fugitiveEmissionsData.DataQualityType = DataQualityType.Actual;
                fugitiveEmissionsData.IndustrialProcessId = 1;
                fugitiveEmissionsData.ConsumptionStart = (DateTime)input.ConsumptionStart;
                fugitiveEmissionsData.ConsumptionEnd = (DateTime)input.ConsumptionEnd;
                fugitiveEmissionsData.OrganizationUnitId = input.OrganizationUnitId;
                fugitiveEmissionsData.isProcessed = false;
                fugitiveEmissionsData.GreenhouseGasId = (Guid)input.GreenhouseGasId;
                fugitiveEmissionsData.EmissionGroupId = input.EmissionGroupId;
                fugitiveEmissionsData.EmissionFactorId = input.EmissionFactorId;
                fugitiveEmissionsData.Status = input.ActivityDataStatus;

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

                var result = await _fugitiveEmissionsRepository.UpdateAsync(fugitiveEmissionsData);
                return result;
            }
            catch (UserFriendlyException userEx)
            {
                throw new UserFriendlyException(userEx.Message, userEx.Details);
            }
            catch (Exception ex)
            { 
                _logger.LogError($"Method: UpdateFugitiveEmissionsDataAsync - Exception: {ex}");
                return null;
            }
        }
    }
}
