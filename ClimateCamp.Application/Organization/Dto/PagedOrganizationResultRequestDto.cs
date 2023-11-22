using Abp.Application.Services.Dto;

namespace ClimateCamp.Application
{
    public class PagedOrganizationResultRequestDto : PagedResultRequestDto
    {
        public bool? IsActive { get; set; }
    }
}
