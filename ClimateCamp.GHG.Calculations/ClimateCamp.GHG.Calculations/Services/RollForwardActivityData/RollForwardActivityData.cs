using Abp.UI;
using ClimateCamp.EntityFrameworkCore;
using ClimateCamp.GHG.Calculations.DataService.Interface;
using ClimateCamp.GHG.Calculations.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mobile.Combustion.Calculation.DataService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClimateCamp.GHG.Calculations.Services.RollForwardActivityData
{
    public class RollForwardActivityData : IRollForwardActivityData
    {
        private readonly IEmissionGroupsDataService _emissionGroupsDataService;
        private readonly IActivityDataService _activityDataService;
        private readonly CommonDbContext _dbContext;
        private readonly ILogger _logger;

        public RollForwardActivityData(
            IEmissionGroupsDataService emissionGroupsDataService,
            IActivityDataService activityDataService,
            CommonDbContext dbContext,
            ILoggerFactory loggerFactory)
        {
            _emissionGroupsDataService = emissionGroupsDataService;
            _activityDataService = activityDataService;
            _dbContext = dbContext;
            _logger = loggerFactory.CreateLogger<EmissionsFactorsDataService>();
        }

        /// <summary>
        /// Method used to "Roll Forward" activity data from a "source" reporting period to a "target" reporting period.
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="consumptionStart"></param>
        /// <param name="consumptionEnd"></param>
        /// <returns></returns>
        public async Task<bool> RollForwardActivityDataByOrganizationId(Guid organizationId, DateTime consumptionStart, DateTime consumptionEnd, DateTime targetPeriodStart, DateTime targetPeriodEnd)
        {
            try
            {
                // First check if there's already any activity data within the target period date range
                if (await HasNoActivityData(organizationId, targetPeriodStart, targetPeriodEnd))
                {
                    // get the list of emission groups by organization id
                    var emissionGroupsList = await _emissionGroupsDataService.GetAllEmissionGroupsAndDataByOrganizationId(organizationId, consumptionStart, consumptionEnd);

                    var globalActivityDataList = new List<RollForwardActivityDataModel>();

                    // for each emission group get all the activity data within the date range and add it to the "global" activity data list
                    foreach (var emissionGroup in emissionGroupsList)
                    {
                        Guid emissionGroupId = emissionGroup.EmissionGroupId;

                        var activityDataList = await _activityDataService.GetActivityDataByOrganizationAndEmissionGroup(organizationId, emissionGroupId, consumptionStart, consumptionEnd);

                        globalActivityDataList.AddRange(activityDataList.Items);

                    }

                    // first check if there's any activity data
                    if (globalActivityDataList.Any())
                    {
                        // for each item from the "global" activity data list, get the emission source id and depending on that, call the appropiate method to roll forward the data
                        foreach (var activityData in globalActivityDataList)
                        {
                            int emissionSourceId = activityData.EmissionSourceId;

                            switch (emissionSourceId)
                            {
                                case (int)CarbonCompute.GHG.EmissionSourceEnum.MobileCombustion:
                                    await _activityDataService.AddMobileCombustionDataAsync(activityData, consumptionStart, targetPeriodStart, targetPeriodEnd);
                                    break;
                                case (int)CarbonCompute.GHG.EmissionSourceEnum.PurchasedElectricity:
                                    await _activityDataService.AddPurchasedElectricityDataAsync(activityData, consumptionStart, targetPeriodStart, targetPeriodEnd);
                                    break;
                                case (int)CarbonCompute.GHG.EmissionSourceEnum.StationaryCombustion:
                                    await _activityDataService.AddStationaryCombustionDataAsync(activityData, consumptionStart, targetPeriodStart, targetPeriodEnd);
                                    break;
                                case (int)CarbonCompute.GHG.EmissionSourceEnum.BusinessTravel:
                                    await _activityDataService.AddBusinessTravelDataAsync(activityData, consumptionStart, targetPeriodStart, targetPeriodEnd);
                                    break;
                                case (int)CarbonCompute.GHG.EmissionSourceEnum.EmployeeCommuting:
                                    await _activityDataService.AddEmployeeCommuteDataAsync(activityData, consumptionStart, targetPeriodStart, targetPeriodEnd);
                                    break;
                                case (int)CarbonCompute.GHG.EmissionSourceEnum.EndOfLifeTreatmentOfSoldProducts:
                                    await _activityDataService.AddEndOfLifeTreatmentDataAsync(activityData, consumptionStart, targetPeriodStart, targetPeriodEnd);
                                    break;
                                case (int)CarbonCompute.GHG.EmissionSourceEnum.FugitiveEmissions:
                                    await _activityDataService.AddFugitiveEmissionsDataAsync(activityData, consumptionStart, targetPeriodStart, targetPeriodEnd);
                                    break;
                                case (int)CarbonCompute.GHG.EmissionSourceEnum.PurchasedGoodsAndServices:
                                    await _activityDataService.AddPurchasedProductsDataAsync(activityData, consumptionStart, targetPeriodStart, targetPeriodEnd);
                                    break;
                                case (int)CarbonCompute.GHG.EmissionSourceEnum.UpstreamTransportation:
                                    await _activityDataService.AddTransportAndDistributionDataAsync(activityData, consumptionStart, targetPeriodStart, targetPeriodEnd);
                                    break;
                                case (int)CarbonCompute.GHG.EmissionSourceEnum.WasteGeneratedInOperations:
                                    await _activityDataService.AddWasteGeneratedDataAsync(activityData, consumptionStart, targetPeriodStart, targetPeriodEnd);
                                    break;
                                case (int)CarbonCompute.GHG.EmissionSourceEnum.UseOfSoldProducts:
                                    await _activityDataService.AddUseOfSoldProductsDataAsync(activityData, consumptionStart, targetPeriodStart, targetPeriodEnd);
                                    break;
                                case (int)CarbonCompute.GHG.EmissionSourceEnum.FuelAndEnergyRelatedActivitites:
                                    await _activityDataService.AddFuelAndEnergyDataAsync(activityData, consumptionStart, targetPeriodStart, targetPeriodEnd);
                                    break;
                            }
                        }
                        return true;
                    }
                    else
                    {
                        throw new UserFriendlyException("No activity data found for the selected period.");
                    }
                }
                else
                {
                    throw new UserFriendlyException(
                        "The target period already has activity data.", 
                        "In order to prevent duplicate data, the Roll Forward method cannot continue.");
                }
            }
            catch (UserFriendlyException userException)
            {
                throw new UserFriendlyException(userException.Message, userException.Details);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Method: RollForwardActivityData - Exception: {exception}");
                return false;
            }
        }

        /// <summary>
        /// Checks if an Organization has existing, not deleted activity data for a specific date range. <br/>
        /// Used for Roll Forward functionality in order to prevent duplicate data being added.
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="targetPeriodStart"></param>
        /// <param name="targetPeriodEnd"></param>
        /// <returns></returns>
        public async Task<bool> HasNoActivityData(Guid organizationId, DateTime targetPeriodStart, DateTime targetPeriodEnd)
        {
            try
            {
                var hasActivityData = await _dbContext.ActivityData
                    .Include(x => x.OrganizationUnit)
                    .AnyAsync(x => x.OrganizationUnit.OrganizationId == organizationId 
                        && x.ConsumptionStart >= targetPeriodStart
                        && x.ConsumptionEnd <= targetPeriodEnd
                        && x.IsDeleted == false);
                return !hasActivityData;
            }
            catch (Exception exception)
            {
                _logger.LogError($"Method: HasNoActivityData - Exception: {exception}");
                return false;
            }
        }

    }
}
