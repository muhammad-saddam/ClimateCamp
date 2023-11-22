using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using ClimateCamp.CarbonCompute;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ClimateCamp.CarbonCompute.GHG;
using static System.Net.WebRequestMethods;

namespace ClimateCamp.Application
{
    [AbpAuthorize]
    public class ProductEmissionsAppService : AsyncCrudAppService<ProductEmissions, ProductEmissionsDto, Guid, ProductEmissionsPagedResultDto, CreateProductEmissionsDto, CreateProductEmissionsDto>, IProductEmissionsAppService
    {
        private readonly IRepository<ProductEmissions, Guid> _productEmissionsRepository;
        private readonly ILogger<ProductEmissionsAppService> _logger;

        /// <param name="productEmissionsRepository"></param>
        /// <param name="logger"></param>
        public ProductEmissionsAppService(IRepository<ProductEmissions, Guid> productEmissionsRepository,
            ILogger<ProductEmissionsAppService> logger) : base(productEmissionsRepository)
        {
            _productEmissionsRepository = productEmissionsRepository;
            _logger = logger;
        }

        public async Task<ProductEmissionsDto> SaveProductEmissions(CreateProductEmissionsDto createProductEmissionsDto)
        {
            try
            {
                if (createProductEmissionsDto.Id == Guid.Empty)
                    return await base.CreateAsync(createProductEmissionsDto);
                else
                    return await base.UpdateAsync(createProductEmissionsDto);
            }
            catch (Exception exception)
            {
                _logger.LogInformation($"Method: SaveProductEmissions - Exception: {exception}");

                return null;
            }
        }

        public async Task<ProductEmissionsDto> GetProductEmission(Guid productId, int periodType, int period, int year)
        {
            try
            {
                var data = await _productEmissionsRepository.GetAll()
                            .Include(x => x.ProductsEmissionSources)
                            .Where(x => x.ProductId == productId &&
                            x.EmissionSourceType == (int)ProductEmissionTypeEnum.Product &&
                            ((periodType == 1 || periodType == 2) ?
                            (x.PeriodType == periodType && x.Period == period && x.Year == year) :
                            x.Year == year)).FirstOrDefaultAsync();

                return ObjectMapper.Map<ProductEmissionsDto>(data);
            }
            catch (Exception exception)
            {
                _logger.LogInformation($"Method: GetProductEmission - Exception: {exception}");

                return null;
            }
        }

        /// <summary>
        /// Method used to get the detailed PCF for a specific year. <br/>
        /// If no detailed PCF found, look for detailed PCF from previous years so it can be used as default values. <br/>
        /// e.g. No detailed PCF for 2022 but available for 2020. Add the 2020 data to the Dto as "PreviousProductsEmissionSources" property. <br/>
        /// This will be used in the frontend to default the Scope tab values. <br/>
        /// See <see href="https://dev.azure.com/climatecamp/CarbonCompute/_sprints/backlog/CarbonCompute%20Team/CarbonCompute/2023.Q1/Sprint%2037?workitem=1456">User story 1456</see> for more details.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="periodType"></param>
        /// <param name="period"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public async Task<ProductEmissionsVM> GetCurrentOrPreviousProductEmissions(Guid productId, int periodType, int period, int year)
        {
            try
            {
                var data = await _productEmissionsRepository.GetAll()
                            .Include(x => x.ProductsEmissionSources)
                            .Where(x => x.ProductId == productId &&
                            x.EmissionSourceType == (int)ProductEmissionTypeEnum.Product &&
                            ((periodType == 1 || periodType == 2) ?
                            (x.PeriodType == periodType && x.Period == period && x.Year == year) :
                            x.Year == year)).FirstOrDefaultAsync();


                if (data != null && !data.ProductsEmissionSources.Any())
                {
                    var yearsList = _productEmissionsRepository.GetAll()
                        .Include(x => x.ProductsEmissionSources)
                        .Where(x => x.ProductId == productId && x.Year < year)
                        .Select(x => x.Year)
                        .Distinct()
                        .OrderByDescending(x => x).ToList();

                    if (yearsList.Any())
                    {
                        foreach (var previousYear in yearsList)
                        {
                            var productEmission = await GetProductEmission(productId, periodType, period, (int)previousYear);
                            if (productEmission.ProductsEmissionSources.Any())
                            {
                                var previousProductEmissions = new ProductEmissionsVM()
                                {
                                    PreviousProductsEmissionSources = productEmission.ProductsEmissionSources.Select(p => new ProductsEmissionSourcesDto
                                    {
                                        EmissionsSourceId = p.EmissionsSourceId,
                                        ProductEmissionsId = p.ProductEmissionsId,
                                        Availability = p.Availability,
                                        tCO2e = null,
                                        Methodology = p.Methodology,
                                        PrimaryDataShare = null,
                                    }).ToList()
                                };
                                ObjectMapper.Map(data, previousProductEmissions);
                                return previousProductEmissions;
                            }
                        }
                    }

                }

                return ObjectMapper.Map<ProductEmissionsVM>(data);

            }
            catch (Exception exception)
            {
                _logger.LogInformation($"Method: GetCurrentAndPreviousProductEmissions - Exception: {exception}");

                return null;
            }
        }

    }
}
