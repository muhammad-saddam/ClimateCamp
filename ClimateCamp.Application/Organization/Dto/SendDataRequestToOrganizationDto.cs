using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimateCamp.Application
{
    public class SendDataRequestToOrganizationDto { 
        //Need to change string to GUID if the list becomes dynamic
        public string[] OrganizationsId { get; set; }
    }
}
