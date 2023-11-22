namespace ClimateCamp.Application
{
    public class ProductDataConfirmationVM
    {
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }
        public int ProductType { get; set; }
        public bool Selected { get; set; } = false;
    }
}
