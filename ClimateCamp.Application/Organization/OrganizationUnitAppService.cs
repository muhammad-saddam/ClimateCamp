using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.MultiTenancy;
using ClimateCamp.Common.Dto;
using ClimateCamp.Core;
using ClimateCamp.Core.Authorization;
using ClimateCamp.Infrastructure.FileUploadService;
using ClimateCamp.Lookup;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClimateCamp.Application
{
    [AbpAuthorize(PermissionNames.Pages_Company)]
    public class OrganizationUnitAppService : AsyncCrudAppService<OrganizationUnit, OrganizationUnitDto, Guid, PagedCompanyResultRequestDto, CreateOrganizationUnitDto, CreateOrganizationUnitDto>, IOrganizationUnitAppService
    {
        private readonly IRepository<OrganizationUnit, Guid> _companyRepository;
        private readonly IFileUploadService _fileUploadService;
        private readonly IRepository<Country, int> _countryRepository;

        public OrganizationUnitAppService(
            IRepository<OrganizationUnit, Guid> companyRepository,
            IRepository<Country, int> countryRepository,
            IFileUploadService fileUploadService) : base(companyRepository)
        {
            _companyRepository = companyRepository;
            _fileUploadService = fileUploadService;
            _countryRepository = countryRepository ?? throw new ArgumentNullException(nameof(countryRepository));
        }

        public async Task<PagedResultDto<OrganizationUnitDto>> GetAllAsyncWithChildren(GetAllAsyncWithChildrenDto input)
        {
            var countries = _countryRepository.GetAll();

            var organizationUnits = await _companyRepository.GetAll()
                .Where(t => t.OrganizationId == input.OrganizationId)
                .Include(t => t.Country)
                .OrderBy(x => x.Name)
                .ToListAsync();
            var recursiveOUnits = organizationUnits.Where(t => t.ParentOrganizationUnitId == null).ToList();

            var result = new PagedResultDto<OrganizationUnitDto>()
            {
                Items = ObjectMapper.Map<List<OrganizationUnitDto>>(recursiveOUnits),
                TotalCount = recursiveOUnits.Count
            };

            foreach (var company in result.Items)
            {
                foreach (var child in company.Children)
                {
                    if (child.CountryId != null)
                    {
                        var country = countries.FirstOrDefault(t => t.Id == child.CountryId);
                        child.CountryName = country != null ? country.Name : child.CountryName;
                    }
                }
            }

            return result;
        }

        public async Task<PagedResultDto<OrganizationUnitDto>> GetAllOrganizationUnitsByOrganizationId(Guid organizationId)
        {
            var organizationUnits = await _companyRepository.GetAll().Where(x => x.OrganizationId == organizationId).ToListAsync();

            var result = new PagedResultDto<OrganizationUnitDto>()
            {
                Items = ObjectMapper.Map<List<OrganizationUnitDto>>(organizationUnits),
                TotalCount = organizationUnits.Count
            };

            return result;
        }

        public async Task<OrganizationUnitDto> CreateAsyncWithFile(CreateOrganizationUnitDto input)
        {
            var company = await base.CreateAsync(input);
            return company;
        }

        public async Task<Boolean> UploadCompanyLogo(IFormFile file, [FromForm] Guid CompanyId)
        {
            if (CompanyId != Guid.Empty)
            {
                var company = await _companyRepository.GetAsync(CompanyId);

                if (company == null)
                    return false;
                var companyLogo = new Infrastructure.Models.UploadFileModel()
                {
                    BlobContainerName = SettingManager.GetSettingValue("OrganizationBlobContainerName"),
                    File = file,
                    FileNameWithExtension = file.FileName,
                    Path = Convert.ToString(MultiTenancyConsts.DefaultTenantId) + "/organizations/" + company.OrganizationId + "/companies/" + Convert.ToString(CompanyId)
                };
                var url = await _fileUploadService.UploadFileAsync(companyLogo);
                if (!string.IsNullOrEmpty(url))
                {
                    company.PicturePath = url;
                    company.PictureName = companyLogo.FileNameWithExtension;
                    await _companyRepository.UpdateAsync(company);
                }
                return true;
            }
            return false;
        }

        public async Task<List<DDLDto<Guid>>> GetParentCompaniesDropDown(GetParentCompaniesDropDownDto input)
        {
            List<DDLDto<Guid>> companies;
            if (input.CompanyId != Guid.Empty)
            {
                companies = await _companyRepository.GetAll()
                   .Where(t => t.Id != input.CompanyId && t.ParentOrganizationUnitId != input.CompanyId && t.OrganizationId == input.OrganizationId && !t.IsDeleted)
                   .OrderBy(t => t.Name)
                   .Select(t => new DDLDto<Guid>() { Id = t.Id, Name = t.Name }).ToListAsync();
            }
            else
            {
                companies = await _companyRepository.GetAll()
                   .Where(t => t.OrganizationId == input.OrganizationId && !t.IsDeleted)
                   .OrderBy(t => t.Name)
                   .Select(t => new DDLDto<Guid>() { Id = t.Id, Name = t.Name }).ToListAsync();
            }
            return companies;
        }

    }
}
