using AutoMapper;
using ClimateCamp.Core;

namespace ClimateCamp.Application
{
    public class OrganizationMapProfile : Profile
    {
        public OrganizationMapProfile()
        {
            CreateMap<CreateOrganizationDto, Organization>();
        }
    }
}
