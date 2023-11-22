using Abp.Application.Services.Dto;

namespace ClimateCamp.Common.Users.Dto
{
    //custom PagedResultRequestDto
    public class PagedUserResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public bool? IsActive { get; set; }
    }
}
