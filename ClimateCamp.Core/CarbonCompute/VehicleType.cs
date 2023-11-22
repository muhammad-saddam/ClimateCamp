using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClimateCamp.CarbonCompute
{
    /// <summary>
    /// Values https://climatecamp.sharepoint.com/:x:/r/sites/ProductDev/Shared%20Documents/General/08.%20Ecosystem/Microsoft/Active%20Vehicle%20Types%2011-9-2021%2010-27-06%20PM.xlsx?d=wa92b83d5c92d4368a9caa3dfb09fc81f&csf=1&web=1&e=Wq0e3L
    /// </summary>
    [Table("VehicleTypes", Schema = "Reference")]
    public class VehicleType : Entity<Guid>
    {

        public VehicleType(string name, string description, int modeOfTransport, int transportationKind)
        {
            this.Name = name;
            this.Description = description;
            this.ModeOfTransport = modeOfTransport;
            this.TransportationKind = transportationKind;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// Road, Rail, Air, etc <br/>
        /// <see cref="GHG.ModeOfTransport"/>
        /// </summary>
        public int ModeOfTransport { get; set; }
        /// <summary>
        /// Travel or Freight <br/>
        /// <see cref="GHG.TransportationKind"/>
        /// </summary>
        public int TransportationKind { get; set; }
    }
}
