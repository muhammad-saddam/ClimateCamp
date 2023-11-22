using Abp;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Linq.Extensions;
using Abp.Localization;
using Abp.MultiTenancy;
using Abp.Runtime.Session;
using Abp.UI;
using Castle.Core.Logging;
using ClimateCamp.Application;
using ClimateCamp.CarbonCompute;
using ClimateCamp.Common.Authorization;
using ClimateCamp.Common.Authorization.Roles;
using ClimateCamp.Common.Authorization.Users;
using ClimateCamp.Common.Dto;
using ClimateCamp.Common.MultiTenancy;
using ClimateCamp.Common.Roles.Dto;
using ClimateCamp.Common.Users.Dto;
using ClimateCamp.Core;
using ClimateCamp.Core.Authorization;
using ClimateCamp.Core.Editions;
using ClimateCamp.EmailClient.Models;
using ClimateCamp.EmailClient.Services;
using ClimateCamp.Infrastructure.AzureADB2C;
using ClimateCamp.Infrastructure.FileUploadService;
using ClimateCamp.Infrastructure.HubSpot;
using ClimateCamp.Infrastructure.Models;
using ClimateCamp.Users.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static ClimateCamp.Core.OrganizationUnit;
using Organization = ClimateCamp.Core.Organization;
using User = ClimateCamp.Common.Authorization.Users.User;

namespace ClimateCamp.Common.Users
{
    public class UserAppService : AsyncCrudAppService<User, UserDto, long, PagedUserResultRequestDto, CreateUserDto, UserDto>, IUserAppService
    {
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IRepository<Role> _roleRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IRepository<Core.Organization, Guid> _organisationRepository;
        private readonly IFileUploadService _fileUploadService;
        private readonly IRepository<User, long> _userRepository;
        private readonly IEmailClientSender _emailClientSender;
        private readonly IConfiguration _config;
        private readonly IGraphClientService _graphClientService;
        private readonly IHubspotService _hubSpotService;
        private readonly IRepository<CustomEdition, int> _editionRepository;
        private readonly ILogger _logger;
        IRepository<OrganizationUnit, Guid> _organisationUnitRepository;
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly IRepository<Industry, int> _industriesRepository;
        private readonly LogInManager _logInManager;
        private readonly AbpLoginResultTypeHelper _abpLoginResultTypeHelper;
        private readonly IEmissionGroupsAppService _emissionGroupsAppService;

        private const int averageCO2EmissionsPerHectolitreOfBeer = 300;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        /// <param name="roleRepository"></param>
        /// <param name="passwordHasher"></param>
        /// <param name="abpSession"></param>
        /// <param name="logInManager"></param>
        /// <param name="fileUploadService"></param>
        /// <param name="organisationRepository"></param>
        /// <param name="userRepository"></param>
        /// <param name="emailClientSender"></param>
        /// <param name="config"></param>
        /// <param name="graphClientService"></param>
        /// <param name="editionRepository"></param>
        /// <param name="logger"></param>
        /// <param name="hubSpotService"></param>
        /// <param name="organisationUnitRepository"></param>
        /// <param name="tenantRepository"></param>
        /// <param name="industriesRepository"></param>
        /// <param name="emissionGroupsAppService"></param>
        public UserAppService(
            IRepository<User, long> repository,
            UserManager userManager,
            RoleManager roleManager,
            IRepository<Role> roleRepository,
            IPasswordHasher<User> passwordHasher,
            IAbpSession abpSession,
            LogInManager logInManager,
            IFileUploadService fileUploadService,
            IRepository<Organization, Guid> organisationRepository,
            IRepository<User, long> userRepository,
            IEmailClientSender emailClientSender,
            IConfiguration config,
            IGraphClientService graphClientService,
            IHubspotService hubSpotService,
            IRepository<CustomEdition, int> editionRepository,
            ILogger logger,
            IRepository<OrganizationUnit, Guid> organisationUnitRepository,
            IRepository<Tenant> tenantRepository,
            IRepository<Industry, int> industriesRepository,
            IEmissionGroupsAppService emissionGroupsAppService
            )
            : base(repository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _roleRepository = roleRepository;
            _passwordHasher = passwordHasher;
            _organisationRepository = organisationRepository;
            _fileUploadService = fileUploadService;
            _userRepository = userRepository;
            _emailClientSender = emailClientSender;
            _config = config;
            _graphClientService = graphClientService;
            _hubSpotService = hubSpotService;
            _editionRepository = editionRepository;
            _logger = logger;
            _organisationUnitRepository = organisationUnitRepository;
            _tenantRepository = tenantRepository;
            _industriesRepository = industriesRepository;

            _logInManager = logInManager;
            _emissionGroupsAppService = emissionGroupsAppService;
        }

