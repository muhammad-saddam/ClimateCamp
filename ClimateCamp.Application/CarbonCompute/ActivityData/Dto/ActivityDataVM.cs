using ClimateCamp.CarbonCompute;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using static ClimateCamp.CarbonCompute.GHG;

namespace ClimateCamp.Application
{
    /// <summary>
    /// Model used to display activitieslist with emission and org unit
    /// </summary>
    public class ActivityDataVM
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
        /// Unit used for activity
        /// </summary>
        public int UnitId { get; set; }
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
        /// <summary>
        /// activity data status:
        /// <see cref="Core.CarbonCompute.Enum.ActivityDataStatus"/>
        /// </summary>
        public int ActivityDataStatus { get; set; }

        #region UpStream Transportation
        public string SupplierOrganization { get; set; }
        public float? GoodsQuantity { get; set; }
        public int? GoodsUnitId { get; set; }
        public float? Distance { get; set; }
        public int? DistanceUnitId { get; set; }
        #endregion

        #region Purchased Goods And Services
        public Guid PurchasedProductId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductCode { get; set; }
        public float? ProductCO2eq { get; set; }
        public int? ProductCO2eqUnitId { get; set; }
        public int? ProductUnitId { get; set; }
        public string TransactionId { get; set; }
        public int? ActivityTypeId { get; set; }
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

        public string EnergyMix { get; set; }

        #region Use Of sold Product
        public float? FuelUsedPerUseOfProduct { get; set; }
        public int? FuelUnitId { get; set; }
        public float? ElectricityConsumptionPerUseOfProduct { get; set; }
        public int? ElectricityUnitId { get; set; }
        public float? RefrigerantLeakagePerUseOfProduct { get; set; }
        public int? RefrigerantLeakageUnitId { get; set; }
        #endregion

        #region Fuel and Energy Related Activities excluded from Scope 1 and 2
        public Guid? FuelTypeId { get; set; }
        public int? EnergyType { get; set; }
        #endregion

        public string  SupplierName  { get; set; }

        public int? EmissionType { get; set; }
        public float? PerUnitCO2e { get; set; }
        public string PerUnitCO2eUnit { get; set; }
        public string ProductionQuantityUnit { get; set; }

    }
}
