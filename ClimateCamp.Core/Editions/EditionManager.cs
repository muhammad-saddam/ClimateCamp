using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;

namespace ClimateCamp.Core.Editions
{
    public class EditionManager : AbpEditionManager
    {
        public const string DefaultEditionName = "Enterprise +";

        public EditionManager(
            IRepository<Edition> editionRepository,
            IAbpZeroFeatureValueStore featureValueStore,
            IUnitOfWorkManager unitOfWorkManager)
            : base(editionRepository, featureValueStore, unitOfWorkManager)
        {
        }
    }
}
