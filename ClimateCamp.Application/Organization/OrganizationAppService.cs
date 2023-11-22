using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.MultiTenancy;
using Abp.Runtime.Session;
using ClimateCamp.CarbonCompute;
using ClimateCamp.Common.Authorization.Users;
using ClimateCamp.Common.Dto;
using ClimateCamp.Core;
using ClimateCamp.Core.Authorization;
using ClimateCamp.Core.CarbonCompute.Enum;
using ClimateCamp.Core.Editions;
using ClimateCamp.EmailClient.Models;
using ClimateCamp.EmailClient.Services;
using ClimateCamp.Infrastructure.FileUploadService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClimateCamp.Application
{
    [AbpAuthorize(PermissionNames.Pages_Organization)]
    public class OrganizationAppService : AsyncCrudAppService<Organization, OrganizationDto, Guid, PagedOrganizationResultRequestDto, CreateOrganizationDto, CreateOrganizationDto>, IOrganizationAppService
    {
        private readonly IRepository<Organization, Guid> _organisationRepository;
        private readonly IFileUploadService _fileUploadService;
        private readonly IAbpSession _abpSession;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<CustomEdition, int> _editionRepository;
        private readonly IConfiguration _config;
        private readonly ILogger<OrganizationAppService> _logger;
        private readonly IRepository<OrganizationTarget, Guid> _organisationTargetRepository;
        private readonly IRepository<TargetIndependant, Guid> _targetIndependantRepository;
        private readonly IRepository<ScienceBasedTarget, Guid> _scienceBasedTargetRepository;
        private readonly IRepository<Core.EmissionsSummary, Guid> _emissionsSummaryRepository;
        private readonly IRepository<Emission, Guid> _emissionRepository;
        private readonly IRepository<ActivityData, Guid> _activityDataRepository;
        private readonly IRepository<ActivityType, int> _activityTypeRepository;
        private readonly IEmailClientSender _emailClientSender;
        private readonly IRepository<EmissionsSource, int> _emissionSourceRepository;
        private readonly IEmissionGroupsAppService _emissionGroupsAppService;
        private readonly IRepository<OrganizationUnit, Guid> _organizationUnitRepository;

        public OrganizationAppService(
            IRepository<Organization, Guid> organisationRepository,
            IFileUploadService fileUploadService, IAbpSession abpSession,
            IRepository<User, long> userRepository,
            IRepository<CustomEdition, int> editionRepository,
            IRepository<OrganizationTarget, Guid> organisationTargetRepository,
            IRepository<TargetIndependant, Guid> targetIndependantRepository,
            IRepository<ScienceBasedTarget, Guid> scienceBasedTargetRepository,
            ILogger<OrganizationAppService> logger,
            IRepository<Core.EmissionsSummary, Guid> emissionsSummaryRepository,
            IRepository<Emission, Guid> emissionRepository,
            IRepository<ActivityData, Guid> activityDataRepository,
            IRepository<ActivityType, int> activityTypeRepository,
            IEmailClientSender emailClientSender,
            IRepository<EmissionsSource, int> emissionSourceRepository,
            IEmissionGroupsAppService emissionGroupsAppService,
            IConfiguration config,
            IRepository<OrganizationUnit, Guid> organizationUnitRepository) : base(organisationRepository)
        {
            _organisationRepository = organisationRepository ?? throw new ArgumentNullException(nameof(organisationRepository));
            _fileUploadService = fileUploadService;
            _abpSession = abpSession;
            _userRepository = userRepository;
            _editionRepository = editionRepository;
            _organisationTargetRepository = organisationTargetRepository;
            _targetIndependantRepository = targetIndependantRepository;
            _scienceBasedTargetRepository = scienceBasedTargetRepository;
            _logger = logger;
            _emissionRepository = emissionRepository;
            _emissionsSummaryRepository = emissionsSummaryRepository;
            _activityDataRepository = activityDataRepository;
            _activityTypeRepository = activityTypeRepository;
            _emailClientSender = emailClientSender;
            _emissionSourceRepository = emissionSourceRepository;
            _config = config;
            _emissionGroupsAppService = emissionGroupsAppService;
            _organizationUnitRepository = organizationUnitRepository;
        }

        public override async Task<PagedResultDto<OrganizationDto>> GetAllAsync(PagedOrganizationResultRequestDto input)
        {
            var userDetails = await _userRepository.GetAsync((long)_abpSession.UserId);
            List<Organization> organizations = null;

            var organizationsQuery = _organisationRepository.GetAll().Where(x => x.IsActive && !x.IsDeleted);

            if (userDetails.OrganizationId != Guid.Empty)
            {
                organizationsQuery = organizationsQuery.Where(x => x.Id == userDetails.OrganizationId);
            }

            organizations = await organizationsQuery
                .OrderBy(x => x.Name)
                .ToListAsync();

            return new PagedResultDto<OrganizationDto>()
            {
                Items = ObjectMapper.Map<List<OrganizationDto>>(organizations),
                TotalCount = organizations.Count
            };
        }

        public async Task<OrganizationDto> CreateAsyncWithFile(CreateOrganizationDto input)
        {
            input.TenantId = MultiTenancyConsts.DefaultTenantId;

            var organization = await base.CreateAsync(input);

            #region User Story 1499 Create Default Emission Groups On New Organization Creation

            await _emissionGroupsAppService.InsertEmissionGroupsFromTemplate("Breweries", organization.Id);

            #endregion

            return organization;

        }

        public override Task<OrganizationDto> UpdateAsync(CreateOrganizationDto input)
        {
            return base.UpdateAsync(input);
        }

        public async Task<Boolean> UploadOrganizationLogo(IFormFile file, [FromForm] Guid OrganizationId)
        {
            if (OrganizationId != Guid.Empty)
            {
                var organization = await _organisationRepository.GetAsync(OrganizationId);

                if (organization == null)
                    return false;
                var companyLogo = new Infrastructure.Models.UploadFileModel()
                {
                    BlobContainerName = SettingManager.GetSettingValue("OrganizationBlobContainerName"),
                    File = file,
                    FileNameWithExtension = file.FileName,
                    Path = Convert.ToString(MultiTenancyConsts.DefaultTenantId) + "/organizations/" + Convert.ToString(OrganizationId)
                };
                //TODO: Here it breaks when trying to change the picture of an Organization using a picture that was already used before.
                var url = await _fileUploadService.UploadFileAsync(companyLogo);
                if (!string.IsNullOrEmpty(url))
                {
                    organization.PicturePath = url;
                    organization.PictureName = companyLogo.FileNameWithExtension;
                    await _organisationRepository.UpdateAsync(organization);
                }
                return true;
            }
            return false;
        }


        public async Task<Guid> CreateOrganization(CreateOrganizationDto input)
        {
            input.TenantId = MultiTenancyConsts.DefaultTenantId;

            var organization = ObjectMapper.Map<Organization>(input);
            if (input.Id != Guid.Empty)
            {
                await _organisationRepository.UpdateAsync(organization);
                CurrentUnitOfWork.SaveChanges();
            }
            else
            {
                var result = await _organisationRepository.InsertAsync(organization);
                CurrentUnitOfWork.SaveChanges();
                input.Id = result.Id;
            }
            return input.Id;
        }

        public override async Task<OrganizationDto> CreateAsync(CreateOrganizationDto input)
        {
            input.TenantId = MultiTenancyConsts.DefaultTenantId;

            //Assign Edition To SelfService Organization
            //TODO: Refactor using the domain service <see cref="EditionManager"/> instead of a repository, like in <see cref="ClimateCamp.Common.MultiTenancy.TenantAppService.CreateAsync"/> method.
            var edition = await _editionRepository.FirstOrDefaultAsync(x => x.Name == "Business");

            if (edition != null)
                input.EditionId = edition.Id;
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
            }
            return await base.CreateAsync(input);

        }

        public OrganizationDto GetOrganizationById(Guid organizationId)
        {
            var organizations = _organisationRepository.GetAll().Include(x => x.Editions).Where(x => x.Id == organizationId).FirstOrDefault();
            var organization = ObjectMapper.Map<OrganizationDto>(organizations);
            return organization;
        }
        public async Task<List<DDLDto<Guid>>> GetOrganizationsDropDown()
        {
            var organizations = await _organisationRepository.GetAll()
                .Select(t => new DDLDto<Guid>() { Id = t.Id, Name = t.Name, PicturePath = t.PicturePath })
                .OrderBy(x => x.Name)
                .ToListAsync();
            return organizations;
        }

        public async Task<bool> UpdateTargetValuesAsync(UpdateTargetValuesDto input)
        {
            if (input.Id == Guid.Empty)
                return false;
            var organization = await _organisationRepository.GetAsync(input.Id);
            if (organization != null)
            {
                organization.BaseLineYear = input.BaseLineYear;
                organization.BaseLineEmission = input.BaseLineEmission;
                organization.Target = input.Target;
                await _organisationRepository.UpdateAsync(organization);
                return true;
            }
            return false;
        }

        public async Task<PagedResultDto<OrganizationDto>> GetVerifiedOrganizations()
        {
            var verifiedOrganizations = await _organisationRepository.GetAll()
                .OrderBy(x => x.Name)
                .Where(x => x.Status == (int)OrganizationStatus.Verified)
                .ToListAsync();
            return new PagedResultDto<OrganizationDto>
            {
                Items = ObjectMapper.Map<List<OrganizationDto>>(verifiedOrganizations),
                TotalCount = verifiedOrganizations.Count
            };
        }

        /// <summary>
        /// Fetch all verified organizations and the unverified organizations that have been added by the organization currently logged in
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<OrganizationDto>> GetOrganizationSuppliers(Guid organizationId)
        {
            var organizationSuppliers = from o in _organisationRepository.GetAll()
                                        join u in _userRepository.GetAll() on o.CreatorUserId equals u.Id into ou
                                        from u in ou.DefaultIfEmpty()
                                        where u.OrganizationId == organizationId || o.Status == (int)OrganizationStatus.Verified
                                        orderby o.Name
                                        select new OrganizationDto
                                        {
                                            Id = o.Id,
                                            Name = o.Name,
                                            Address = o.Address,
                                            PhoneNumber = o.PhoneNumber,
                                            VATNumber = o.VATNumber,
                                            CountryId = o.CountryId,
                                            AnnualReportingPeriod = o.AnnualReportingPeriod,
                                            IsActive = o.IsActive,
                                            TenantId = o.TenantId,
                                            PictureName = o.PictureName,
                                            PicturePath = o.PicturePath,
                                            HubSpotId = o.HubSpotId,
                                            Status = (OrganizationStatus)o.Status
                                        };

            var result = await organizationSuppliers.ToListAsync();

            //Orphan Supplier Organization
            result.Add(new OrganizationDto
            {
                Id = Guid.Empty,
                Name = "N/A"
            });

            return new PagedResultDto<OrganizationDto>
            {
                Items = ObjectMapper.Map<List<OrganizationDto>>(result),
                TotalCount = result.Count
            };
        }

        public async Task<List<OrganizationTargetDto>> GetOrganizationTarget(Guid organizationId, int type)
        {
            try
            {
                var organizationTargets = await (from orgTarget in _organisationTargetRepository.GetAll()
                                                 where orgTarget.OrganizationId == organizationId && (type != 0 ? orgTarget.TSFType == type : true)
                                                 select orgTarget)
                                          .ToListAsync();

                return organizationTargets.Any() ? ObjectMapper.Map<List<OrganizationTargetDto>>(organizationTargets) : null;
            }
            catch (Exception exception)
            {
                _logger.LogInformation($"Method: GetOrganizationTarget - Exception: {exception}");
                return null;
            }
        }

        public async Task<OrganizationTargetBaseLineYearScopeEmissionsVM> GetOrgnizationUnitBaseLineYearScopeEmissions(Guid organizationUnitId, int baseLineYear)
        {
            try
            {
                var organizationUnitEmission = await (from emissionSummary in _emissionsSummaryRepository.GetAll()
                                                  where emissionSummary.OrganizationUnitId == organizationUnitId && emissionSummary.Period == baseLineYear
                                                      select emissionSummary)
                                             .FirstOrDefaultAsync();

                return organizationUnitEmission != null ? new OrganizationTargetBaseLineYearScopeEmissionsVM
                {
                    Scope1 = organizationUnitEmission.Scope1tCO2e,
                    Scope2 = organizationUnitEmission.Scope2tCO2e,
                    Scope3 = organizationUnitEmission.Scope3tCO2e,
                    ProductionMetricId = organizationUnitEmission.ProductionMetricId,
                } :
                new OrganizationTargetBaseLineYearScopeEmissionsVM
                {
                    Scope1 = 0,
                    Scope2 = 0,
                    Scope3 = 0,
                    ProductionMetricId = 0
                };
            }
            catch (Exception exception)
            {
                _logger.LogInformation($"Method: GetOrgnizationBaseLineYearScopeEmissions - Exception: {exception}");
                return null;
            }
        }

        public async Task<TargetIndependantDto> GetTargetIndependant(Guid organizationUnitId, int targetYear)
        {
            try
            {
                var targetIndependant = await (from trgIndependant in _targetIndependantRepository.GetAll()
                                               where trgIndependant.TargetYear == targetYear && trgIndependant.OrganizationUnitId == organizationUnitId
                                               select trgIndependant)
                                         .FirstOrDefaultAsync();

                return targetIndependant != null ? ObjectMapper.Map<TargetIndependantDto>(targetIndependant) : new TargetIndependantDto
                {
                    Id = Guid.Empty,
                    TargetYear = targetYear,
                    BaseLineYear = 0,
                    OrganizationUnitId = organizationUnitId,
                    Scope1Target = 0,
                    Scope2Target = 0,
                    Scope3Target = 0,
                    OrganizationTargetId = Guid.Empty
                };
            }
            catch (Exception exception)
            {
                _logger.LogInformation($"Method: GetTargetIndependant - Exception: {exception}");
                return null;
            }
        }

        public async Task<TargetIndependantDto> GetTargetIndependantByOrganizationTarget(Guid organizationTargetId)
        {
            try
            {
                var targetIndependant = await (from trgIndependant in _targetIndependantRepository.GetAll()
                                               where trgIndependant.OrganizationTargetId == organizationTargetId
                                               select trgIndependant)
                                         .FirstOrDefaultAsync();

                return targetIndependant != null ? ObjectMapper.Map<TargetIndependantDto>(targetIndependant) : null;
            }
            catch (Exception exception)
            {
                _logger.LogInformation($"Method: GetTargetIndependantByOrganizationTarget - Exception: {exception}");
                return null;
            }
        }

        public async Task<ScienceBasedTargetDto> GetScienceBasedTarget(Guid organizationTargetId, int baseLineYear)
        {
            try
            {
                var scienceBasedTarget = await (from scBasedTarget in _scienceBasedTargetRepository.GetAll()
                                                where scBasedTarget.OrganizationTargetId == organizationTargetId && scBasedTarget.BaseLineYear == baseLineYear
                                                select scBasedTarget)
                                         .FirstOrDefaultAsync();

                return scienceBasedTarget != null ? ObjectMapper.Map<ScienceBasedTargetDto>(scienceBasedTarget) : null;
            }
            catch (Exception exception)
            {
                _logger.LogInformation($"Method: GetScienceBasedTarget - Exception: {exception}");
                return null;
            }
        }

        public async Task<CreateOrganizationTargetDto> SaveOrganizationTarget(CreateOrganizationTargetDto createOrganizationTargetDto)
        {
            try
            {
                var organizationTarget = new OrganizationTarget();

                if (createOrganizationTargetDto.Id == Guid.Empty)
                    organizationTarget = await _organisationTargetRepository.InsertAsync(ObjectMapper.Map<OrganizationTarget>(createOrganizationTargetDto));
                else
                    organizationTarget = await _organisationTargetRepository.UpdateAsync(ObjectMapper.Map<OrganizationTarget>(createOrganizationTargetDto));

                await CurrentUnitOfWork.SaveChangesAsync();

                createOrganizationTargetDto.Id = organizationTarget.Id;

                return createOrganizationTargetDto;
            }
            catch (Exception exception)
            {
                _logger.LogInformation($"Method: SaveOrganizationTarget - Exception: {exception}");
                return null;
            }
        }

        public async Task<CreateTargetIndependantDto> SaveTargetIndependant(CreateTargetIndependantDto createTargetIndependantDto)
        {
            try
            {
                var targetIndependant = new TargetIndependant();

                if (createTargetIndependantDto.Id == Guid.Empty)
                    targetIndependant = await _targetIndependantRepository.InsertAsync(ObjectMapper.Map<TargetIndependant>(createTargetIndependantDto));
                else
                    targetIndependant = await _targetIndependantRepository.UpdateAsync(ObjectMapper.Map<TargetIndependant>(createTargetIndependantDto));

                await CurrentUnitOfWork.SaveChangesAsync();

                createTargetIndependantDto.Id = targetIndependant.Id;

                return createTargetIndependantDto;
            }
            catch (Exception exception)
            {
                _logger.LogInformation($"Method: SaveTargetIndependant - Exception: {exception}");
                return null;
            }
        }

        public async Task<CreateScienceBasedTargetDto> SaveScienceBasedTarget(CreateScienceBasedTargetDto createScienceBasedTargetDto)
        {
            try
            {
                var scienceBasedTarget = new ScienceBasedTarget();

                if (createScienceBasedTargetDto.Id == Guid.Empty)
                    scienceBasedTarget = await _scienceBasedTargetRepository.InsertAsync(ObjectMapper.Map<ScienceBasedTarget>(createScienceBasedTargetDto));
                else
                    scienceBasedTarget = await _scienceBasedTargetRepository.UpdateAsync(ObjectMapper.Map<ScienceBasedTarget>(createScienceBasedTargetDto));

                await CurrentUnitOfWork.SaveChangesAsync();

                createScienceBasedTargetDto.Id = scienceBasedTarget.Id;

                return createScienceBasedTargetDto;
            }
            catch (Exception exception)
            {
                _logger.LogInformation($"Method: SaveScienceBasedTarget - Exception: {exception}");
                return null;
            }
        }

        public async Task<List<StackChartVM>> GetStackChartData(int baseLineYear, int targetYear, Guid organizationUnitId)
        {
            try
            {

                var stackChartTargetYearData = (from emission in _emissionRepository.GetAll()
                                                join activityData in _activityDataRepository.GetAll()
                                                on emission.ActivityDataId equals activityData.Id
                                                join activityType in _activityTypeRepository.GetAll()
                                                on activityData.ActivityTypeId equals activityType.Id
                                                join emissionSource in _emissionSourceRepository.GetAll()
                                                on activityType.EmissionsSourceId equals emissionSource.Id
                                                where
                                                activityData.OrganizationUnitId == organizationUnitId &&
                                                activityData.ConsumptionStart.Year >= baseLineYear &&
                                                activityData.ConsumptionStart.Year <= targetYear
                                                select
                                                new
                                                {
                                                    activityData,
                                                    emission,
                                                    emissionSource
                                                })
                                      .AsEnumerable()
                                      .GroupBy(g => g.activityData.ConsumptionStart.Year)
                                      .ToList();

                var stackChartListData = new List<StackChartVM>();

                //adding target years data
                stackChartListData.AddRange(
                    stackChartTargetYearData
                    .OrderBy(o => o.Key)
                    .Select(group => new StackChartVM
                    {
                        Year = group.Key.ToString(),
                        Scope1CO2e = group.Where(x => (int)x.emissionSource.EmissionScope == (int)GHG.EmissionScope.Scope1).Sum(x => x.emission.CO2E ?? 0),
                        Scope2CO2e = group.Where(x => (int)x.emissionSource.EmissionScope == (int)GHG.EmissionScope.Scope2).Sum(x => x.emission.CO2E ?? 0),
                        Scope3CO2e = group.Where(x => (int)x.emissionSource.EmissionScope == (int)GHG.EmissionScope.Scope3).Sum(x => x.emission.CO2E ?? 0),
                    })
                    );


                return stackChartListData;

            }
            catch (Exception exception)
            {
                _logger.LogInformation($"Method: GetStackChartData - Exception: {exception}");

                return null;
            }
        }
        public async Task<bool> SendDataRequestToOrganization(SendDataRequestToOrganizationDto sendDataRequestToOrganizationDto)
        {
            var userDetails = await _userRepository.GetAsync((long)_abpSession.UserId);
            // The list is static the id in this case is the name of the company
            var targetedOrganizationName = sendDataRequestToOrganizationDto.OrganizationsId;
            var requesterOrganizationName = await _organisationRepository.GetAsync((Guid)userDetails.OrganizationId);

            try
            {
                var emailModel = new SendDataRequestToOrganizationEmail()
                {
                    FromEmail = _config.GetValue<string>("App:FromEmail"),
                    ToEmail = "ruben@climatecamp.io",
                    Subject = "New data request to an organization",
                    requesterFullName = userDetails.FullName,
                    requesterEmailAdress = userDetails.EmailAddress,
                    requesterOrganizationName = requesterOrganizationName.Name,
                    targetedOrganizationsName = targetedOrganizationName,
                    TemplateName = "SendDataRequestToOrganization",
                };
                await _emailClientSender.SendEmail(emailModel, _config.GetValue<string>("App:Functions:EmailSenderFunctionUrl"));

                var teamsNotification = new SendDataRequestToOrganizationEmail()
                {
                    FromEmail = _config.GetValue<string>("App:FromEmail"),
                    ToEmail = _config.GetValue<string>("App:LiveMonitoringEmailCollaboration"),
                    Subject = "New data request to an organization",
                    requesterFullName = userDetails.FullName,
                    requesterEmailAdress = userDetails.EmailAddress,
                    requesterOrganizationName = requesterOrganizationName.Name,
                    targetedOrganizationsName = targetedOrganizationName,
                    TemplateName = "SendDataRequestToOrganization",
                };
                await _emailClientSender.SendEmail(teamsNotification, _config.GetValue<string>("App:Functions:EmailSenderFunctionUrl"));
                return true;
            }
            catch (Exception exception) {
                _logger.LogInformation($"Method: SendDataRequestToOrganization - Exception: {exception}");
                return false;
            }
        }

        public async Task<float?> GetOrganizationTotalEmissions(Guid organizationId)
        {
            try
            {
                float? totalEmissions = 0;

                //Get All Organization Units in heirarchical order for the organization
                var organizationUnitsList = await _organizationUnitRepository.GetAll()
                                        .Where(x => x.OrganizationId == organizationId)
                                        .Select(x => new { x.Id, x.ParentOrganizationUnitId })
                                        .ToListAsync();

                //Get the top level or parent organization unit
                var parentOrganizationUnit = organizationUnitsList.First(x => x.ParentOrganizationUnitId == null);

                var parentOrganizationUnitData = await _emissionsSummaryRepository.GetAll()
                                         .Where(x => x.OrganizationUnitId == parentOrganizationUnit.Id)
                                         .ToListAsync();

                if (parentOrganizationUnitData.Any())
                {
                    foreach (var org in parentOrganizationUnitData)
                    {
                        if (!org.IsCO2ePerProductionUnitActive)
                        {
                            totalEmissions += org.Scope1tCO2e + org.Scope2tCO2e + org.Scope3tCO2e;
                        }
                        else
                        {
                            totalEmissions += org.Scope1CO2ePPU + org.Scope2CO2ePPU + org.Scope3CO2ePPU;
                        }
                    }

                    totalEmissions = totalEmissions / parentOrganizationUnitData.Sum(x => x.ProductionQuantity);
                }

                return totalEmissions;

            }
            catch (Exception exception)
            {
                _logger.LogInformation($"Method: GetOrganizationTotalEmissions - Exception: {exception}");
                return null;
            }

        }

        public async Task<float?> GetOrganizationProductionQuantity(Guid organizationId, int period)
        {
            try
            {
                float? productionQuantity = 0;

                //Get All Organization Units in heirarchical order for the organization
                var organizationUnitsList = await _organizationUnitRepository.GetAll()
                                        .Where(x => x.OrganizationId == organizationId)
                                        .Select(x => new { x.Id, x.ParentOrganizationUnitId })
                                        .ToListAsync();

                //Get the top level or parent organization unit
                var parentOrganizationUnit = organizationUnitsList.First(x => x.ParentOrganizationUnitId == null);

                var parentOrganizationUnitData = await _emissionsSummaryRepository.GetAll()
                                         .Where(x => x.OrganizationUnitId == parentOrganizationUnit.Id && x.Period == period)
                                         .ToListAsync();

                if (parentOrganizationUnitData.Any())
                {
                    productionQuantity = parentOrganizationUnitData.Sum(x => x.ProductionQuantity);
                }

                //If production quantity does not exists for the top level organization unit against selected period
                //then get the production quantity against all the child level organization units against the selected period
                if (productionQuantity == 0)
                {

                    var childOrganizationUnits = organizationUnitsList
                                                .Where(x => x.ParentOrganizationUnitId != null)
                                                .Select(x => x.Id)
                                                .ToList();

                    productionQuantity = await _emissionsSummaryRepository.GetAll()
                                         .Where(x => childOrganizationUnits.Contains(x.OrganizationUnitId) && x.Period == period)
                                         .SumAsync(x => x.ProductionQuantity);

                }

                return productionQuantity;

            }
            catch (Exception exception)
            {
                _logger.LogInformation($"Method: GetOrganizationTotalEmissions - Exception: {exception}");
                return null;
            }

        }
    }
}
