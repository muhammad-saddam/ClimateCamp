using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Abp.UI;
using ClimateCamp.Application;
using ClimateCamp.Core.Editions;
using ClimateCamp.Core.Features;
using ClimateCamp.Edition.Dto;
using ClimateCamp.Feature.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClimateCamp.Edition.Services
{
    [AbpAuthorize]
    public class EditionAppService : AsyncCrudAppService<EditionFeatureSettingCustom, FeatureDto, long>, IDataCollectionAppService
    {
        private readonly IRepository<EditionFeatureSettingCustom, long> _featureRepository;
        private readonly IAbpSession _abpSession;
        private readonly IRepository<Common.Authorization.Users.User, long> _userRepository;
        private readonly IRepository<Core.Organization, Guid> _organizationRepository;
        private readonly IRepository<CustomEdition, int> _editionRepository;
        public EditionAppService(
           IRepository<EditionFeatureSettingCustom, long> featureRepository,
           IAbpSession abpSession,
           IRepository<Common.Authorization.Users.User, long> userRepository,
           IRepository<Core.Organization, Guid> organizationRepository,
           IRepository<CustomEdition, int> editionRepository) : base(featureRepository)
        {
            _featureRepository = featureRepository;
            _abpSession = abpSession;
            _userRepository = userRepository;
            _organizationRepository = organizationRepository;
            _editionRepository = editionRepository;
        }

        public async Task<List<FeatureDto>> GetAllFeaturesByEdition()
        {
            try
            {
                var organizationId = _userRepository.Get(_abpSession.GetUserId()).OrganizationId;

                if (organizationId == Guid.Empty || organizationId == null)
                    return ObjectMapper.Map<List<FeatureDto>>(_featureRepository.GetAll().Where(x => x.Type != (int)EditionFeartureType.EmissionSource).ToList());

                var editionId = _organizationRepository.Get(organizationId ?? Guid.Empty).EditionId;

                if (editionId == null)
                {
                    throw new UserFriendlyException("No edition assigned to your Organization. Please contact support!");
                }

                return ObjectMapper.Map<List<FeatureDto>>(_featureRepository.GetAll().Where(x => x.EditionId == editionId && x.Type != (int)EditionFeartureType.EmissionSource).ToList());
            }
            catch (UserFriendlyException exception)
            {
                throw new UserFriendlyException(exception.Message);
            }

        }

        public async Task<List<CustomEdition>> GetAllEditions()
        {
            try
            {
                var editionList = _editionRepository.GetAll().ToList();
                return editionList;
            }
            catch (UserFriendlyException exception)
            {
                throw new UserFriendlyException(exception.Message);
            }
        }
    }
}
