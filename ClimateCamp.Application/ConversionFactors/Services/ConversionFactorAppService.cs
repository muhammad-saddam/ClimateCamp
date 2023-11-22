using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
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
    public class ConversionFactorAppService : AsyncCrudAppService<Core.ConversionFactors, ConversionFactorDto, Guid, PagedConversionFactorsResultDto, CreateConversionFactorDto, CreateConversionFactorDto>, IConversionFactorAppService
    {
        private readonly IRepository<Core.ConversionFactors, Guid> _conversionFactorsRepository;
        private readonly ILogger<ConversionFactorAppService> _logger;

        /// <param name="conversionFactorsRepository"></param>
        /// <param name="logger"></param>
        public ConversionFactorAppService(IRepository<Core.ConversionFactors, Guid> conversionFactorsRepository,
                                         ILogger<ConversionFactorAppService> logger) : base(conversionFactorsRepository)
        {
            _conversionFactorsRepository = conversionFactorsRepository;
            _logger = logger;
        }

        public async Task<bool> SaveConversionFactors(List<CreateConversionFactorDto> conversionFactors)
        {
            try
            {
                foreach (var cFactor in conversionFactors)
                {
                    if (cFactor.Id == Guid.Empty)
                        await base.CreateAsync(cFactor);
                    else
                        await base.UpdateAsync(cFactor);
                }

                return true;
            }
            catch (Exception exception)
            {
                _logger.LogError($"Method: SaveConversionFactors - Exception: {exception}");
                return false;
            }
        }
        
        public async Task<bool> ActivateConversionFactor(Guid? conversionFactorId)
        {
            try {

                // desactivate the previous Conversion Factor
                var conversionFactorActive = await _conversionFactorsRepository.FirstOrDefaultAsync(x => x.IsActive);
                if(conversionFactorActive != null)
                {
                    conversionFactorActive.IsActive = false;
                    await _conversionFactorsRepository.UpdateAsync(conversionFactorActive);
                }

                // if the conversionFactorId is not empty means a conversion factor was seclected 
                if (conversionFactorId.HasValue)
                {
                    var conversionFactorToBeActive = await _conversionFactorsRepository.FirstOrDefaultAsync(x => x.Id == conversionFactorId);
                    // activate the new Conversion Factor
                    if (conversionFactorToBeActive != null)
                    {
                        conversionFactorToBeActive.IsActive = true;
                        await _conversionFactorsRepository.UpdateAsync(conversionFactorToBeActive);
                    }
                }

                return true;
            }
            catch (Exception exception)
            {
                _logger.LogError($"Method: ActivateConversionFactor - Exception: {exception}");
                return false;
            }
        }

        public async Task<PagedResultDto<ConversionFactorDto>> GetAllConversionFactorsByProductId(Guid productId)
        {
            try
            {
                var conversionFactors = await _conversionFactorsRepository.GetAll().Where(x => x.ProductId == productId).ToListAsync();

                var result = new PagedResultDto<ConversionFactorDto>()
                {
                    Items = ObjectMapper.Map<List<ConversionFactorDto>>(conversionFactors),
                    TotalCount = conversionFactors.Count
                };

                return result;
            }
            catch (Exception exception)
            {
                _logger.LogError($"Method: GetAllConversionFactorsByProductId - Exception: {exception}");
                return new PagedResultDto<ConversionFactorDto>();
            }
        }


        public async Task<PagedResultDto<ConversionFactorDto>> GetAllConversionFactorsByActivityDataId(Guid activityDataId)
        {
            try
            {
                var conversionFactors = await _conversionFactorsRepository.GetAll().Where(x => x.ActivityDataId == activityDataId).ToListAsync();

                var result = new PagedResultDto<ConversionFactorDto>()
                {
                    Items = ObjectMapper.Map<List<ConversionFactorDto>>(conversionFactors),
                    TotalCount = conversionFactors.Count
                };

                return result;
            }
            catch (Exception exception)
            {
                _logger.LogError($"Method: GetAllConversionFactorsByActivityDataId - Exception: {exception}");
                return new PagedResultDto<ConversionFactorDto>();
            }
        }
    }
}
