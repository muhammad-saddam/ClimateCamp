using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.UI;
using ClimateCamp.Core;
using ClimateCamp.Core.CarbonCompute.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace ClimateCamp.Application
{
    [AbpAuthorize]
    public class EmissionsSummaryAppService : AsyncCrudAppService<Core.EmissionsSummary, EmissionsSummaryDto, Guid, PagedEmissionsSummaryResultRequestDto, CreateEmissionsSummaryDto, CreateEmissionsSummaryDto>
    {
        private readonly IRepository<Core.EmissionsSummary, Guid> _emissionsSummaryRepository;
        private readonly IRepository<Core.EmissionsSummaryScopeDetails, int> _emissionsSummaryScopeDetails;
        private readonly ILogger<ActivityDataAppService> _logger;

        public EmissionsSummaryAppService(
           IRepository<Core.EmissionsSummary, Guid> emissionsSummaryRepository,
           IRepository<EmissionsSummaryScopeDetails, int> emissionsSummaryScopeDetails,
           ILogger<ActivityDataAppService> logger) : base(emissionsSummaryRepository)

        {
            _emissionsSummaryRepository = emissionsSummaryRepository;
            _emissionsSummaryScopeDetails = emissionsSummaryScopeDetails;
            _logger = logger;
        }


        public async Task<EmissionsSummaryDto>GetEmissionSummaryByOrgUnit(Guid organizationUnitId, int? period, bool checkForDraft)
        {
            try
            {
                Core.EmissionsSummary emissionSummary;
                if (period != null) {
                    emissionSummary = await _emissionsSummaryRepository
                    .FirstOrDefaultAsync(e => e.OrganizationUnitId == organizationUnitId && e.Period == period && (checkForDraft ? !e.IsDraft : true));

                }
                else
                {
                    emissionSummary = await _emissionsSummaryRepository.GetAll()
                        .OrderByDescending(e => e.Period)
                        .FirstOrDefaultAsync(e => e.OrganizationUnitId == organizationUnitId);
                }

                if(emissionSummary != null)
                {
                    var scope3Details = _emissionsSummaryScopeDetails.GetAll()
                   .Where(x => x.EmissionSummaryId == emissionSummary.Id)
                   .OrderBy(x => x.Availability == Availability.NotApplicable.To<int>())
                   .ThenBy(x => x.Availability == Availability.Available.To<int>())
                   .ThenBy(x => x.Availability == Availability.Unavailable.To<int>());

                    EmissionsSummaryDto emissionsSummaryDto = new EmissionsSummaryDto
                    {
                        Id = emissionSummary.Id,
                        OrganizationUnitId = organizationUnitId,
                        Period = emissionSummary.Period,
                        ProductionMetricId = emissionSummary.ProductionMetricId,
                        ProductionQuantity = emissionSummary.ProductionQuantity,
                        IsActiveScope1Emissions = emissionSummary.IsActiveScope1Emissions == null ? true : emissionSummary.IsActiveScope1Emissions,
                        Scope1tCO2e = emissionSummary.Scope1tCO2e,
                        IsActiveScope2Emissions = emissionSummary.IsActiveScope2Emissions == null ? true : emissionSummary.IsActiveScope2Emissions,
                        Scope2Methodology = emissionSummary.Scope2Methodology,
                        Scope2tCO2e = emissionSummary.Scope2tCO2e,
                        IsActiveScope3Emissions = emissionSummary.IsActiveScope3Emissions == null ? true : emissionSummary.IsActiveScope3Emissions,
                        Scope3tCO2e = emissionSummary.Scope3tCO2e,
                        Scope3PrimaryDataShare = emissionSummary.Scope3PrimaryDataShare,
                        TotalPCfProxy = emissionSummary.TotalPCfProxy,
                        Audited = emissionSummary.Audited,
                        Auditor = emissionSummary.Auditor,
                        Certificate = emissionSummary.Certificate,
                        IsScope1DetailViewActive = emissionSummary.IsScope1DetailViewActive ?? false,
                        IsScope2DetailViewActive = emissionSummary.IsScope2DetailViewActive ?? false,
                        IsScope3DetailViewActive = emissionSummary.IsScope3DetailViewActive,
                        IsCO2ePerProductionUnitActive = emissionSummary.IsCO2ePerProductionUnitActive,
                        Scope1CO2ePPU = emissionSummary.Scope1CO2ePPU,
                        Scope2CO2ePPU = emissionSummary.Scope2CO2ePPU,
                        Scope3CO2ePPU = emissionSummary.Scope3CO2ePPU,
                        EmissionsSummaryScopeDetails = scope3Details.ToList()
                    };
                    return emissionsSummaryDto;
                }
                else
                {   
                    return new EmissionsSummaryDto
                    {
                        Id = Guid.Empty,
                        OrganizationUnitId = organizationUnitId,
                        Period = period != null ? period : null,
                        ProductionMetricId = 0,
                        ProductionQuantity = 0,
                        Scope1tCO2e = 0,
                        Scope2Methodology = 0,
                        Scope2tCO2e = 0,
                        Scope3tCO2e = 0,
                        Scope3PrimaryDataShare = 0,
                        Audited = false,
                        EmissionsSummaryScopeDetails = new List<EmissionsSummaryScopeDetails>()
                    };
                }
                
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"Method: GetEmissionSummaryByOrgUnit - Exception: {ex}");
                return null;
            }
            
        }


        /// <summary>
        /// Gets the emission summary if available for the reference period. If not found, will add the data from the closest period in order to have that as default. <br/>
        /// See User story #1580 for more details.
        /// </summary>
        /// <param name="organizationUnitId"></param>
        /// <param name="period"></param>
        /// <param name="checkForDraft"></param>
        /// <returns></returns>
        public async Task<EmissionsSummaryDto> GetCurrentOrClosestPeriodEmissionSummaryByOrgUnit(Guid organizationUnitId, int? period, bool checkForDraft)
        {
            try
            {
                Core.EmissionsSummary emissionSummary;
                if (period != null)
                {
                   emissionSummary = await _emissionsSummaryRepository
                    .FirstOrDefaultAsync(e => e.OrganizationUnitId == organizationUnitId && e.Period == period && (checkForDraft ? !e.IsDraft : true));

                }
                else
                {
                    emissionSummary = await _emissionsSummaryRepository.GetAll()
                        .OrderByDescending(e => e.Period)
                        .FirstOrDefaultAsync(e => e.OrganizationUnitId == organizationUnitId);
                }

                // case when there is an emissions summary for the reference period as well as detailed scope emissions summary
                if (emissionSummary != null)
                {
                    var scopeDetails = await _emissionsSummaryScopeDetails.GetAll()
                   .Where(x => x.EmissionSummaryId == emissionSummary.Id)
                   .OrderBy(x => x.Availability == Availability.NotApplicable.To<int>())
                   .ThenBy(x => x.Availability == Availability.Available.To<int>())
                   .ThenBy(x => x.Availability == Availability.Unavailable.To<int>())
                   .ToListAsync();

                    EmissionsSummaryDto emissionsSummaryDto = new EmissionsSummaryDto
                    {
                        Id = emissionSummary.Id,
                        OrganizationUnitId = organizationUnitId,
                        Period = emissionSummary.Period,
                        ProductionMetricId = emissionSummary.ProductionMetricId,
                        ProductionQuantity = emissionSummary.ProductionQuantity,
                        IsActiveScope1Emissions = emissionSummary.IsActiveScope1Emissions == null ? true : emissionSummary.IsActiveScope1Emissions,
                        Scope1tCO2e = emissionSummary.Scope1tCO2e,
                        IsActiveScope2Emissions = emissionSummary.IsActiveScope2Emissions == null ? true : emissionSummary.IsActiveScope2Emissions,
                        Scope2Methodology = emissionSummary.Scope2Methodology,
                        Scope2tCO2e = emissionSummary.Scope2tCO2e,
                        IsActiveScope3Emissions = emissionSummary.IsActiveScope3Emissions == null ? true : emissionSummary.IsActiveScope3Emissions,
                        Scope3tCO2e = emissionSummary.Scope3tCO2e,
                        Scope3PrimaryDataShare = emissionSummary.Scope3PrimaryDataShare,
                        TotalPCfProxy = emissionSummary.TotalPCfProxy,
                        Audited = emissionSummary.Audited,
                        Auditor = emissionSummary.Auditor,
                        Certificate = emissionSummary.Certificate,
                        IsScope3DetailViewActive = emissionSummary.IsScope3DetailViewActive,
                        IsCO2ePerProductionUnitActive = emissionSummary.IsCO2ePerProductionUnitActive,
                        Scope1CO2ePPU = emissionSummary.Scope1CO2ePPU,
                        Scope2CO2ePPU = emissionSummary.Scope2CO2ePPU,
                        Scope3CO2ePPU = emissionSummary.Scope3CO2ePPU,
                        EmissionsSummaryScopeDetails = scopeDetails.ToList()
                    };
                    return emissionsSummaryDto;
                }
                else
                {
                    var emissionsSummary = await GetEmissionsSummarybyClosestAvailableYear(organizationUnitId, period, checkForDraft);
                    return emissionsSummary;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Method: GetCurrentOrClosestPeriodEmissionSummaryByOrgUnit - Exception: {ex}");
                return null;
            }

        }

        /// <summary>
        /// Method used to sort years based on distance from reference year, from closest past/future year to furthest.
        /// </summary>
        /// <param name="period"></param>
        /// <param name="yearsList"></param>
        /// <returns></returns>
        public static List<(Guid Id, int Period)> SortYearsByDistanceFromReference(int? period, List<(Guid Id, int Period)> yearsList)
        {
            if (!period.HasValue)
            {
                return yearsList;
            }

            var sortedList = yearsList.OrderBy(x => Math.Abs(x.Period - period.Value)).ToList();

            return sortedList;
        }


        /// <summary>
        /// Gets the detailed organizational emissions summary from the closest available year based on the reference year.
        /// </summary>
        /// <param name="organizationUnitId"></param>
        /// <param name="period"></param>
        /// <param name="checkForDraft"></param>
        /// <returns></returns>
        public async Task<EmissionsSummaryDto> GetEmissionsSummarybyClosestAvailableYear(Guid organizationUnitId, int? period, bool checkForDraft)
        {
            try
            {
                // get list of years for which there is an emissionSummary(organizationUnitId)
                var yearsList = await _emissionsSummaryRepository.GetAll()
                    .Where(x => x.OrganizationUnitId == organizationUnitId && (checkForDraft ? !x.IsDraft : true))
                    .OrderByDescending(x => x.Period)
                    .Select(x => new { x.Id, x.Period })
                    .Distinct()
                    .ToListAsync();

                // sort the list from the closest previous/future year to the furthest previous/future year
                var sortedYearsList = SortYearsByDistanceFromReference(period, yearsList.Select(x => (x.Id, x.Period)).ToList());

                if (sortedYearsList.Any())
                {
                    foreach (var year in sortedYearsList)
                    {
                        var allScopesDetails = _emissionsSummaryScopeDetails.GetAll()
                            .Where(x => x.EmissionSummaryId == year.Id)
                            .OrderBy(x => x.Availability == Availability.NotApplicable.To<int>())
                            .ThenBy(x => x.Availability == Availability.Available.To<int>())
                            .ThenBy(x => x.Availability == Availability.Unavailable.To<int>()).ToList();

                        // Check if there are any entries with anything except "Unavailable" and if so, assign those values as 'EmissionsSummaryScopeDetails'
                        if (allScopesDetails.Any(x => x.Availability != (int)Availability.Unavailable))
                        {
                            var selectedScopeDetails = allScopesDetails
                                    .Select(x => new SelectedScopeDetails
                                    {
                                        EmissionSourceId = x.EmissionSourceId,
                                        Availability = x.Availability,
                                        Methodology = x.Methodology,
                                        EmissionScope = x.EmissionScope
                                    })
                                    .ToList();
                            return new EmissionsSummaryDto
                            {
                                Id = Guid.Empty,
                                OrganizationUnitId = organizationUnitId,
                                Period = period != null ? period : null,
                                ProductionMetricId = 0,
                                ProductionQuantity = 0,
                                Scope1tCO2e = 0,
                                Scope2Methodology = 0,
                                Scope2tCO2e = 0,
                                Scope3tCO2e = 0,
                                Scope3PrimaryDataShare = 0,
                                Audited = false,
                                EmissionsSummaryScopeDetails = selectedScopeDetails.Select(x => new EmissionsSummaryScopeDetails
                                {
                                    Id = 0,
                                    EmissionSummaryId = Guid.Empty,
                                    EmissionSourceId = x.EmissionSourceId,
                                    Availability = x.Availability,
                                    tCO2e = null,
                                    tCO2ePPU = null,
                                    Methodology = x.Methodology,
                                    PrimaryDataShare = null,
                                    EmissionScope = x.EmissionScope,
                                    SelectedAvailability = null,
                                    SelectedMethodology = null,
                                }).ToList()
                            };
                        }

                    }
                    // case when there is no emission summary for the reference period and all other periods have no emissions summary scope details either
                    return new EmissionsSummaryDto
                    {
                        Id = Guid.Empty,
                        OrganizationUnitId = organizationUnitId,
                        Period = period != null ? period : null,
                        ProductionMetricId = 0,
                        ProductionQuantity = 0,
                        Scope1tCO2e = 0,
                        Scope2Methodology = 0,
                        Scope2tCO2e = 0,
                        Scope3tCO2e = 0,
                        Scope3PrimaryDataShare = 0,
                        Audited = false,
                        EmissionsSummaryScopeDetails = new List<EmissionsSummaryScopeDetails>()
                    };
                }
                else
                {
                    // case when there is no emissions summary for any year as well as no detailed scope emissions summary
                    return new EmissionsSummaryDto
                    {
                        Id = Guid.Empty,
                        OrganizationUnitId = organizationUnitId,
                        Period = period != null ? period : null,
                        ProductionMetricId = 0,
                        ProductionQuantity = 0,
                        Scope1tCO2e = 0,
                        Scope2Methodology = 0,
                        Scope2tCO2e = 0,
                        Scope3tCO2e = 0,
                        Scope3PrimaryDataShare = 0,
                        Audited = false,
                        EmissionsSummaryScopeDetails = new List<EmissionsSummaryScopeDetails>()
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Method: GetEmissionsSummarybyClosestAvailableYear - Exception: {ex}");
                return null;
            }
        }


        public async Task<CreateEmissionsSummaryDto> AddEmissionsSummaryByOrganizationUnitAsync(CreateEmissionsSummaryDto input)
        {
            try
            {
              
                var emissionSummary = _emissionsSummaryRepository.FirstOrDefault(x => x.Period == input.Period && x.OrganizationUnitId == input.OrganizationUnitId);
                if (emissionSummary != null)
                {
                    emissionSummary.ProductionMetricId = input.ProductionMetricId;
                    emissionSummary.ProductionQuantity = input.ProductionQuantity;
                    emissionSummary.IsActiveScope1Emissions = input.IsActiveScope1Emissions;
                    emissionSummary.Scope1tCO2e = input.Scope1tCO2e;
                    emissionSummary.IsActiveScope2Emissions = input.IsActiveScope2Emissions;
                    emissionSummary.Scope2tCO2e = input.Scope2tCO2e;
                    emissionSummary.Scope2Methodology = input.Scope2Methodology;
                    emissionSummary.IsActiveScope3Emissions = input.IsActiveScope3Emissions;
                    emissionSummary.Scope3tCO2e = input.Scope3tCO2e;    
                    emissionSummary.Scope3PrimaryDataShare = input.Scope3PrimaryDataShare;
                    emissionSummary.TotalPCfProxy = input.TotalPCfProxy;
                    emissionSummary.Audited = input.Audited;
                    emissionSummary.Auditor = input.Auditor;
                    emissionSummary.Certificate = input.Certificate;
                    emissionSummary.IsScope1DetailViewActive = input.IsScope1DetailViewActive;
                    emissionSummary.IsScope2DetailViewActive = input.IsScope2DetailViewActive;
                    emissionSummary.IsScope3DetailViewActive = input.IsScope3DetailViewActive;
                    emissionSummary.IsCO2ePerProductionUnitActive = input.IsCO2ePerProductionUnitActive;
                    emissionSummary.Scope1CO2ePPU = input.Scope1CO2ePPU;
                    emissionSummary.Scope2CO2ePPU = input.Scope2CO2ePPU;
                    emissionSummary.Scope3CO2ePPU = input.Scope3CO2ePPU;
                    emissionSummary.IsDraft = input.IsDraft;
                    await _emissionsSummaryRepository.UpdateAsync(emissionSummary);
                    CurrentUnitOfWork.SaveChanges();
                }
                else
                {
                    var emissionsSummary = ObjectMapper.Map<Core.EmissionsSummary>(input);
                    var result = await _emissionsSummaryRepository.InsertAsync(emissionsSummary);
                    CurrentUnitOfWork.SaveChanges();
                    input.Id = result.Id;
                }

                foreach (var scope3Detail in input.EmissionsSummaryScopeDetails)
                {
                    scope3Detail.EmissionSummaryId = emissionSummary != null ? emissionSummary.Id : input.Id;
                    var scope3EmissionsDetail = _emissionsSummaryScopeDetails.FirstOrDefault(x => x.EmissionSummaryId == (emissionSummary != null ? emissionSummary.Id : input.Id) && x.EmissionSourceId == scope3Detail.EmissionSourceId);
                    if (scope3EmissionsDetail != null)
                    {
                        scope3EmissionsDetail.Availability = scope3Detail.SelectedAvailability.id;
                        scope3EmissionsDetail.Methodology = scope3Detail.SelectedMethodology != null ? scope3Detail.SelectedMethodology.id : null;
                        scope3EmissionsDetail.PrimaryDataShare = scope3Detail.PrimaryDataShare; 
                        scope3EmissionsDetail.tCO2e = scope3Detail.tCO2e;
                        scope3EmissionsDetail.tCO2ePPU = scope3Detail.tCO2ePPU;
                        await _emissionsSummaryScopeDetails.UpdateAsync(scope3EmissionsDetail);
                        CurrentUnitOfWork.SaveChanges();
                    }
                    else
                    {
                        var emissionsSummary = ObjectMapper.Map<EmissionsSummaryScopeDetails>(scope3Detail);
                        var result = await _emissionsSummaryScopeDetails.InsertAsync(emissionsSummary);
                        result.Availability = result.SelectedAvailability.id;   
                        result.Methodology = result.SelectedMethodology != null ? result.SelectedMethodology.id : null;
                        CurrentUnitOfWork.SaveChanges();
                        scope3Detail.Id = result.Id;
                    }

                }
                return input;
            }
            catch (UserFriendlyException userEx)
            {
                _logger.LogError($"Method: AddEmissionsSummaryByOrganizationUnitAsync - Exception: {userEx.Message}");
                throw new UserFriendlyException(userEx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Method: AddEmissionsSummaryByOrganizationUnitAsync - Exception: {ex}");
                return null;
            }
        }

        public async Task<PagedResultDto<EmissionsSummaryDto>> GetEmissionsSummaryByOrganizationUnit(Guid organizationUnitId, bool checkForDraft)
        {
            try
            {
                var emissionsSummary = await _emissionsSummaryRepository
                    .GetAll()
                    .Where(x => x.OrganizationUnitId == organizationUnitId && (checkForDraft ? !x.IsDraft : true))
                    .ToListAsync();

                var result = new PagedResultDto<EmissionsSummaryDto>()
                {
                    Items = ObjectMapper.Map<List<EmissionsSummaryDto>>(emissionsSummary),
                    TotalCount = emissionsSummary.Count
                };

                return result;
            }
            catch (Exception exception)
            {
                _logger.LogError($"Method: GetEmissionsSummaryByOrganizationUnit - Exception: {exception}");
                return null;
            }
        }

        public class SelectedScopeDetails
        {
            public int EmissionSourceId { get; set; }
            public int? Availability { get; set; }
            public int? Methodology { get; set; }
            public int EmissionScope { get; set; }
        }
    }
}
