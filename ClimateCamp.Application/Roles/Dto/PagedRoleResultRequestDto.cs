using Abp.Application.Services.Dto;

namespace ClimateCamp.Common.Roles.Dto
{
    public class PagedRoleResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
    }
}

