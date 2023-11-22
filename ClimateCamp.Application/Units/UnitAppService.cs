using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using ClimateCamp.CarbonCompute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClimateCamp.Application
{
    [AbpAuthorize]
    public class UnitAppService : AsyncCrudAppService<Unit, UnitDto, int, PagedUnitResponseDto, CreateUnitDto, CreateUnitDto>, IUnitAppService
    {
        private readonly IRepository<Unit, int> _unitRepository;
        public UnitAppService(
            IRepository<Unit, int> unitRepository) : base(unitRepository)
        {
            _unitRepository = unitRepository;
        }



        public  async Task<List<UnitGroup>> GetGroupedUnits()
        {
            List<UnitGroup> GroupedUnitsList = new List<UnitGroup>();

            List<GHG.UnitGroup> unitGroupList =  Enum.GetValues(typeof(GHG.UnitGroup)).Cast<GHG.UnitGroup>().ToList();
            List<Unit> unitList = _unitRepository.GetAll().ToList();

            foreach (var unitGroup in unitGroupList)
            {
                UnitGroup model = new UnitGroup();
                model.label = unitGroup.ToString();
                model.value = (int)unitGroup;
                model.items = unitList.Any(x => x.Group == (int)unitGroup) ?
                              unitList.Where(x => x.Group == (int)unitGroup)
                              .Select(x => new Item { label = x.Name, value = x.Id })
                              .ToList() : new List<Item>();
                if (model.items.Count > 0)
                {
                    GroupedUnitsList.Add(model);
                }
                
            }

            return GroupedUnitsList;
        }

    }
}
