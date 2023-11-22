using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using ClimateCamp.Application.Common;
using ClimateCamp.CarbonCompute;
using ClimateCamp.Core;
using ClimateCamp.Core.CarbonCompute;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static ClimateCamp.CarbonCompute.GHG;

namespace ClimateCamp.Application
{
    [AbpAuthorize]
    public class UseOfSoldProductsAppService : AsyncCrudAppService<UseOfSoldProductsData, UseOfSoldProductsDto, Guid, UseOfSoldProductsResponseDto, CreateUseOfSoldProductsDto, CreateUseOfSoldProductsDto>, IUseOfSoldProductsAppService
    {
        private readonly IRepository<ActivityType, int> _activityTypeRepository;
        private readonly IRepository<UseOfSoldProductsData, Guid> _useOfSoldProductsRepository;
        private readonly ILogger<UseOfSoldProductsAppService> _logger;
        private readonly IRepository<Emission, Guid> _emissionsRepository;

        public UseOfSoldProductsAppService
            (IRepository<UseOfSoldProductsData, Guid> useOfSoldProductsRepository,
             IRepository<ActivityType, int> activityTypeRepository,
             IRepository<Emission, Guid> emissionsRepository) : base(useOfSoldProductsRepository)
        {
            _useOfSoldProductsRepository = useOfSoldProductsRepository;
            _activityTypeRepository = activityTypeRepository;
            _emissionsRepository = emissionsRepository;
        }


        public async Task<UseOfSoldProductsData> AddUseOfSoldProductsDataAsync(ActivityDataVM input)
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
                var activityModel = new UseOfSoldProductsData
                {
                    Id = activityId,
                    Name = input.Name,
                    Quantity = (float)input.Quantity,
                    UnitId = input.UnitId,
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
                    EmissionGroupId = input.EmissionGroupId,
                    EmissionFactorId = input.EmissionFactorId,
                    Status = input.ActivityDataStatus
                };
                //The INSERT statement conflicted with the FOREIGN KEY constraint "FK_ActivityData_EmissionGroups_EmissionGroupId".The conflict occurred in database "cliamtecampLocal", table "Reference.EmissionGroups", column 'Id'.
                //The statement has been terminated.
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

                var result = await _useOfSoldProductsRepository.InsertAsync(activityModel);
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
        public Task<UseOfSoldProductsData> GetUseOfSoldProductsData()
        {
            throw new NotImplementedException();
        }
        public async Task<UseOfSoldProductsData> UpdateUseOfSoldProductsDataAsync(ActivityDataVM input)
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
                var useOfSoldProductsData = _useOfSoldProductsRepository.Get((Guid)input.Id);

                useOfSoldProductsData.Name = input.Name;
                useOfSoldProductsData.Quantity = (float)input.Quantity;
                useOfSoldProductsData.UnitId = input.UnitId;//TODO: To be revised and adjusted in case that from the front end this will be something that the user can set
                useOfSoldProductsData.TransactionDate = (DateTime)input.ConsumptionEnd;
                useOfSoldProductsData.ActivityTypeId = activityTypeId;
                useOfSoldProductsData.Description = input.Description;
                useOfSoldProductsData.IsActive = true;
                useOfSoldProductsData.DataQualityType = DataQualityType.Actual;
                useOfSoldProductsData.IndustrialProcessId = 1;
                useOfSoldProductsData.ConsumptionStart = (DateTime)input.ConsumptionStart;
                useOfSoldProductsData.ConsumptionEnd = (DateTime)input.ConsumptionEnd;
                useOfSoldProductsData.OrganizationUnitId = input.OrganizationUnitId;
                useOfSoldProductsData.isProcessed = false;
                useOfSoldProductsData.EmissionGroupId = input.EmissionGroupId;
                useOfSoldProductsData.EmissionFactorId = input.EmissionFactorId;
                useOfSoldProductsData.Status = input.ActivityDataStatus;
                useOfSoldProductsData.ElectricityConsumptionPerUseOfProduct = input.ElectricityConsumptionPerUseOfProduct;
                useOfSoldProductsData.ElectricityUnitId = input.ElectricityUnitId ;
                useOfSoldProductsData.RefrigerantLeakagePerUseOfProduct = input.RefrigerantLeakagePerUseOfProduct;
                useOfSoldProductsData.RefrigerantLeakageUnitId = input.RefrigerantLeakageUnitId;
                useOfSoldProductsData.FuelUsedPerUseOfProduct = input.FuelUsedPerUseOfProduct;
                useOfSoldProductsData.FuelUnitId = input.FuelUnitId;

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

                var result = await _useOfSoldProductsRepository.UpdateAsync(useOfSoldProductsData);
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