        public override Task<PagedResultDto<UserDto>> GetAllAsync(PagedUserResultRequestDto input)
        {
            throw new NotImplementedException();
        }
        public async Task<PagedResultDto<UserDto>> GetAllByOrganizationAsync(GetAllAsyncDto input)
        {


            var organizationUsers = await _userRepository.GetAll()
                .Include(t => t.Roles)
                .Where(t => t.OrganizationId == input.OrganizationId).ToListAsync();
            var userList = new List<UserDto>();
            var allRoles = await _roleManager.Roles.ToListAsync();
            foreach (var user in organizationUsers)
            {
                var roleIds = user.Roles.Select(x => x.RoleId).ToArray();
                var userRoles = allRoles.Where(r => roleIds.Contains(r.Id)).Select(r => r.DisplayName);

                var userDto = base.MapToEntityDto(user);
                userDto.RoleNames = userRoles.ToArray();
                userList.Add(userDto);
            }

            var result = new PagedResultDto<UserDto>()
            {
                Items = userList,
                TotalCount = organizationUsers.Count
            };
            return result;
        }
        [AbpAllowAnonymous]
        public async Task<UserDto> GetSelfServiceUserByEmail(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);

                return MapToEntityDto(user);
            }
            catch (Exception exception)
            {
                _logger.Error(exception.Message, exception);
                return null;
            }
        }
        [AbpAllowAnonymous]
        public async Task<OrganizationDto> GetSelfServiceOrganizationById(Guid organizationId)
        {
            var organizations = await _organisationRepository.SingleAsync(x => x.Id == organizationId);
            var organization = ObjectMapper.Map<OrganizationDto>(organizations);
            return organization;
        }
        public override async Task<UserDto> CreateAsync(CreateUserDto input)
        {
            CheckCreatePermission();
            var userActivationUrl = "";
            var user = ObjectMapper.Map<User>(input);
            try
            {
                input.Password = !input.IsSelfServiceUser ? SettingManager.GetSettingValue("UserDefaultPassword") : input.Password;
                user.Surname = user.Surname;
                user.Name = user.Name ?? "";
                user.UserName = input.EmailAddress;
                user.TenantId = AbpSession.TenantId;
                user.IsLockoutEnabled = false;
                user.IsActive = input.IsSelfServiceUser;
                user.IsFirstLoginExperience = input.IsSelfServiceUser ? true : input.IsFirstLoginExperience;

                await _userManager.InitializeOptionsAsync(AbpSession.TenantId);

                CheckErrors(await _userManager.CreateAsync(user, input.Password));

                if (input.RoleNames != null && input.RoleNames.Count() > 0)
                {
                    CheckErrors(await _userManager.SetRolesAsync(user, input.RoleNames));
                }
                CurrentUnitOfWork.SaveChanges();
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                if (input.IsSelfServiceUser)
                {
                    var loginResult = await GetLoginResultAsync(
                   input.EmailAddress,
                   input.Password,
                   AbpSession.TenantId.ToString()
               );
                    var accessToken = CreateAccessToken(CreateJwtClaims(loginResult.Identity));
                    userActivationUrl = $"{_config.GetValue<string>("App:ClientUrl")}/{SettingManager.GetSettingValue("SelfRegistrationPageUrl")}?&email={user.EmailAddress}&isEmailConfirmed=true&token={HttpUtility.UrlEncode(token)}&access_token={accessToken}";
                }
                else
                    userActivationUrl = $"{_config.GetValue<string>("App:ClientUrl")}/{SettingManager.GetSettingValue("SetPasswordPageUrl")}?email={user.EmailAddress}&organizationId={user.OrganizationId}&token={HttpUtility.UrlEncode(token)}";

                

                var organization = user.OrganizationId != Guid.Empty ? await _organisationRepository.GetAsync(user.OrganizationId.Value) : null;


                #region send email to user and teams channel

                var fromUser = new User();

                if (!input.IsSelfServiceUser)
                    fromUser = await _userManager.GetUserByIdAsync(AbpSession.UserId ?? 0);

                var emailModel = new ActivateUserEmailDto()
                {
                    LogoUrl = organization?.PicturePath ?? "https://stclimatecampprdeu01.blob.core.windows.net/climatecamp-files/images/climateCamp-browser-logo.png",
                    ActivationLink = userActivationUrl,
                    FromEmail = _config.GetValue<string>("App:FromEmail"),
                    FromName = !input.IsSelfServiceUser ? fromUser.Name + " " + fromUser.Surname : string.Empty,
                    ToEmail = user.EmailAddress,
                    ToName = $"{user.Name} {user.Surname}",
                    FirstName = !input.IsSelfServiceUser ? user.Name : string.Empty,
                    Subject = !input.IsSelfServiceUser ? $"Welcome, {user.Name} {user.Surname}, to ClimateCamp!" : "Welcome to ClimateCamp!",
                    TemplateName = input.IsSelfServiceUser ? "ActivateUserEmail" : "PersonalInvite",
                    IsSelfServiceUser = input.IsSelfServiceUser,
                    SupportLink = "https://meetings-eu1.hubspot.com/laurent-moyersoen",

                };
                await _graphClientService.CreateUser(input);
                await _emailClientSender.SendEmail(emailModel, _config.GetValue<string>("App:Functions:EmailSenderFunctionUrl"));
                if (!input.IsSelfServiceUser)
                {
                    var teamsNotification = new TeamsChannelEmail()
                    {
                        LogoUrl = organization?.PicturePath ?? "",
                        FromEmail = _config.GetValue<string>("App:FromEmail"),
                        ToEmail = _config.GetValue<string>("App:LiveMonitoringEmail"),
                        ToName = "Production Events 📢 - Product Dev",
                        Subject = $"New User registered: {user.Name} {user.Surname} - {user.EmailAddress}",
                        Organization = organization?.Name,
                        UserEmail = user.EmailAddress,
                        UserName = $"New User registered: {user.Name} {user.Surname}",
                        TemplateName = "TeamsChannelEmail",
                        IsSelfServiceUser = input.IsSelfServiceUser,
                        userPhone = input.Phone,
                    };
                    await _emailClientSender.SendEmail(teamsNotification, _config.GetValue<string>("App:Functions:EmailSenderFunctionUrl"));
                }
                else
                {
                    //TODO: this execution path has strange behaviour, and sends a Teams notification with a send team channel without username and email populates.
                    emailModel.ToEmail = _config.GetValue<string>("App:LiveMonitoringEmail");
                    await _emailClientSender.SendEmail(emailModel, _config.GetValue<string>("App:Functions:EmailSenderFunctionUrl"));
                }
                #endregion
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            //TODO: message admin: userName, userRole, userOrg, targetUserId = admin?
            //var organizationName = user.OrganizationId != Guid.Empty ? await _organisationRepository.GetAsync(user.OrganizationId.Value) : null;
            // var userRole = await _roleRepository.GetAll().Where
            //await PublishNewOrganizationUser(user.Name);
            return MapToEntityDto(user);
        }

        /// <summary>
        /// Creating the organization on user registration.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
         [AbpAllowAnonymous]
        public async Task<Guid> CreateSelfServiceOrganization(CreateSelfServiceOrganizationDto input)
        {
            try
            {

                input.TenantId = MultiTenancyConsts.DefaultTenantId;

                var organization = new Organization
                {
                    Name = input.Name,
                    ProductionQuantity = input.ProductionVolume,
                    TenantId = input.TenantId,
                    IsActive = true,
                    IsDeleted = false,
                    EmissionsFactorsLibraryId = ClimateCampConsts.DefaultEmissionsFactorsLibraryId,
                    BillingPreferenceId = input.BillingPreferenceId,
                    ReportingFrequencyId= input.ReportingFrequencyId
                };

                //Assign Edition To SelfService Organization
                //TODO: Refactor using the domain service <see cref="EditionManager"/> instead of a repository, like in <see cref="ClimateCamp.Common.MultiTenancy.TenantAppService.CreateAsync"/> method.
                var edition = await _editionRepository.FirstOrDefaultAsync(x => x.Name == "Business");

                if (edition != null)
                    organization.EditionId = edition.Id;
                else
                {
                    var customEdition = new CustomEdition
                    {
                        Name = "Business",
                        DisplayName = "Climatecamp Business Edition",
                        Image = "../../../assets/img/billings/billing-placeholder.png",
                        PriceLabel = "500€/Month",
                        IsContactSales = true
                    };

                    await _editionRepository.InsertAsync(customEdition);
                    CurrentUnitOfWork.SaveChanges();

                    organization.EditionId = customEdition.Id;
                }

                await _organisationRepository.InsertAsync(organization);
                CurrentUnitOfWork.SaveChanges();

                #region User Story #692 Create First Organization Unit And Tenant

                var orgUnitCreated = await _organisationUnitRepository.GetAll().Where(x => x.OrganizationId == organization.Id).AnyAsync();

                if (!orgUnitCreated)
                {
                    #region Create Organization Unit

                    var organizationUnit = await _organisationUnitRepository.InsertAsync(new OrganizationUnit
                    {
                        Name = organization.Name,
                        Type = OrganizationUnitType.Unit,
                        Address = organization.Address,
                        PhoneNumber = organization.PhoneNumber,
                        VATNumber = organization.VATNumber,
                        MonthlyRevenue = Convert.ToDouble(organization.Revenue),
                        CountryId = organization.CountryId,
                        EffectiveStartDate = DateTime.Now,
                        IsActive = true,
                        IsDeleted = false,
                        OrganizationId = organization.Id,
                        PictureName = organization.PictureName,
                        PicturePath = organization.PicturePath,
                    });

                    CurrentUnitOfWork.SaveChanges();

                    #endregion

                    #region Create Tenant

                    var tenant = await _tenantRepository.InsertAsync(new Tenant
                    {
                        Name = organization.Name,
                        TenancyName = organization.Name,
                        EditionId = organization.EditionId,
                        IsDeleted = false,
                        IsActive = true,
                        CreationTime = DateTime.Now
                    });

                    CurrentUnitOfWork.SaveChanges();

                    #endregion

                    //Update Organization Tenant with new Tenant created
                    var organizationData = _organisationRepository.Get(organization.Id);

                    organizationData.TenantId = tenant.Id;
                    organizationData.LastModificationTime = DateTime.Now;

                    await _organisationRepository.UpdateAsync(organizationData);

                    CurrentUnitOfWork.SaveChanges();
                }

                #endregion

                #region User Story 1119 Create Default Emission Groups Template For Breweries
                await _emissionGroupsAppService.InsertEmissionGroupsFromTemplate("Breweries", organization.Id);
                   // need to fix that when second or more emission groups templates available
                //if (input.IndustryType.Equals("Breweries", StringComparison.OrdinalIgnoreCase))
                //{

                //    //await InsertEmissionGroupsFromTemplate(input.IndustryType, organization.Id);

                //}

                #endregion

                var user = await _userManager.FindByEmailAsync(input.email);
                //Notify team that a new organization was registered

                var teamsNotification = new TeamsChannelEmail()
                {
                    LogoUrl = organization?.PicturePath ?? "",
                    FromEmail = _config.GetValue<string>("App:FromEmail"),
                    ToEmail = _config.GetValue<string>("App:LiveMonitoringEmail"),
                    ToName = "Production Events 📢 - Product Dev",
                    Subject = $"New Organization registered: {organization?.Name}",
                    TemplateName = "TeamsChannelEmail",
                    Organization = organization?.Name,
                    UserEmail = user.EmailAddress,
                    UserName = user?.Name + " " + user.Surname,
                    userPhone = user?.Phone,


                };
                await _emailClientSender.SendEmail(teamsNotification, _config.GetValue<string>("App:Functions:EmailSenderFunctionUrl"));

                //Assign Roles To SelfService User

                //First check if this organization has any user created
                //and what is the role already aasigned to the user

                var organizationUsers = await _userRepository.GetAllListAsync(x => x.OrganizationId == organization.Id);


                if (!organizationUsers.Any())
                    await _userManager.SetRolesAsync(user, new string[] { "OrganizationAdmin" });
                else
                {
                    bool isOrganizationAdminRoleFound = false;

                    foreach (var orgUser in organizationUsers)
                    {
                        var userRoles = await _userManager.GetRolesAsync(orgUser);

                        if (userRoles.Any(x => x == "OrganizationAdmin"))
                        {
                            isOrganizationAdminRoleFound = true;
                            break;
                        }

                    }


                    if (isOrganizationAdminRoleFound) // any of the organization user has already assigned the climatecampadminrole
                        await _userManager.SetRolesAsync(user, new string[] { "SustainabilityManager" });
                    else
                        await _userManager.SetRolesAsync(user, new string[] { "OrganizationAdmin" });

                }

                user.OrganizationId = organization.Id;

                await _userManager.UpdateAsync(user);

                await CurrentUnitOfWork.SaveChangesAsync();

                return organization.Id;
            }
            catch (Exception exception)
            {
                _logger.Error(exception.Message, exception);
                throw;
            }

        }
        [AbpAllowAnonymous]
        public async Task<bool> CheckIfEmailAddressAlreadyTaken(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);

                return user != null;
            }
            catch (Exception exception)
            {
                _logger.Error(exception.Message, exception);
                return false;
            }
        }
        [AbpAllowAnonymous]
        /// <summary>
        /// This is used to calculate the initial stage CO2e footprint based on production volume in hectolitres
        /// </summary>
        /// <returns>Tons of CO2</returns>
        public async Task<decimal> InitialStageFootprintCalculation(InitialStageFootprintCalculationDto input)
        {
            if (input.Id == Guid.Empty)
                throw new UserFriendlyException(message: "Provided Id is invalid");
            if (input.BaseLineEmission < 0)
                throw new UserFriendlyException(message: "BaseLineEmission must be a positive number");

            var organization = await _organisationRepository.GetAsync(input.Id);

            var initialFootprint = input.BaseLineEmission * averageCO2EmissionsPerHectolitreOfBeer / 1000;
            organization.BaseLineYear = DateTime.UtcNow.Year;
            organization.BaseLineEmission = initialFootprint;
            await _organisationRepository.UpdateAsync(organization);
            return initialFootprint;
        }

        public async Task<UserDto> GetUserRolesAsync(EntityDto<long> input)
        {
            try
            {
                var user = await base.GetAsync(input);
                var role = _roleRepository.GetAll().Where(x => x.Name == user.RoleNames[0]).FirstOrDefault();
                var organization = _organisationRepository.GetAll().Include(x => x.Editions).Where(x => x.Id == user.OrganizationId).FirstOrDefault();
                user.Role = role;
                if (organization != null)
                    user.Organization = organization;
                return user;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return null;
            }

        }

        public async Task<UserDto> UpdateWithFileAsync(UserDto input)
        {
            //Updating the profile stored in Azure ADB2C
            await _graphClientService.UpdateUser(input);

            CheckUpdatePermission();
            //retrieve the user profile from the Local DB (SQL Server)
            var user = await _userManager.GetUserByIdAsync(input.Id);
            var prevPassword = user.Password;

            //BUG: This is not working as expected. All the user profile properties are overridden by the values from the "input" parameter, risk of delete valid values.
            //THe fix may involve introducing a UpdateUserDto that holds the minimum set of properties to update from the web UI.

            MapToEntity(input, user);

            await _userManager.UpdateNormalizedUserNameAsync(user);
            await _userManager.SetUserNameAsync(user, input.EmailAddress);

            if (prevPassword != input.Password)
                user.Password = _passwordHasher.HashPassword(user, input.Password);
            else
                user.Password = prevPassword;
            CheckErrors(await _userManager.UpdateAsync(user));

            if (input.RoleNames != null)
            {
                CheckErrors(await _userManager.SetRolesAsync(user, input.RoleNames));
            }

            // update ADB2C User as well

            return await GetAsync(input);
        }

        public async Task<UserDto> UpdatUserFirstLoginExperience(UserDto input)
        {
            input.IsFirstLoginExperience = false;
            var user = await _userManager.GetUserByIdAsync(input.Id);
            MapToEntity(input, user);
            user.IsFirstLoginExperience = input.IsFirstLoginExperience;
            CheckErrors(await _userManager.UpdateAsync(user));
            return await GetAsync(input);
        }

        public async Task<Boolean> UploadProfilePicture(IFormFile file, [FromForm] int UserId)
        {
            if (UserId > 0)
            {
                var user = await _userRepository.GetAsync(UserId);
                var aduser = await _graphClientService.GetUserBySignInName(user.EmailAddress);

                if (user == null)
                    return false;
                var companyLogo = new Infrastructure.Models.UploadFileModel()
                {
                    BlobContainerName = SettingManager.GetSettingValue("OrganizationBlobContainerName"),
                    File = file,
                    FileNameWithExtension = file.FileName,
                    Path = Convert.ToString(MultiTenancyConsts.DefaultTenantId) + "/organizations/" + user.OrganizationId + "/users/" + Convert.ToString(UserId)
                };
                var url = await _fileUploadService.UploadFileAsync(companyLogo);
                if (!string.IsNullOrEmpty(url))
                {
                    user.PicturePath = url;
                    user.PictureName = companyLogo.FileNameWithExtension;
                    await _userRepository.UpdateAsync(user);
                }
                return true;
            }
            return false;
        }

        public override async Task DeleteAsync(EntityDto<long> input)
        {
            var user = await _userManager.GetUserByIdAsync(input.Id);
            await _graphClientService.DeleteUserById(user.EmailAddress);
            await _userManager.DeleteAsync(user);


        }

        // [AbpAuthorize(PermissionNames.Pages_Users_Activation)]
        public async Task Activate(EntityDto<long> user)
        {
            await Repository.UpdateAsync(user.Id, async (entity) =>
            {
                entity.IsActive = true;
            });
        }

        // [AbpAuthorize(PermissionNames.Pages_Users_Activation)]
        public async Task DeActivate(EntityDto<long> user)
        {
            await Repository.UpdateAsync(user.Id, async (entity) =>
            {
                entity.IsActive = false;
            });
        }

        public async Task<ListResultDto<RoleDto>> GetRoles()
        {
            var roles = await _roleRepository.GetAllListAsync();
            return new ListResultDto<RoleDto>(ObjectMapper.Map<List<RoleDto>>(roles));
        }

        public async Task ChangeLanguage(ChangeUserLanguageDto input)
        {
            await SettingManager.ChangeSettingForUserAsync(
                AbpSession.ToUserIdentifier(),
                LocalizationSettingNames.DefaultLanguage,
                input.LanguageName
            );
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="workEmail"></param>
        /// <returns></returns>
        public async Task<HubSpotCompanySearchByDomainResponseModel> GetHubSpotCompanyByDomainAsync(string workEmail)
        {
            try
            {
                return await _hubSpotService.GetHubSpotCompanyByDomainAsync(workEmail);
            }
            catch (Exception ex)
            {
                Logger.Error("HubSpot company retrieval failed", ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hubSpotCompanyModel"></param>
        /// <returns></returns>
        public async Task<long> CreateHubSpotCompany(HubSpotCompanyRequestModel hubSpotCompanyModel)
        {
            try
            {
                return await _hubSpotService.CreateHubSpotCompany(hubSpotCompanyModel);
            }
            catch (Exception ex)
            {
                Logger.Error("Unable to create HubSpot Company", ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hubSpotCompanyModel"></param>
        /// <returns></returns>
        public async Task<long> UpdateHubSpotCompany(HubSpotCompanyRequestModel hubSpotCompanyModel)
        {
            try
            {
                return await _hubSpotService.UpdateHubSpotCompany(hubSpotCompanyModel);
            }
            catch (Exception ex)
            {
                Logger.Error("Unable to create HubSpot Company", ex);
                throw;
            }
        }

        public async Task<dynamic> ListUsers()
        {
            return await _graphClientService.ListUsers();
        }

        protected override User MapToEntity(CreateUserDto createInput)
        {
            var user = ObjectMapper.Map<User>(createInput);
            user.SetNormalizedNames();
            return user;
        }

        protected override void MapToEntity(UserDto input, User user)
        {
            ObjectMapper.Map(input, user);
            user.SetNormalizedNames();
        }

        protected override UserDto MapToEntityDto(User user)
        {
            if (user.Roles != null && user.Roles.Count > 0)
            {
                var roleIds = user.Roles.Select(x => x.RoleId).ToArray();

                var roles = _roleManager.Roles.Where(r => roleIds.Contains(r.Id)).Select(r => r.NormalizedName);

                var userDto = base.MapToEntityDto(user);
                userDto.RoleNames = roles.ToArray();

                return userDto;
            }
            else
            {
                var userDto = base.MapToEntityDto(user);
                return userDto;
            }
        }

        protected override IQueryable<User> CreateFilteredQuery(PagedUserResultRequestDto input)
        {
            return Repository.GetAllIncluding(x => x.Roles)
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.UserName.Contains(input.Keyword) || x.Name.Contains(input.Keyword) || x.EmailAddress.Contains(input.Keyword))
                .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive);
        }

        protected override async Task<User> GetEntityByIdAsync(long id)
        {
            var user = await Repository.GetAllIncluding(x => x.Roles).FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                throw new EntityNotFoundException(typeof(User), id);
            }

            return user;
        }

        protected override IQueryable<User> ApplySorting(IQueryable<User> query, PagedUserResultRequestDto input)
        {
            return query.OrderBy(r => r.UserName);
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        public async Task<bool> ChangePassword(ChangePasswordDto input)
        {
            await _userManager.InitializeOptionsAsync(AbpSession.TenantId);

            var user = await _userManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                throw new Exception("There is no current user!");
            }

            if (await _userManager.CheckPasswordAsync(user, input.CurrentPassword))
            {
                CheckErrors(await _userManager.ChangePasswordAsync(user, input.NewPassword));
            }
            else
            {
                CheckErrors(IdentityResult.Failed(new IdentityError
                {
                    Description = "Incorrect password."
                }));
            }

            await _graphClientService.SetPasswordByUserId(user.EmailAddress, input.NewPassword);
            return true;
        }
        [AbpAllowAnonymous]
        public async Task<bool> ResetPassword(ResetPasswordDto input)
        {
            var user = _userManager.Users.Where(x => x.EmailAddress == input.EmailAddress).FirstOrDefault();
            if (user == null)
            {
                throw new UserFriendlyException("this email is not registered in  our system");
            }
            var organization = _organisationRepository.FirstOrDefault(x => x.Id == user.OrganizationId);
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var userActivationUrl = $"{_config.GetValue<string>("App:ClientUrl")}/{SettingManager.GetSettingValue("SetPasswordPageUrl")}?email={user.EmailAddress}&organizationId={((user.OrganizationId == null || user.OrganizationId == Guid.Empty) ? Guid.Empty : user.OrganizationId)}&token={HttpUtility.UrlEncode(token)}";
            var emailModel = new ActivateUserEmailDto()
            {
                LogoUrl = organization != null ? organization.PicturePath ?? "" : "",
                ActivationLink = userActivationUrl,
                FromEmail = _config.GetValue<string>("App:FromEmail"),
                ToEmail = user.EmailAddress,
                ToName = $"{user.Name} {user.Surname}",
                Subject = "Reset Password ClimateCamp!",
                TemplateName = "ResetPasswordEmail"
            };
            var response = await _emailClientSender.SendEmail(emailModel, _config.GetValue<string>("App:Functions:EmailSenderFunctionUrl"));
            return true;
        }

        public async Task<List<DDLDto<int>>> GetRolesDropDown()
        {
            var roles = await _roleRepository.GetAll()
                .Where(x => x.Name != "ClimateCampAdmin")
                .Select(t => new DDLDto<int>()
                {
                    Id = t.Id,
                    Name = t.Name,
                    DisplayName = t.DisplayName
                }).ToListAsync();
            return roles;
        }

        [AbpAllowAnonymous]
        public async Task<bool> SetPassword(SetPasswordDto input)
        {

            var user = _userManager.Users.Where(x => x.EmailAddress == input.Email).FirstOrDefault();
            if (user != null)
            {
                user.Password = _passwordHasher.HashPassword(user, input.Password);
                user.IsActive = true;
                await CurrentUnitOfWork.SaveChangesAsync();
            }
            await _graphClientService.SetPasswordByUserId(user.EmailAddress, user.Password);
            return true;
        }

        public async Task<string> Ping()
        {
            return "Ping Pong!";
        }

        /// <summary>
        /// Returns the list of industries as well as sub-industries.
        /// </summary>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task<List<IndustriesGroup>> GetGroupedIndustries()
        {
            try
            {
                List<IndustriesGroup> industriesGroupList = new List<IndustriesGroup>();

                List<Industry> mainIndustriesList = _industriesRepository.GetAll().Where(x => x.ParentIndustryId == null)
                                                    .OrderByDescending(x => x.IsPriority)
                                                    .ThenBy(x => x.Name)
                                                    .ToList();

                List<Industry> industriesList = _industriesRepository.GetAll().Where(x => x.ParentIndustryId != null).ToList();

                foreach (var mainIndustry in mainIndustriesList)
                {
                    IndustriesGroup model = new IndustriesGroup
                    {
                        label = mainIndustry.Name,
                        data = mainIndustry.Id,
                        Children = industriesList.Any(x => x.ParentIndustryId == mainIndustry.Id) ?
                                     industriesList.Where(x => x.ParentIndustryId == mainIndustry.Id)
                                     .Select(x => new IndustryChild { data = x.Id, label = x.Name, IsPriority = x.IsPriority })
                                     .OrderByDescending(x => x.IsPriority)
                                     .ThenBy(x => x.label)
                                     .ToList() : new List<IndustryChild>()
                    };
                    industriesGroupList.Add(model);

                }

                return industriesGroupList;

            }
            catch (Exception ex)
            {
                _logger.Error($"Method: GetGroupedIndustries - Exception: {ex}");
                return null;
            }
        }

        private string CreateAccessToken(IEnumerable<Claim> claims, TimeSpan? expiration = null)
        {
            var now  = DateTime.UtcNow;
            var securityValue = _config.GetValue<string>("Authentication:JwtBearer:SecurityKey");
          var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(securityValue));
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _config.GetValue<string>("Authentication:JwtBearer:Issuer"),
                audience: _config.GetValue<string>("Authentication:JwtBearer:Audience"),
                claims: claims,
                notBefore: now,
                expires: now.AddDays(1),
                signingCredentials:  new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
            ); ;

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        private static List<Claim> CreateJwtClaims(ClaimsIdentity identity)
        {
            var claims = identity.Claims.ToList();
            var nameIdClaim = claims.First(c => c.Type == ClaimTypes.NameIdentifier);

            // Specifically add the jti (random nonce), iat (issued timestamp), and sub (subject/user) claims.
            claims.AddRange(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, nameIdClaim.Value),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            });

            return claims;
        }

        private async Task<AbpLoginResult<Tenant, User>> GetLoginResultAsync(string usernameOrEmailAddress, string password, string tenancyName)
        {
            var loginResult = await _logInManager.LoginAsync(usernameOrEmailAddress, password, tenancyName);

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    return loginResult;
                default:
                    throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(loginResult.Result, usernameOrEmailAddress, tenancyName);
            }
        }

        /*

        /// <summary>
        /// Notify organizational admin of new user.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userRole"></param>
        /// <param name="organizationName"></param>
        /// <param name="targetUserIdentifier"></param>
        /// <returns></returns>
         public async Task PublishNewOrganizationUser(string userName, string userRole, string organizationName, UserIdentifier targetUserIdentifier)
         {
             Logger.Warn("NewUser publish notification.");
                 await _notificationPublisher.PublishAsync(
                 NotificationTypes.NewOrganizationUser,
                 new MessageNotificationData(userName + " has joined as a " + userRole + " for " + organizationName +"."),
                 userIds: new[] { targetUserIdentifier }
                 );
         }
         */
    }

    public class TokenAuthConfiguration
    {
        public SymmetricSecurityKey SecurityKey { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public SigningCredentials SigningCredentials { get; set; }

        public TimeSpan Expiration { get; set; }
    }
}


