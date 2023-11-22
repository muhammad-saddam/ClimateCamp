using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using System;
using System.Threading.Tasks;

namespace ClimateCamp.Application
{
    [AbpAuthorize]
    // AsyncCrudAppService<Organization, OrganizationDto, Guid, PagedOrganizationResultRequestDto, CreateOrganizationDto, CreateOrganizationDto>, IOrganizationAppService
    public class ReductionAppService : AsyncCrudAppService<ClimateCamp.Core.Reduction, ReductionDto, Guid, PagedReductionResponseDto, CreateReductionDto, CreateReductionDto>, IDataCollectionAppService
    {
        private readonly IRepository<ClimateCamp.Core.Reduction, Guid> _reductionRepository;

        public ReductionAppService(
            IRepository<ClimateCamp.Core.Reduction, Guid> reductionRepository) : base(reductionRepository)
        {
            _reductionRepository = reductionRepository;
        }

        public async Task<bool> setIsFavourite(CreateReductionDto input)
        {
            try
            {
                var reduction = await _reductionRepository.GetAsync(input.Id);
                reduction.isFavourite = input.isFavourite;
                await _reductionRepository.UpdateAsync(reduction);
                return true;
            }

            catch (Exception ex)
            {
                return false;
            }

        }
    }
}
