

using ClimateCamp.CarbonCompute;

namespace ClimateCamp.Application
{
    public class EmissionSourcesVM
    {
        public GHG.EmissionScope EmissionScope { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Scope { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; }
        public int Id { get; set; }

        public double TotalEmissions { get; set; }
    }
}
