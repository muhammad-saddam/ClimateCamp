using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ClimateCamp.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClimateCamp.Application
{
    [AutoMapFrom(typeof(Core.EmissionsSummary))]
    public class EmissionsSummaryDto : EntityDto<Guid>
    {
        public Guid? OrganizationUnitId { get; set; }
        public int? Period { get; set; }
        /// <summary>
        /// Production quantity of the organization unit in the reference year, unit specified in Production Metrics
        /// </summary>
        public float? ProductionQuantity { get; set; }
        /// <summary>
        /// Unit of Production Quantity
        /// </summary>
        public int ProductionMetricId { get; set; }
        /// <summary>
        /// Enable or disable Scope1tCO2e
        /// </summary>
        public bool? IsActiveScope1Emissions { get; set; }
        public float? Scope1tCO2e { get; set; }
        /// <summary>
        /// Enable or disable Scope2tCO2e
        /// </summary>
        public bool? IsActiveScope2Emissions { get; set; }
        /// <summary>
        /// Total of Scope 2 CO2e in tons for the organization unit in the referenced year
        /// </summary>
        public float? Scope2tCO2e { get; set; }
        /// <summary>
        /// Location based or market based
        /// </summary>
        public int Scope2Methodology { get; set; }
        /// <summary>
        /// Enable or disable Scope3tCO2e
        /// </summary>
        public bool? IsActiveScope3Emissions { get; set; }
        /// <summary>
        /// Total of Scope 3 CO2e in tons for the organization unit in the referenced year
        /// </summary>
        public float? Scope3tCO2e { get; set; }
        /// <summary>
        /// Percentage of primary data in Scope 3 emissions data
        /// </summary>
        public float? Scope3PrimaryDataShare { get; set; }
        /// <summary>
        /// Total PCF for scope 3 emissions data
        /// </summary>
        public float? TotalPCfProxy { get; set; }
        /// <summary>
        /// Is the emissions data audited?
        /// </summary>
        public bool Audited { get; set; }
        /// <summary>
        /// Auditor: required if Audited
        /// </summary>
        public string? Auditor { get; set; }
        /// <summary>
        /// Audit certificate for emissions
        /// </summary>
        public string Certificate { get; set; }
        /// <summary>
        /// Is the CO2e summary calculated per production unit (rather than total production volume)?
        /// </summary>
        public bool IsCO2ePerProductionUnitActive { get; set; }
        /// <summary>
        /// Scope 1 CO2e per production unit in tons for the organization unit in the referenced year
        /// </summary>
        public float? Scope1CO2ePPU { get; set; }
        /// <summary>
        /// Scope 3 CO2e per production unit in tons for the organization unit in the referenced year
        /// </summary>
        public float? Scope2CO2ePPU { get; set; }
        /// <summary>
        /// Scope 3 CO2e per production unit in tons for the organization unit in the referenced year
        /// </summary>
        public float? Scope3CO2ePPU { get; set; }
        /// <summary>
        /// Are the Scope 1 emissions per emission source present?
        /// </summary>
        public bool IsScope1DetailViewActive { get; set; }
        /// <summary>
        /// Are the Scope 2 emissions per emission source present?
        /// </summary>
        public bool IsScope2DetailViewActive { get; set; }
        /// <summary>
        /// Are the Scope 3 emissions per emission source present?
        /// </summary>
        public bool IsScope3DetailViewActive { get; set; }
        /// <summary>
        /// will be used to represent if the emission summary was saved as draft or not
        /// </summary>
        public bool IsDraft { get; set; }
        [NotMapped]
        public List<EmissionsSummaryScopeDetails> EmissionsSummaryScopeDetails { get; set; }



    }
}