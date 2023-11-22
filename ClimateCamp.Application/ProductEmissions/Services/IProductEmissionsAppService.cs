using System;
using System.Threading.Tasks;

namespace ClimateCamp.Application
{
    public interface IProductEmissionsAppService
    {
        Task<ProductEmissionsDto> SaveProductEmissions(CreateProductEmissionsDto createProductEmissionsDto);
        Task<ProductEmissionsDto> GetProductEmission(Guid productId, int periodType, int period, int year);
        Task<ProductEmissionsVM> GetCurrentOrPreviousProductEmissions(Guid productId, int periodType, int period, int year);
    }
}
