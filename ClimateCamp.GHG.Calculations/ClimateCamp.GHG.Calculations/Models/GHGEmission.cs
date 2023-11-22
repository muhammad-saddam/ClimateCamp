using System;

namespace Mobile.Combustion.Calculation.Models
{
    public class GHGEmission
    {
        public Guid Id { get; set; }
        public long? UserId { get; set; }
        public Guid? OrganizationUnitId { get; set; }
        public double CO2Emission { get; set; }
        public double CH4Emission { get; set; }
        public double N2OEmission { get; set; }
        public double CO2eEmission { get; set; }
        public DateTime Date { get; set; }
    }
}
