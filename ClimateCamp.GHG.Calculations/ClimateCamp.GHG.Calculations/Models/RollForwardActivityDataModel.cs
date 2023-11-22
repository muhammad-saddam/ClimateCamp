using System;
using ClimateCamp.CarbonCompute;
using ClimateCamp.Core.CarbonCompute.Enum;
using static ClimateCamp.CarbonCompute.GHG;

namespace ClimateCamp.GHG.Calculations.Models
{
    /// <summary>
    /// Model used by the Roll Forward Azure function.
    /// </summary>
    public class RollForwardActivityDataModel
    {
        /// <summary>
        /// activity data Id
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// Activity Name
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// emission data associated with activity
        /// </summary>
        public float? Emission { get; set; }
        /// <summary>
        /// Organization Unit data assocated with acivity
        /// </summary>
        public string? OrganizationUnit { get; set; }
        /// <summary>
        /// consumption start - consumption end  period
        /// </summary>
        /// 
        public Guid OrganizationUnitId { get; set; }
        /// <summary>
        /// consumption start - consumption end  period
        /// </summary>
        /// 
        public int EmissionSourceId { get; set; }
        /// <summary>
        /// consumption start - consumption end  period
        /// </summary>
        public string? Period { get; set; }
        /// <summary>
        /// Activity type id.
        /// </summary>
        public int? ActivityTypeId { get; set; }
        /// <summary>
        /// Activity data calculated emissions or not
        /// </summary>
        public bool? IsProcessed { get; set; }

        /// <summary>
        /// Activity data deleted or not
        /// </summary>
        public bool IsDeleted { get; set; }
        /// <summary>
        /// Quantity of activity data
        /// </summary>
        public float? Quantity { get; set; }
        /// <summary>
        /// Unit used for activity
        /// </summary>
        public int UnitId { get; set; }
        public string QuantityUnitName { get; set; }
        /// <summary>
        /// consumption start date
        /// </summary>
        public DateTime? ConsumptionStart { get; set; }
        /// <summary>
        /// consumption end date
        /// </summary>
        public DateTime? ConsumptionEnd { get; set; }
        /// <summary>
        /// transaction date
        /// </summary>
        public DateTime? TransactionDate { get; set; }
        /// <summary>
        /// Transaction Source of Activity
        /// </summary>
        public string TransactionSource { get; internal set; }

        /// <summary>
        /// Activity Description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// CO2e - default in kg
        /// </summary>
        public float? CO2e { get; set; }
        /// <summary>
        /// unit of CO2e
        /// </summary>
        public int? CO2eUnitId { get; set; }
        public int? EmissionsDataQualityScore { get; set; }
        public Guid? GreenhouseGasId { get; set; }
        public string? GreenhouseGasCode { get; set; }
        public Guid? VehicleTypeId { get; set; }
        public string? VehicleTypeName { get; set; }
        public Guid? EmissionGroupId { get; set; }
        public string GroupName { get; set; }
        /// <summary>
        /// This property is for the <see cref="WasteGeneratedData"/> entity.
        /// </summary>
        public int? WasteTreatmentMethod { get; set; }

        public string EmissionSourceName { get; set; }
        public string SourceTransactionId { get; set; }

        #region UpStream Transportation
        public string SupplierOrganization { get; set; }
        public float? GoodsQuantity { get; set; }
        public int? GoodsUnitId { get; set; }
        public float? Distance { get; set; }
        public int? DistanceUnitId { get; set; }
        /// <summary>
        /// Indicates if Upstream or Downstream. See <see cref="TransportAndDistributionType"/>
        /// </summary>
        public int? TransportType { get; set; }
        #endregion

        #region Purchased Goods And Services
        public Guid PurchasedProductId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductCode { get; set; }
        public float? ProductCO2eq { get; set; }
        public int? ProductCO2eqUnitId { get; set; }
        public int? ProductUnitId { get; set; }
        public string TransactionId { get; set; }

        public DataQualityType DataQualityType { get; set; }
        public int Status { get; set; }
        #endregion

        #region Emission Factor
        public Guid? EmissionFactorId { get; set; }
        public string EmissionFactorName { get; set; }
        public string EmissionFactorLibraryName { get; set; }
        public int? Year { get; set; }
        public float? EmissionFactorCO2e { get; set; }
        public string EmissionFactorCO2eUnitName { get; set; }
        public int? EmissionFactorCO2eUnitId { get; set; }
        #endregion

        #region Use Of sold Product
        public float? FuelUsedPerUseOfProduct { get; set; }
        public int? FuelUnitId { get; set; }
        public float? ElectricityConsumptionPerUseOfProduct { get; set; }
        public int? ElectricityUnitId { get; set; }
        public float? RefrigerantLeakagePerUseOfProduct { get; set; }
        public int? RefrigerantLeakageUnitId { get; set; }
        #endregion


        public string EnergyMix { get; set; }
        #region Fuel and Energy Related Activities excluded from Scope 1 and 2
        public Guid? FuelTypeId { get; set; }
        public int? EnergyType { get; set; }
        #endregion
    }
}
