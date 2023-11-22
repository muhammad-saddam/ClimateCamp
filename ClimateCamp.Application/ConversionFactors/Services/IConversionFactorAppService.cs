using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClimateCamp.Application
{
    public interface IConversionFactorAppService
    {
        Task<PagedResultDto<ConversionFactorDto>> GetAllConversionFactorsByProductId(Guid productId);
        Task<bool> SaveConversionFactors(List<CreateConversionFactorDto> conversionFactors);
    }
}
