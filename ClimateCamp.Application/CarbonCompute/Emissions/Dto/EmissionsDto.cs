using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.ComponentModel;

namespace ClimateCamp.Application
{
    [AutoMapFrom(typeof(ClimateCamp.CarbonCompute.Emission))]
    public class EmissionsDto : EntityDto<Guid>
    {
        public Guid? OrganizationUnitId { get; set; }
        public Guid? ActivityDataId { get; set; }
        public int ActivityTypeId { get; set; }
        public Guid EmissionsFactorsLibraryId { get; set; }
        public Guid ResponsibleEntityID { get; set; }
        public int ResponsibleEntityType { get; set; }
        public bool IsActive { get; set; }

        #region Gas Factors
        public float CO2 { get; set; }
        public int CO2Unit { get; set; }
        public float CH4 { get; set; }
        public int CH4Unit { get; set; }
        public float N20 { get; set; }
        public int N20Unit { get; set; }
        public float HFCs { get; set; }
        public int HFCsUnit { get; set; }
        public float NF3 { get; set; }
        public int NF3Unit { get; set; }
        public float PFCs { get; set; }
        public int PFCsUnit { get; set; }
        public float SF6 { get; set; }
        public int SF6Unit { get; set; }
        public float CO2E { get; set; }
        public int CO2EUnit { get; set; }
        public float OtherGHGs { get; set; }
        public int OtherGHGsUnit { get; set; }
        #endregion
        public float? CO2eFactor { get; set; }
        public int? CO2eFactorUnitId { get; set; }
        [DefaultValue(1)]
        public int Version { get; set; }
    }
}
