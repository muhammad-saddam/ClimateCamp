using Abp.Domain.Entities;

namespace ClimateCamp.Lookup
{
    public class Country : Entity
    {
        public Country()
        {
        }

        public Country(string Name, string TwoCharCode, string ThreeCharCode)
        {
            this.Name = Name;
            this.TwoCharCode = TwoCharCode;
            this.ThreeCharCode = ThreeCharCode;
        }

        public string Name { get; set; }
        public string TwoCharCode { get; set; }
        public string ThreeCharCode { get; set; }
    }
}
