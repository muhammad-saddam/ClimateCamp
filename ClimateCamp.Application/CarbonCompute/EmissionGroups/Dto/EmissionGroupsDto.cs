using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClimateCamp.Application
{
    [AutoMapFrom(typeof(ClimateCamp.CarbonCompute.EmissionGroups))]
    public class EmissionGroupsDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public Guid? OrganizationId { get; set; }
        public int? EmissionSourceId { get; set; }
        public Guid? ParentEmissionGroupId { get; set; }
        public ICollection<EmissionGroupsDto> Children { get; set; }

        #region Additional Usefull properties not mapped to actual DB Entity
        [NotMapped]
        public string Label { get; set; }
        [NotMapped]
        public bool Expanded { get; set; }
        [NotMapped]
        public string Type { get; set; } = "emission-group";
        [NotMapped]
        public string StyleClass { get; set; } = "p-person";
        [NotMapped]
        public Data Data { get; set; }
        [NotMapped]
        public string TreeSelectIcon { get; set; }
        [NotMapped]
        public string EmissionSourceName { get; set; }
        #endregion
    }

   public class Data
    {
        public string Name { get; set; }
        public string Avatar { get; set; }
    }
}
