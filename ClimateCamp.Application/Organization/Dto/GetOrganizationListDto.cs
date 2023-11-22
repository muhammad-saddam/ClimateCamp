using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ClimateCamp.Core;

namespace ClimateCamp.Application
{


    [AutoMapFrom(typeof(Organization))]
    public class GetOrganizationListDto : EntityDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
        public string PhoneNumber { get; set; }
        public string VATNumber { get; set; }
        public int BillingPreferences { get; set; }
        public int? Country { get; set; }
    }
}
