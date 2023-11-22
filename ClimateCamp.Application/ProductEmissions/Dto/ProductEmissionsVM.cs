using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimateCamp.Application
{
    public class ProductEmissionsVM : ProductEmissionsDto
    {
        public ICollection<ProductsEmissionSourcesDto> PreviousProductsEmissionSources { get; set; }
    }
}
