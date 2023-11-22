using ClimateCamp.EmailClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimateCamp.EmailClient.Models
{
    public class SendDataRequestToOrganizationEmail : EmailSenderModel
    {
        public string requesterFullName { get; set; }
        public string requesterEmailAdress { get; set; }
        public string requesterOrganizationName { get; set; }
        public string[] targetedOrganizationsName { get; set; }
    }
}
