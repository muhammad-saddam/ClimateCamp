using System.Collections.Generic;

namespace ClimateCamp.Application
{
    public class ProductEmissionGroupedVM
    {
        public int? Year { get; set; }
        public List<ProductEmissionTypesVM> Emissions { get; set; }
    }
}
