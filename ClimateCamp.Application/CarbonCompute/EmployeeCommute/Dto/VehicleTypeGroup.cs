using System;
using System.Collections.Generic;

namespace ClimateCamp.Application
{
    public class VehicleTypeGroup
    {
        public string label { get; set; }
        public int data { get; set; }
        public string expandedIcon { get; set; }
        public string collapsedIcon { get; set; }
        public List<VTChild> Children { get; set; }
    }

    public class VTChild
    {
        public string label { get; set; }
        public Guid data { get; set; }
    }

}