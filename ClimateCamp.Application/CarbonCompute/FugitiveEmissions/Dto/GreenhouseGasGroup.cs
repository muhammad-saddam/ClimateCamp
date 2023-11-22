using System;
using System.Collections.Generic;

namespace ClimateCamp.Application
{
    public class GreenhouseGasGroup
    {
        public string label { get; set; }
        public int data { get; set; }
        public string expandedIcon { get; set; }
        public string collapsedIcon { get; set; }
        public ICollection<Child> Children { get; set; }
    }

    public class Child
    {
        public string label { get; set; }
        public Guid data { get; set; }
    }

}
