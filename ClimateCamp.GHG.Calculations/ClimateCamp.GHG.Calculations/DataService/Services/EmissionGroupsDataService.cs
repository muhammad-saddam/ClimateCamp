using ClimateCamp.Application;
using ClimateCamp.CarbonCompute;
using ClimateCamp.EntityFrameworkCore;
using ClimateCamp.GHG.Calculations.DataService.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClimateCamp.GHG.Calculations.DataService.Services
{
    public class EmissionGroupsDataService : IEmissionGroupsDataService
    {
        private readonly CommonDbContext _dbContext;
        private readonly ILogger _logger;

        private double subTotalTons, subTotalKg = 0;

        public EmissionGroupsDataService(CommonDbContext dbContext, ILoggerFactory loggerFactory)
        {
            _dbContext = dbContext;
            _logger = loggerFactory.CreateLogger<EmissionGroupsDataService>();
        }

        public async Task<List<EmissionGroups>> GetEmissionGroupsByOrganizationId(Guid organizationId)
        {
            try
            {
                var parentEmissionGroup = await _dbContext.EmissionGroups
                                    .FirstOrDefaultAsync(x => x.OrganizationId == organizationId && x.ParentEmissionGroupId == null);

                var emissionGroupsList = await _dbContext.EmissionGroups
                        .Where(x => x.OrganizationId == organizationId && x.ParentEmissionGroupId == parentEmissionGroup.Id).ToListAsync();

                return emissionGroupsList;
            }
            catch(Exception exception)
            {
                _logger.LogInformation($"Method: GetEmissionGroupsByOrganizationId - Exception: {exception}");
                return null;
            }
            
        }

        public async Task<List<GroupedEmissionsVM>> GetAllEmissionGroupsAndDataByOrganizationId(Guid organizationId, DateTime consumptionStart, DateTime consumptionEnd)
        //public async Task<List<GroupedEmissionsVM>> GetAllEmissionGroupsAndDataByOrganizationId(Guid organizationId)
        {
            try
            {
                // will not convert the result set into GroupedEmissionsVM class because of Self join
                // were loosing nested data for childs in case of converting the anonymous result set into List<GroupedEmissionsVM> 

                var emissionGroupsDataList = await (from eg in _dbContext.EmissionGroups
                                                    .Include(x => x.Children)
                                                    .Include(x => x.EmissionsSource)
                                                    join ad in _dbContext.ActivityData
                                                    .Where(x => x.ConsumptionStart.Date >= consumptionStart.Date && x.ConsumptionEnd.Date <= consumptionEnd.Date)
                                                    on eg.Id equals ad.EmissionGroupId into egad
                                                    from ad in egad.DefaultIfEmpty()
                                                    join e in _dbContext.Emissions on ad.Id equals e.ActivityDataId into ade
                                                    from e in ade.DefaultIfEmpty()
                                                    where eg.OrganizationId == organizationId
                                                    select new
                                                    {
                                                        EmissionGroupId = eg.Id,
                                                        Icon = eg.Icon,
                                                        Label = eg.Name,
                                                        ParentEmissionGroupId = eg.ParentEmissionGroupId ?? null,
                                                        EmissionSourceId = eg.EmissionSourceId ?? null,
                                                        EmissionSourceName = eg.EmissionsSource != null ? eg.EmissionsSource.Name : null,
                                                        ActivityDataId = ad != null ? ad.Id : Guid.Empty,
                                                        CO2e = e != null ? e.CO2E : null,
                                                        CO2eUnitId = e != null ? e.CO2EUnitId : null,
                                                        Quantity = ad != null ? ad.Quantity : 0,
                                                        QuantityUnitId = ad != null ? ad.UnitId : null,
                                                        ConsumptionStart = ad != null ? ad.ConsumptionStart : default(DateTime),
                                                        ConsumptionEnd = ad != null ? ad.ConsumptionEnd : default(DateTime),
                                                        Children = eg.Children ?? null,
                                                    }
                                         )
                                         .ToListAsync();


                // will use this list to get child nodes emissions data based on emission group id further
                // this dynamic list "emissionGroupsDataList" could be used but will slow down the search due to its dynamic mode
                // basically converting the result set returned from database into solid class instead of anonymous class

                var allEmissionsGroupsData = new List<GroupedEmissionsVM>();

                allEmissionsGroupsData.AddRange(emissionGroupsDataList.GroupBy(x => x.EmissionGroupId, (key, group) => new GroupedEmissionsVM
                {
                    EmissionGroupId = key,
                    Icon = group.First().Icon,
                    Label = group.First().Label,
                    ParentEmissionGroupId = group.First().ParentEmissionGroupId,
                    EmissionSourceId = group.First().EmissionSourceId,
                    EmissionSourceName = group.First().EmissionSourceName,
                    ActivityDataId = group.First().ActivityDataId,
                    CO2e = group.Sum(x => x.CO2e),
                    CO2eUnitId = group.First().CO2eUnitId,
                    Quantity = group.First().Quantity,
                    QuantityUnitId = group.First().QuantityUnitId,
                    ConsumptionStart = group.First().ConsumptionStart,
                    ConsumptionEnd = group.First().ConsumptionEnd
                }));

                //getting onlty the first level parent nodes
                var recursiveEmissionGroupsData = emissionGroupsDataList.Where(x => x.ParentEmissionGroupId == null).ToList()[0].Children;


                //constructing the final list using recursion method
                var emissionGroupsVM = new List<GroupedEmissionsVM>();

                foreach (var emissionGroup in recursiveEmissionGroupsData)
                {
                    var emissionData = allEmissionsGroupsData.Single(x => x.EmissionGroupId == emissionGroup.Id);

                    emissionGroupsVM.Add(new GroupedEmissionsVM
                    {
                        EmissionGroupId = emissionGroup.Id,
                        Icon = emissionGroup.Icon,
                        Label = emissionGroup.Name,
                        ParentEmissionGroupId = emissionGroup.ParentEmissionGroupId,
                        EmissionSourceId = emissionGroup.EmissionSourceId,
                        EmissionSourceName = allEmissionsGroupsData.Single(x => x.EmissionGroupId == emissionGroup.Id).EmissionSourceName,
                        ActivityDataId = emissionData.ActivityDataId,
                        CO2e = emissionData.CO2e,
                        CO2eUnitId = emissionData.CO2eUnitId,
                        Quantity = emissionData.Quantity,
                        QuantityUnitId = emissionData.QuantityUnitId,
                        ConsumptionStart = emissionData.ConsumptionStart,
                        ConsumptionEnd = emissionData.ConsumptionEnd,
                        Children = emissionGroup.Children != null ? AssignChildNodesDataToVMRecursively(emissionGroup.Children.ToList(), allEmissionsGroupsData) : null,
                    });
                }

                //calculating the total emissions = Parent emission + All Nested child emissions again using recursion method

                var kgId = _dbContext.Units.Where(x => x.Name.ToLower() == "kg").FirstOrDefault().Id;
                var tId = _dbContext.Units.Where(x => x.Name.ToLower() == "t").FirstOrDefault().Id;

                emissionGroupsVM.ForEach(emissionGroup =>
                {
                    subTotalTons = 0; subTotalKg = 0;

                    if (emissionGroup.CO2eUnitId == tId)
                        subTotalTons += (double)emissionGroup.CO2e;
                    else if (emissionGroup.CO2eUnitId == kgId)
                        subTotalKg += (double)emissionGroup.CO2e;

                    if (emissionGroup.Children != null)
                        AddEmisisonsForChildGroups(emissionGroup.Children.ToList(), tId, kgId);

                    emissionGroup.TotalEmissions = Math.Round(subTotalTons + subTotalKg / 1000, 5);

                });

                return emissionGroupsVM;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Method: GetAllEmissionGroupsAndDataByOrganizationId - Exception: {ex}");
                return null;
            }
        }

        private List<GroupedEmissionsVM> AssignChildNodesDataToVMRecursively(List<EmissionGroups> emissionGroups, List<GroupedEmissionsVM> emissionGroupsDataList)
        {
            return emissionGroups.Select(emissionGroup => new GroupedEmissionsVM
            {
                EmissionGroupId = emissionGroup.Id,
                Icon = emissionGroup.Icon,
                Label = emissionGroup.Name,
                ParentEmissionGroupId = emissionGroup.ParentEmissionGroupId,
                EmissionSourceId = emissionGroup.EmissionSourceId,
                EmissionSourceName = emissionGroupsDataList.Single(x => x.EmissionGroupId == emissionGroup.Id).EmissionSourceName,
                ActivityDataId = emissionGroupsDataList.Single(x => x.EmissionGroupId == emissionGroup.Id).ActivityDataId,
                CO2e = emissionGroupsDataList.Single(x => x.EmissionGroupId == emissionGroup.Id).CO2e,
                CO2eUnitId = emissionGroupsDataList.Single(x => x.EmissionGroupId == emissionGroup.Id).CO2eUnitId,
                Quantity = emissionGroupsDataList.Single(x => x.EmissionGroupId == emissionGroup.Id).Quantity,
                QuantityUnitId = emissionGroupsDataList.Single(x => x.EmissionGroupId == emissionGroup.Id).QuantityUnitId,
                ConsumptionStart = emissionGroupsDataList.Single(x => x.EmissionGroupId == emissionGroup.Id).ConsumptionStart,
                ConsumptionEnd = emissionGroupsDataList.Single(x => x.EmissionGroupId == emissionGroup.Id).ConsumptionEnd,
                Children = emissionGroup.Children != null ? AssignChildNodesDataToVMRecursively(emissionGroup.Children.ToList(), emissionGroupsDataList) : null
            }).ToList();
        }


        private void AddEmisisonsForChildGroups(List<GroupedEmissionsVM> emissionGroups, int tId, int kgId)
        {
            foreach (var item in emissionGroups)
            {
                if (item.CO2eUnitId == tId)
                    subTotalTons += (double)item.CO2e;
                else if (item.CO2eUnitId == kgId)
                    subTotalKg += (double)item.CO2e;

                if (item.Children != null)
                    AddEmisisonsForChildGroups(item.Children.ToList(), tId, kgId);
            }
        }
    }
}
