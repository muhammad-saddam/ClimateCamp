namespace ClimateCamp.GHG.Calculations.Models
{
    public class GenericEmissionCalculationModel
    {
        public int UnitId { get; set; }
        public float Emission { get; set; }
        public bool IsConversionFactorExist { get; set; } = false;
        public float ConversionFactor { get; set; } = 0;
    }
}
