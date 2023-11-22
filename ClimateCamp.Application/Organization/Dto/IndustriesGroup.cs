using Abp.Application.Services.Dto;
using ClimateCamp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimateCamp.Application
{
    public class IndustriesGroup
    {
        public string label { get; set; }
        public int data { get; set; }
        public string expandedIcon { get; set; }
        public string collapsedIcon { get; set; }
        public List<IndustryChild> Children { get; set; }
    }

    public class IndustryChild
    {
        public string label { get; set; }
        public int data { get; set; }
        public bool IsPriority { get; set; }
    }
}
