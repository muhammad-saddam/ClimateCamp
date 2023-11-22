using Abp.Application.Services;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClimateCamp.Application
{
    public interface IUnitAppService: IApplicationService
    {
        Task<List<UnitGroup>> GetGroupedUnits();
    }

}
