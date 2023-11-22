namespace Mobile.Combustion.Calculation.Models
{
    public class EmissionFactorModel
    {
        public float Co2 { get; set; }
        public int? Co2unitId { get; set; }
        public float Ch4 { get; set; }
        public int? Ch4unitId { get; set; }
        public float N20 { get; set; }
        public int? N20unitId { get; set; }
        public float CO2e { get; set; }
        public int? Co2eunitId { get; set; }
    }
}
