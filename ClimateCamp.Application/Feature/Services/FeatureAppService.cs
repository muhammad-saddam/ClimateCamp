using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using ClimateCamp.Application;
using ClimateCamp.Feature.Dto;

namespace ClimateCamp.Feature.Services
{
    [AbpAuthorize]
    public class FeatureAppService : AsyncCrudAppService<Core.Features.EditionFeatureSettingCustom, FeatureDto, long>, IDataCollectionAppService
    {
        private readonly IRepository<Core.Features.EditionFeatureSettingCustom, long> _featureRepository;

        public FeatureAppService(
          IRepository<Core.Features.EditionFeatureSettingCustom, long> featureRepository) : base(featureRepository)
        {
            _featureRepository = featureRepository;
        }
    }
}
