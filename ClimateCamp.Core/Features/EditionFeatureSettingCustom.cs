using Abp.Application.Features;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClimateCamp.Core.Features
{
    public enum EditionFeartureType
    {
        Feature = 1,
        EmissionSource = 2
    }
    public class EditionFeatureSettingCustom : EditionFeatureSetting
    {
        public string Icon { get; set; }
        public bool IsActive { get; set; }
        public bool ShowActiveLabel { get; set; }
        public long? ParentId { get; set; }
        public int? Type { get; set; }
        [NotMapped]
        public List<EditionFeatureSettingCustom> ChildFeatures { get; set; }

        public EditionFeatureSettingCustom()
        {
            ChildFeatures = new List<EditionFeatureSettingCustom>();
            Value = string.Empty;
            IsActive = true;
            ShowActiveLabel = false;
            ParentId = null;
        }
    }
}
