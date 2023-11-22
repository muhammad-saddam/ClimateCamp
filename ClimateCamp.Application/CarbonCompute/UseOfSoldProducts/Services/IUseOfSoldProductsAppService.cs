using Abp.Application.Services;
using ClimateCamp.CarbonCompute;
using ClimateCamp.Core.CarbonCompute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimateCamp.Application
{
    public interface IUseOfSoldProductsAppService : IApplicationService
    {
        Task<UseOfSoldProductsData> AddUseOfSoldProductsDataAsync(ActivityDataVM input);
        Task<UseOfSoldProductsData> GetUseOfSoldProductsData();
        Task<UseOfSoldProductsData> UpdateUseOfSoldProductsDataAsync(ActivityDataVM input);
    }
}
