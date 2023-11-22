using System.ComponentModel;

namespace ClimateCamp.Core.CarbonCompute.Enum
{
    public enum CrossSectoralStandards
    {
        // TODO: change this so that it returns the names with spaces instead of the numbers
        [Description("GHG Protocol Product Standard")]
        GHGProtocolProductStandard = 0,
        [Description("ISO Standard 14067")]
        ISOStandard14067 = 1,
        [Description("ISO Standard 14044")]
        ISOStandard14044 = 2
    }
}
