using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ClimateCamp.Application;
using ClimateCamp.Common.Roles.Dto;
using ClimateCamp.Common.Users.Dto;
using ClimateCamp.Users.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClimateCamp.Common.Users
{
    public interface IUserAppService : IAsyncCrudAppService<UserDto, long, PagedUserResultRequestDto, CreateUserDto, UserDto>
    {
        Task DeActivate(EntityDto<long> user);
        Task Activate(EntityDto<long> user);
        Task<ListResultDto<RoleDto>> GetRoles();
        Task ChangeLanguage(ChangeUserLanguageDto input);

        Task<bool> ChangePassword(ChangePasswordDto input);

        Task<bool> SetPassword(SetPasswordDto input);
        Task<bool> CheckIfEmailAddressAlreadyTaken(string email);
        Task<Guid> CreateSelfServiceOrganization(CreateSelfServiceOrganizationDto input);
        Task<decimal> InitialStageFootprintCalculation(InitialStageFootprintCalculationDto input);
        Task<UserDto> GetSelfServiceUserByEmail(string email);
        Task<OrganizationDto> GetSelfServiceOrganizationById(Guid organizationId);
        Task<List<IndustriesGroup>> GetGroupedIndustries();
    }
}
