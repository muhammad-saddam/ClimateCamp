using Abp.Application.Editions;

namespace ClimateCamp.Core.Editions
{
    public class CustomEdition : Edition
    {
        public string PriceLabel { get; set; }
        public string Image { get; set; }
        public bool IsContactSales { get; set; }
        public bool? IsActive { get; set; }
    }
}
