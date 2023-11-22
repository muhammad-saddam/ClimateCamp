using AutoMapper;
using ClimateCamp.Lookup;

namespace ClimateCamp.Application
{
    public class OrganizationUnitMapProfile : Profile
    {
        public OrganizationUnitMapProfile()
        {
            CreateMap<OrganizationUnitDto, Country>()
                .ForMember(x => x.Name, opt => opt.MapFrom(input => input.CountryName));
        }
    }
}
