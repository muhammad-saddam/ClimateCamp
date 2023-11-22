using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using ClimateCamp.CarbonCompute;
using ClimateCamp.CarbonCompute.EmissionSources.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClimateCamp.Application
{
    [AbpAuthorize]
    public class EmissionSourcesAppService : AsyncCrudAppService<EmissionsSource, EmissionSourceDto, int, PagedEmissionSourceResponseDto, CreateEmissionSourcesDto, CreateEmissionSourcesDto>
    {
        private readonly IRepository<EmissionsSource, int> _emissionSourceRepository;
        private readonly IRepository<Emission, Guid> _emissionRepository;
        private readonly IRepository<Unit, int> _unitRepository;

        public EmissionSourcesAppService(
            IRepository<EmissionsSource, int> emissionSourceRepository, IRepository<Emission, Guid> emissionRepository, IRepository<Unit, int> unitRepository) : base(emissionSourceRepository)
        {
            _emissionSourceRepository = emissionSourceRepository;
            _emissionRepository = emissionRepository;
            _unitRepository = unitRepository;
        }
        /// <summary>
        /// Get all emission sources
        /// </summary>
        /// <returns></returns>
        public async Task<PagedResultDto<EmissionSourcesVM>> GetAllEmissionSources()
        {

            try
            {
                var emissionSources = _emissionSourceRepository.GetAll();
                List<EmissionSourcesVM> emissionSourcesList = new List<EmissionSourcesVM>();
                foreach (var emissionSource in emissionSources)
                {
                    EmissionSourcesVM model = new EmissionSourcesVM
                    {
                        Name = emissionSource.Name,
                        Description = emissionSource.Description,
                        EmissionScope = emissionSource.EmissionScope,
                        IsActive = emissionSource.IsActive,
                        Id = emissionSource.Id
                    };
                    emissionSourcesList.Add(model);
                }
                var result = new PagedResultDto<EmissionSourcesVM>()
                {
                    Items = ObjectMapper.Map<List<EmissionSourcesVM>>(emissionSourcesList.Where(x => x.IsActive == true).OrderBy(x => x.Name)),
                    TotalCount = emissionSourcesList.Count
                };

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public async Task<PagedResultDto<EmissionSourcesVM>> GetAllScope3EmissionSources()
        {

            try
            {
                var emissionSources = _emissionSourceRepository.GetAll();
                List<EmissionSourcesVM> emissionSourcesList = new List<EmissionSourcesVM>();
                foreach (var emissionSource in emissionSources)
                {
                    EmissionSourcesVM model = new EmissionSourcesVM
                    {
                        Name = emissionSource.Name,
                        Description = emissionSource.Description,
                        EmissionScope = emissionSource.EmissionScope,
                        IsActive = emissionSource.IsActive,
                        Id = emissionSource.Id
                    };
                    emissionSourcesList.Add(model);
                }
                var scope3EmissionSourcesList = ObjectMapper.Map<List<EmissionSourcesVM>>(emissionSourcesList.Where(x => x.EmissionScope == GHG.EmissionScope.Scope3).OrderBy(x => x.Name));
                var result = new PagedResultDto<EmissionSourcesVM>()
                {
                    Items = scope3EmissionSourcesList,
                    TotalCount = scope3EmissionSourcesList.Count
                };

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Get all Emission sources with total Emissions
        /// </summary>
        /// <param name="consumptionStart"></param>
        /// <param name="consumptionEnd"></param>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<EmissionSourcesVM>> GetAllEmissionsCountByEmissionSources(DateTime? consumptionStart, DateTime? consumptionEnd, Guid organizationId)
        {

            try
            {
                var emissionSources = _emissionSourceRepository.GetAll();
                var kgId = _unitRepository.GetAll().Where(x => x.Name.ToLower() == "kg").FirstOrDefault().Id;
                var tId = _unitRepository.GetAll().Where(x => x.Name.ToLower() == "t").FirstOrDefault().Id;

                List<EmissionSourcesVM> emissionSourcesList = new List<EmissionSourcesVM>();


                // check uploaded activity data
                var EmissionsQuery = _emissionRepository.GetAll()
                          .Include(x => x.ActivityData)
                          .ThenInclude(x => x.ActivityType)
                          .ThenInclude(x => x.EmissionsSource)
                          .Include(x => x.OrganizationUnit)
                          .ThenInclude(x => x.Organization)
                          .Where(x => x.ActivityData.ConsumptionEnd.Date >= consumptionStart.Value.Date &&  // applied date filter based on date part only becasue of moment.js utc offset issue
                                      x.ActivityData.ConsumptionStart.Date <= consumptionEnd.Value.Date &&
                                      x.CO2E != null);

                if (organizationId != Guid.Empty)
                {
                    EmissionsQuery = EmissionsQuery.Where(x => x.ActivityData.OrganizationUnit.OrganizationId == organizationId);
                }
                var Emissions = await EmissionsQuery.ToListAsync();

                if (Emissions.Count == 0)
                {
                    Emissions = await _emissionRepository.GetAll()
                        .Include(x => x.ActivityData)
                        .ThenInclude(x => x.ActivityType)
                        .ThenInclude(x => x.EmissionsSource)
                        .Include(x => x.OrganizationUnit)
                        .ThenInclude(x => x.Organization)
                        .Where(x => x.ActivityData.OrganizationUnit.OrganizationId == organizationId
                        && x.ActivityData.DataQualityType == GHG.DataQualityType.Estimated
                        && x.EmissionsDataQualityScore == GHG.EmissionsDataQualityScore.Estimated
                        && x.ActivityData.ConsumptionStart >= consumptionStart && x.ActivityData.ConsumptionEnd <= consumptionEnd)
                        .ToListAsync();
                }

                foreach (var emissionSource in emissionSources)
                {
                    var subtotalTons = Emissions
                        .Where(x => x.ActivityData.ActivityType.EmissionsSourceId == emissionSource.Id && x.CO2EUnitId.Equals(tId))
                        .Sum(x => (double)x.CO2E);
                    var subtotalKg = Emissions
                        .Where(x => x.ActivityData.ActivityType.EmissionsSourceId == emissionSource.Id && x.CO2EUnitId.Equals(kgId))
                        .Sum(x => (double)x.CO2E);

                    EmissionSourcesVM model = new EmissionSourcesVM
                    {
                        TotalEmissions = Math.Round(subtotalTons + subtotalKg / 1000, 5),
                        Name = emissionSource.Name,
                        Description = emissionSource.Description,
                        EmissionScope = emissionSource.EmissionScope,
                        IsActive = emissionSource.IsActive,
                        Id = emissionSource.Id
                    };
                    emissionSourcesList.Add(model);
                }

                var result = new PagedResultDto<EmissionSourcesVM>()
                {
                    Items = ObjectMapper.Map<List<EmissionSourcesVM>>(emissionSourcesList.OrderBy(x => !x.IsActive).ThenBy(x => x.Name)),
                    TotalCount = emissionSourcesList.Count
                };

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async override Task<PagedResultDto<EmissionSourceDto>> GetAllAsync(PagedEmissionSourceResponseDto input)
        {
            var emissionSources = await _emissionSourceRepository.GetAllListAsync();

            var emissionSourcesList = new List<EmissionSourceDto>();

            foreach (var emissionSource in emissionSources)
            {
                EmissionSourceDto model = new EmissionSourceDto
                {
                    Name = emissionSource.Name,
                    Description = emissionSource.Description,
                    EmissionScope = emissionSource.EmissionScope,
                    Id = emissionSource.Id
                };
                emissionSourcesList.Add(model);
            }
            var result = new PagedResultDto<EmissionSourceDto>()
            {
                Items = ObjectMapper.Map<List<EmissionSourceDto>>(emissionSourcesList.OrderBy(x => x.EmissionScope).ThenBy(x => x.Name)),
                TotalCount = emissionSourcesList.Count
            };

            return result;
        }

    }
}
