using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using ClimateCamp.CarbonCompute;
using Microsoft.Extensions.Logging;
using System;

namespace ClimateCamp.Application
{
    [AbpAuthorize]
    public class CarbonFootprintsAppService : AsyncCrudAppService<CarbonFootprints, CarbonFootprintsDto, Guid, CarbonFootprintsPagedResultDto, CreateCarbonFootprintsDto, CreateCarbonFootprintsDto>, ICarbonFootprintsAppService
    {
        private readonly IRepository<CarbonFootprints, Guid> _carbonFootprintsRepository;
        private readonly ILogger<CarbonFootprintsAppService> _logger;

        /// <param name="carbonFootprintsRepository"></param>
        /// <param name="logger"></param>
        public CarbonFootprintsAppService(IRepository<CarbonFootprints, Guid> carbonFootprintsRepository,
            ILogger<CarbonFootprintsAppService> logger) : base(carbonFootprintsRepository)
        {
            _carbonFootprintsRepository = carbonFootprintsRepository;
            _logger = logger;
        }

    }
}
