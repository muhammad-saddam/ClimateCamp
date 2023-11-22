using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.MultiTenancy;
using ClimateCamp.CarbonCompute;
using ClimateCamp.Common.Authorization.Users;
using ClimateCamp.Core;
using ClimateCamp.Core.Editions;
using ClimateCamp.EmailClient.Models;
using ClimateCamp.EmailClient.Services;
using ClimateCamp.Infrastructure.HubSpot;
using ClimateCamp.Infrastructure.Models;
using ClimateCamp.Supplier.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ClimateCamp.Core.OrganizationUnit;

namespace ClimateCamp.Application
{
    /// <summary>
    /// 
    /// </summary>
    [AbpAuthorize]
    public class SupplierAppService : AsyncCrudAppService<CustomerSupplier, CustomerSuppliersDto, long, PagedCustomerSuppliersResultRequestDto, CreateSupplierInput, CreateSupplierInput>, ISupplierAppService//, CommonAppServiceBase
    {
        private readonly IRepository<Organization, Guid> _organisationRepository;
        private readonly IRepository<EmissionsFactorsLibrary, Guid> _emissionFactorRepository;
        private readonly IRepository<CustomerSupplier, long> _supplierRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IEmailClientSender _emailClientSender;
        private readonly IHubspotService _hubSpotService;
        private readonly IRepository<CustomEdition, int> _editionRepository;
        private readonly IRepository<OrganizationUnit, Guid> _organizationUnitRepository;
        private readonly IEmissionGroupsAppService _emissionGroupsAppService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="supplierRepository"></param>
        /// <param name="organisationRepository"></param>
        /// <param name="emissionFactorRepository"></param>
        /// <param name="userRepository"></param>
        /// <param name="emailClientSender"></param>
        /// <param name="configuration"></param>
        /// <param name="hubSpotService"></param>
        /// <param name="editionRepository"></param>
        /// <param name="organizationUnitRepository"></param>
        /// <param name="emissionGroupsAppService"></param>
        public SupplierAppService(
            IRepository<CustomerSupplier, long> supplierRepository,
            IRepository<Organization, Guid> organisationRepository,
            IRepository<EmissionsFactorsLibrary, Guid> emissionFactorRepository,
            IRepository<User, long> userRepository,
            IEmailClientSender emailClientSender,
            IConfiguration configuration,
            IHubspotService hubSpotService,
            IRepository<CustomEdition, int> editionRepository,
            IRepository<OrganizationUnit, Guid> organizationUnitRepository,
            IEmissionGroupsAppService emissionGroupsAppService) : base(supplierRepository)
        {
            _organisationRepository = organisationRepository;
            _supplierRepository = supplierRepository;
            _emailClientSender = emailClientSender;
            _configuration = configuration;
            _userRepository = userRepository;
            _hubSpotService = hubSpotService;
            _emissionFactorRepository = emissionFactorRepository;
            _editionRepository = editionRepository;
            _organizationUnitRepository = organizationUnitRepository;
            _emissionGroupsAppService = emissionGroupsAppService;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async override Task<CustomerSuppliersDto> CreateAsync(CreateSupplierInput input)
        {
            try
            {
                //call hubspot api to ftech org detials if not found create on hubspot and create on database if with with same customer organizationId and supplierorgnization id not exist as well 
                // send an email to ruben and laurent
                // set SupplierOrganizationId to newly created org

                var organization = new Organization();

                var hubSpotCompanyData = await _hubSpotService.GetHubSpotCompanyByDomainAsync(input.ContactEmailAddress);

                var edition = await _editionRepository.FirstOrDefaultAsync(x => x.Name == "Business");

                if (hubSpotCompanyData.total == 0)
                {
                    var hubSpotCompanyId = await _hubSpotService.CreateHubSpotCompany(new HubSpotCompanyRequestModel
                    {
                        name = input.Name,
                        production_volume = input.YearlyProductQuantity,
                        annualrevenue = Convert.ToDecimal(input.YearlyServiceQuantity),
                        numberofemployees = 0
                    });

                    organization.Name = input.Name;
                    organization.Revenue = Convert.ToDecimal(input.YearlyServiceQuantity);
                    organization.TotalEmployees = 0;
                    organization.ProductionQuantity = input.YearlyProductQuantity;
                    organization.TenantId = MultiTenancyConsts.DefaultTenantId;
                    organization.HubSpotId = hubSpotCompanyId;
                    organization.IsActive = true;
                    organization.IsDeleted = false;
                    organization.EmissionsFactorsLibraryId = ClimateCampConsts.DefaultEmissionsFactorsLibraryId;
                    organization.EditionId = edition.Id;
                    organization.CreatorUserId = input.CreatorUserId;
                }
                else
                {
                    organization.Name = hubSpotCompanyData.results[0].properties.name;
                    organization.Revenue = hubSpotCompanyData.results[0].properties.annualrevenue;
                    organization.TotalEmployees = int.Parse(hubSpotCompanyData.results[0].properties.numberofemployees ?? "0");
                    organization.ProductionQuantity = long.Parse(hubSpotCompanyData.results[0].properties.production_volume ?? "0");
                    organization.TenantId = MultiTenancyConsts.DefaultTenantId;
                    organization.HubSpotId = long.Parse(hubSpotCompanyData.results[0].properties.hs_object_id ?? "0");
                    organization.IsActive = true;
                    organization.IsDeleted = false;
                    organization.EmissionsFactorsLibraryId = ClimateCampConsts.DefaultEmissionsFactorsLibraryId;
                    organization.EditionId = edition.Id;
                    organization.CreatorUserId = input.CreatorUserId;
                }
                var result = new Organization();
                var isExist = await _organisationRepository.FirstOrDefaultAsync(x => x.Name.ToLower() == organization.Name.ToLower());
                if (isExist == null)
                {
                    result = await _organisationRepository.InsertAsync(organization);

                    #region Create Organization Unit if Organization doesn't exist
                    var orgUnitCreated = await _organizationUnitRepository.GetAll().Where(x => x.OrganizationId == result.Id).AnyAsync();

                    if (!orgUnitCreated)
                    {
                        var organizationUnit = await _organizationUnitRepository.InsertAsync(new OrganizationUnit
                        {
                            Name = result.Name,
                            Type = OrganizationUnitType.Unit,
                            Address = result.Address,
                            PhoneNumber = result.PhoneNumber,
                            VATNumber = result.VATNumber,
                            MonthlyRevenue = Convert.ToDouble(result.Revenue),
                            CountryId = result.CountryId,
                            EffectiveStartDate = DateTime.UtcNow,
                            IsActive = true,
                            IsDeleted = false,
                            OrganizationId = result.Id,
                            PictureName = result.PictureName,
                            PicturePath = result.PicturePath,
                        });

                    }
                    #endregion
                }
                else
                {
                    result = _organisationRepository.FirstOrDefault(x => x.Name.ToLower() == organization.Name.ToLower());
                }

                await CurrentUnitOfWork.SaveChangesAsync();

                input.SupplierOrganizationId = result.Id;

                var supplierResponse = await base.CreateAsync(input);
                supplierResponse.Name = organization.Name;

                #region User Story 1499 Create Default Emission Groups On New Supplier Creation

                await _emissionGroupsAppService.InsertEmissionGroupsFromTemplate("Breweries", supplierResponse.SupplierOrganizationId);

                #endregion

                await CurrentUnitOfWork.SaveChangesAsync();

                var currentUser = _userRepository.Get(AbpSession.UserId ?? 0);

                var currentUserOrganization = _organisationRepository.Get(input.CustomerOrganizationId);
                var emails = "laurent@climatecamp.io, ruben@climatecamp.io"; // replace that with actual contact person email when fully implemnted!
                var emailList = emails.Split(",").ToList();

                foreach (var email in emailList)
                {
                    var emailModel = new InvitedSupplierNotificationEmail
                    {
                        LogoUrl = result?.PicturePath ?? "",
                        FromEmail = _configuration.GetValue<string>("App:FromEmail"),
                        ToEmail = email,
                        ToName = !string.IsNullOrEmpty(input.ContactPersonFirstName) ? input.ContactPersonFirstName : input.ContactEmailAddress,
                        Subject = "New Supplier Invitation!",
                        TemplateName = string.Empty,
                        Body = "<p style='font-weight:600'><strong>User Name:-  </strong> " + currentUser.UserName + "</p>" +
                               "<p style='font-weight:600'><strong>User Company Name:-  </strong> " + currentUserOrganization.Name + "</p>" +
                               "<p style='font-weight:600'><strong>Supplier Company Name:-  </strong> " + organization.Name + "</p>" +
                               "<p style='font-weight:600'><strong>Supplier Email:-  </strong> " + input.ContactEmailAddress + "</p>" +
                               "<p style='font-weight:600'><strong>Product:-  </strong> " + input.Product + "</p>" +
                               "<p style='font-weight:600'><strong>Service Name:-  </strong> " + input.Service + "</p>" +
                               "<p style='font-weight:600'><strong>Product Quantity:-  <strong>" + input.YearlyProductQuantity + "</p>" +
                               "<p style='font-weight:600'><strong>Service Quantity:-  </strong>" + input.YearlyServiceQuantity + "</p>"
                    };
                    await _emailClientSender.SendEmail(emailModel, _configuration.GetValue<string>("App:Functions:EmailSenderFunctionUrl"));
                }
                return supplierResponse;
            }

            catch (Exception ex)
            {
                Logger.Error("Unable to create Supplier organization: Method - CreateAsync", ex);
                throw;
            }

        }
        /// <summary>
        /// fetch All Suppliers organization with filtered with customers organizationId
        /// </summary>
        /// <param name="input">If the customerOrganizationId is Guid.Empty it will retrieve all suppliers</param>
        /// <returns></returns>
        public async Task<PagedResultDto<CustomerSuppliersDto>> GetSuppliersByorganizationId(SuppliersRequestDto input)
        {
            List<CustomerSupplier> suppliers = new();
            try
            {

                if (input.customerOrganizationId != Guid.Empty)
                    suppliers = await _supplierRepository.GetAll().Include(x => x.SupplierOrganization).Include(x => x.Country).Where(x => x.CustomerOrganizationId == input.customerOrganizationId).ToListAsync();
                else
                    suppliers = await _supplierRepository.GetAll().Include(x => x.SupplierOrganization).Include(x => x.Country).ToListAsync();
                List<CustomerSuppliersDto> suppliersList = new List<CustomerSuppliersDto>();
                foreach (var supplier in suppliers)
                {
                    CustomerSuppliersDto model = new()
                    {
                        Name = supplier.SupplierOrganization?.Name,
                        ContactPersonFirstName = supplier.ContactFirstName,
                        ContactPersonLastName = supplier.ContactLastName,
                        ContactEmailAddress = supplier.ContactEmailAddress,
                        Product = supplier.Product,
                        Id = supplier.Id,
                        YearlyProductQuantity = supplier.YearlyProductQuantity,
                        YearlyServiceQuantity = supplier.YearlyServiceQuantity,
                        Tag = supplier.Tag,
                        Status = Enum.GetName(typeof(SupplierActivationStatus), supplier.Status),
                        CustomerOrganizationId = supplier.CustomerOrganizationId,
                        SupplierOrganizationId = supplier.SupplierOrganizationId,
                        CountryId = supplier?.CountryId
                    };
                    suppliersList.Add(model);
                }
                return new PagedResultDto<CustomerSuppliersDto>()
                {
                    Items = ObjectMapper.Map<List<CustomerSuppliersDto>>(suppliersList),
                    TotalCount = suppliers.Count
                };
            }
            catch (Exception ex)
            {
                Logger.Error("Unable to get Supplier organizations: Method - GetSuppliersByorganizationId", ex);
                return null;
            }

        }

        /// <summary>
        /// fetch All Suppliers organization with filtered with customers organizationId and also with status validated
        /// </summary>
        /// <param name="customrOrganizationId">If Guid.Empty it will retrieve all suppliers</param>
        /// <returns></returns>
        public async Task<PagedResultDto<CustomerSuppliersDto>> GetSuppliersByCustomerOrganizationIdAndStatusValidated(Guid customrOrganizationId)
        {
            List<CustomerSupplier> suppliers = new();
            try
            {

                if (customrOrganizationId != Guid.Empty)
                    suppliers = await _supplierRepository.GetAll()
                        .Include(x => x.SupplierOrganization)
                        .Include(x => x.Country)
                        .Where(x => (x.CustomerOrganizationId == customrOrganizationId || x.Status == SupplierActivationStatus.Validated))
                        .ToListAsync();
                else
                    suppliers = await _supplierRepository.GetAll()
                        .Include(x => x.SupplierOrganization)
                        .Include(x => x.Country)
                        .ToListAsync();

                List<CustomerSuppliersDto> suppliersList = new();
                foreach (var supplier in suppliers)
                {
                    CustomerSuppliersDto model = new()
                    {
                        Name = supplier.SupplierOrganization?.Name,
                        ContactPersonFirstName = supplier.ContactFirstName,
                        ContactEmailAddress = supplier.ContactEmailAddress,
                        Product = supplier.Product,
                        Id = supplier.Id,
                        YearlyProductQuantity = supplier.YearlyProductQuantity,
                        YearlyServiceQuantity = supplier.YearlyServiceQuantity,
                        Tag = supplier.Tag,
                        Status = Enum.GetName(typeof(SupplierActivationStatus), supplier.Status),
                        CustomerOrganizationId = supplier.CustomerOrganizationId,
                        SupplierOrganizationId = supplier.SupplierOrganizationId,
                        CountryId = supplier?.CountryId
                    };
                    suppliersList.Add(model);
                }
                return new PagedResultDto<CustomerSuppliersDto>()
                {
                    Items = ObjectMapper.Map<List<CustomerSuppliersDto>>(suppliersList),
                    TotalCount = suppliers.Count
                };
            }
            catch (Exception ex)
            {
                Logger.Error("Unable to get Supplier organizations: Method - GetSuppliersByCustomerOrganizationId", ex);
                return null;
            }

        }

        //public async Task<PagedResultDto<GetSupplierOutput>> GetMyOrganizationSuppliersAsync()
        //{
        //    throw new NotImplementedException();
        //    //AbpSession.UserId
        //    //var userDetails = await _userRepository.GetAsync((long)_abpSession.UserId);
        //    //List<Organization> organizations = null;
        //    //if (userDetails.OrganizationId != Guid.Empty)
        //    //{
        //    //    organizations = await _organisationRepository.GetAll().Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == userDetails.OrganizationId).ToListAsync();
        //    //    return new PagedResultDto<OrganizationDto>()
        //    //    {
        //    //        Items = ObjectMapper.Map<List<OrganizationDto>>(organizations),
        //    //        TotalCount = organizations.Count
        //    //    };
        //    //}

        //    //var s = await Repository
        //    //    .GetAll()
        //    //    .Include(cs => cs.SupplierOrganization)
        //    //    .Where(registration => registration.UEventId == @event.Id)
        //    //    .Select(registration => registration.User)
        //    //    .ToListAsync();



        //    //return new PagedResultDto<GetSupplierOutput>()
        //    //{
        //    //    Items = ObjectMapper.Map<List<GetSupplierOutput>>(organizations),
        //    //    TotalCount = organizations.Count
        //    //};
        //}

        //public async Task InviteSupplierAsync(InviteSupplierInput input)
        //{


        //    try
        //    {
        //        var emailModel = new InvitedSupplierNotificationEmail()
        //        {
        //            LogoUrl = "",
        //            FromEmail = _configuration.GetValue<string>("App:FromEmail"),
        //            ToEmail = "laurent@climatecamp.io",//user.EmailAddress,
        //            //ToName = $"{user.FirstName} {user.LastName}",
        //            Subject = $"New supplier '{input.Name}' invited to ClimateCamp! - Contact: {input.ContactEmailAddress}",
        //            TemplateName = "ActivateUserEmail"

        //        };
        //        await _emailClientSender.SendEmail(emailModel, _configuration.GetValue<string>("App:Functions:EmailSenderFunctionUrl"));
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error("Couldn't invite the supplier", ex);
        //    };
        //}
    }
}
