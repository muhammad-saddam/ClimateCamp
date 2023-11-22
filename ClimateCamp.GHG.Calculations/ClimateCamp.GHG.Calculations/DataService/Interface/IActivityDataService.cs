using Abp.Application.Services.Dto;
using ClimateCamp.Application;
using ClimateCamp.CarbonCompute;
using ClimateCamp.GHG.Calculations.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mobile.Combustion.Calculation.DataService
{
    public interface IActivityDataService
    {
        Task<ActivityData> SaveActivityData(ActivityData activity);
        Task<ActivityData> UpdateActivityData(ActivityData activity);
        Task<List<ActivityData>> getMobileCombustionDistanceActivityData(string organizationId, int emissionSourceId);
        Task<PagedResultDto<RollForwardActivityDataModel>> GetActivityDataByOrganizationAndEmissionGroup(Guid organizationId, Guid emissionGroupId, DateTime consumptionStart, DateTime consumptionEnd);
        Task<bool> AddMobileCombustionDataAsync(RollForwardActivityDataModel activityData, DateTime sourcePeriodStart, DateTime targetPeriodStart, DateTime targetPeriodEnd);
        Task<bool> AddPurchasedElectricityDataAsync(RollForwardActivityDataModel activityData, DateTime sourcePeriodStart, DateTime targetPeriodStart, DateTime targetPeriodEnd);
        Task<bool> AddStationaryCombustionDataAsync(RollForwardActivityDataModel activityData, DateTime sourcePeriodStart, DateTime targetPeriodStart, DateTime targetPeriodEnd);
        Task<bool> AddBusinessTravelDataAsync(RollForwardActivityDataModel activityData, DateTime sourcePeriodStart, DateTime targetPeriodStart, DateTime targetPeriodEnd);
        Task<bool> AddEmployeeCommuteDataAsync(RollForwardActivityDataModel activityData, DateTime sourcePeriodStart, DateTime targetPeriodStart, DateTime targetPeriodEnd);
        Task<bool> AddEndOfLifeTreatmentDataAsync(RollForwardActivityDataModel activityData, DateTime sourcePeriodStart, DateTime targetPeriodStart, DateTime targetPeriodEnd);
        Task<bool> AddFugitiveEmissionsDataAsync(RollForwardActivityDataModel activityData, DateTime sourcePeriodStart, DateTime targetPeriodStart, DateTime targetPeriodEnd);
        Task<bool> AddPurchasedProductsDataAsync(RollForwardActivityDataModel activityData, DateTime sourcePeriodStart, DateTime targetPeriodStart, DateTime targetPeriodEnd);
        Task<bool> AddTransportAndDistributionDataAsync(RollForwardActivityDataModel activityData, DateTime sourcePeriodStart, DateTime targetPeriodStart, DateTime targetPeriodEnd);
        Task<bool> AddWasteGeneratedDataAsync(RollForwardActivityDataModel activityData, DateTime sourcePeriodStart, DateTime targetPeriodStart, DateTime targetPeriodEnd);
        Task<bool> AddUseOfSoldProductsDataAsync(RollForwardActivityDataModel activityData, DateTime sourcePeriodStart, DateTime targetPeriodStart, DateTime targetPeriodEnd);
        Task<bool> AddFuelAndEnergyDataAsync(RollForwardActivityDataModel activityData, DateTime sourcePeriodStart, DateTime targetPeriodStart, DateTime targetPeriodEnd);
    }
}
