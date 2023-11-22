using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using ClimateCamp.CarbonCompute;
using ClimateCamp.Core;
using ClimateCamp.Core.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using static ClimateCamp.Core.OrganizationUnit;

namespace ClimateCamp.Application
{
    public class EmissionsAppService : AsyncCrudAppService<Emission, EmissionsDto, Guid, EmissionsResponseDto, CreateEmissionsDto, CreateEmissionsDto>, IEmissionsAppService
    {
        private readonly IRepository<Emission, Guid> _emissionRepository;
        private readonly IRepository<OrganizationUnit, Guid> _organizationUnitRepository;
        private readonly IRepository<EmissionsFactorsLibrary, Guid> _emissionsFactorsLibraryRepository;
        private readonly IRepository<ActivityType, int> _activityTypeRepository;
        private readonly IRepository<EmissionGroups, Guid> _emissionsGroupRepository;
        private readonly IRepository<ActivityData, Guid> _activityRepository;
        private readonly IRepository<EmissionsSource, int> _emissionsSourceRepository;
        private readonly IRepository<Unit, int> _unitRepository;
        private readonly IRepository<Core.EmissionsSummary, Guid> _emissionsSummaryRepository;

        private double subTotalTons,subTotalKg = 0;

        private readonly ILogger<EmissionsAppService> _logger;

        /// <param name="emissionRepository"></param>
        /// <param name="organizationUnitRepository"></param>
        /// <param name="emissionsFactorsLibraryRepository"></param>        
        /// <param name="logger"></param>
        /// <param name="activityTypeRepository"></param>
        /// <param name="activityRepository"></param>
        /// <param name="emissionsGroupRepository"></param>
        /// <param name="emissionsSourceRepository"></param>
        /// <param name="unitRepository"></param> 
        /// <param name="emissionsSummaryRepository"></param>
        public EmissionsAppService(
            IRepository<Emission, Guid> emissionRepository,
            IRepository<OrganizationUnit, Guid> organizationUnitRepository,
            IRepository<EmissionsFactorsLibrary, Guid> emissionsFactorsLibraryRepository,
            ILogger<EmissionsAppService> logger,
            IRepository<ActivityType, int> activityTypeRepository,
            IRepository<EmissionGroups, Guid> emissionsGroupRepository,
            IRepository<ActivityData, Guid> activityRepository,
            IRepository<EmissionsSource, int> emissionsSourceRepository,
            IRepository<Unit, int> unitRepository,
            IRepository<Core.EmissionsSummary, Guid> emissionsSummaryRepository) : base(emissionRepository)

        {
            _emissionRepository = emissionRepository;
            _organizationUnitRepository = organizationUnitRepository;
            _emissionsFactorsLibraryRepository = emissionsFactorsLibraryRepository;
            _logger = logger;
            _activityTypeRepository = activityTypeRepository;
            _emissionsGroupRepository = emissionsGroupRepository;
            _activityRepository = activityRepository;
            _emissionsSourceRepository = emissionsSourceRepository;
            _unitRepository = unitRepository;
            _emissionsSummaryRepository = emissionsSummaryRepository;
        }
        public async Task<PagedResultDto<EmissionsReportCSVModel>> GetEmissionsByOrganizationId(EmissionsReportFilterModel input)
        {
            try
            {
                List<Guid> organizationUnitIds;

                var activity = _activityTypeRepository.GetAll().ToList();
                if (input.OrganizationId == Guid.Empty)
                    organizationUnitIds = _organizationUnitRepository.GetAll().Select(x => x.Id).ToList(); //All OUs for all Organizations
                else
                    organizationUnitIds = _organizationUnitRepository.GetAll().Where(org => org.OrganizationId == input.OrganizationId).Select(x => x.Id).ToList();
                List<EmissionsReportCSVModel> emissionList = new List<EmissionsReportCSVModel>();
                foreach (var organizationUnit in organizationUnitIds)
                {
                    var emissionsQuery = _emissionRepository.GetAll()
                      .Include(t => t.ActivityData)
                      .Include(t => t.ActivityData.ActivityType)
                      //.ThenInclude(t => t.EmissionsSource)
                      .Include(t => t.ActivityData.Unit)
                      .Include(t => t.OrganizationUnit).ThenInclude(t => t.Country)
                      .Include(t => t.CO2EUnit)
                      .Include(t => t.EmissionsFactorsLibrary)
                    .Where(t => t.OrganizationUnitId == organizationUnit && t.ActivityData.Status != (int)Core.CarbonCompute.Enum.ActivityDataStatus.Draft);

                    if (input.From != null)
                    {
                        emissionsQuery = emissionsQuery.Where(t => t.ActivityData.TransactionDate >= input.From);
                    }

                    if (input.To != null)
                    {
                        emissionsQuery = emissionsQuery.Where(t => t.ActivityData.TransactionDate <= input.To);
                    }

                    if (input.EmissionsSourceId != null)
                    {
                        emissionsQuery = emissionsQuery.Where(t => t.ActivityData.ActivityType.EmissionsSourceId == input.EmissionsSourceId);
                    }

                    var emissions = emissionsQuery.ToList();

                    foreach (var emission in emissions)
                    {

                        // var emissionLibrary = _emissionsFactorsLibraryRepository.Get(emission.EmissionsFactorsLibraryId);
                        var model = new EmissionsReportCSVModel()
                        {
                            OrganizationUnit = emission.OrganizationUnit?.Name,
                            OrganizationUnitType = Enum.GetName(typeof(OrganizationUnitType), emission.OrganizationUnit?.Type),
                            Country = emission.OrganizationUnit?.Country?.Name,
                            CO2E = Math.Round((double)emission.CO2E, 2),
                            Unit = emission.CO2EUnit.Name,
                            ActivityDataType = emission?.ActivityData?.ActivityType?.Name,
                            EmissionScope = emission?.ActivityData?.ActivityType?.EmissionsSource?.EmissionScope.ToString(),
                            EmissionSource = emission?.ActivityData?.ActivityType?.EmissionsSource?.Name,
                            Quantity = emission.ActivityData?.Quantity,
                            QuantityUnit = emission.ActivityData?.Unit?.Name,
                            TransactionDate = emission.ActivityData?.TransactionDate.ToString(),
                            TransactionSource = emission.ActivityData?.TransactionId,
                            ActivityName = emission.ActivityData?.Name,
                            // ActivityDescription = emission.ActivityData?.Description.Replace(",", ""),
                            EmissionFactorLibrary = emission?.EmissionsFactorsLibrary.Name,
                            EmissionFactorLibraryYear = emission?.EmissionsFactorsLibrary.Year.ToString()
                        };

                        emissionList.Add(model);
                    }
                }

                var result = new PagedResultDto<EmissionsReportCSVModel>()
                {
                    Items = ObjectMapper.Map<List<EmissionsReportCSVModel>>(emissionList),
                    TotalCount = emissionList.Count
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Method: GetEmissionsByOrganizationId - Exception: {ex}");
                return null;
            }

        }

        /// <summary>
        /// Returns all EmissionGroups along with activity &amp; emissions data. <br/>
        /// It sets NULL if there are no correlating entries in the joined tables.
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="ConsumptionEnd"></param>
        /// <param name="ConsumptionStart"></param>
        /// <param name="year"></param>
        /// <param name="perUnitVolumeProduction"></param>
        /// <returns></returns>
        public async Task<List<GroupedEmissionsVM>> GetAllEmissionGroupsAndDataByOrganizationId(Guid organizationId, DateTime ConsumptionStart, DateTime ConsumptionEnd, int year, bool perUnitVolumeProduction)
        {
            try
            {
                float? sumProductionQuantity = 0;
                int productionQunatityUnit = 0, kgId = 0, tId = 0;
                string productionQuantityUnitName = string.Empty;

                if(perUnitVolumeProduction)
                {

                    //Get All Organization Units in heirarchical order for the organization
                    var organizationUnitsList = await _organizationUnitRepository.GetAll()
                                            .Where(x => x.OrganizationId == organizationId)
                                            .Select(x => new { x.Id, x.ParentOrganizationUnitId })
                                            .ToListAsync();

                    //Get the top level or parent organization unit
                    var parentOrganizationUnit = organizationUnitsList.First(x => x.ParentOrganizationUnitId == null);

                    //Check if top level organization unit data exists against selected period
                    var parentOrganizationUnitData = await _emissionsSummaryRepository.GetAll()
                                             .Where(x => x.OrganizationUnitId == parentOrganizationUnit.Id && x.Period == year)
                                             .ToListAsync();

                    if (parentOrganizationUnitData.Any())
                    {
                        sumProductionQuantity = parentOrganizationUnitData.Sum(x => x.ProductionQuantity);

                        productionQunatityUnit = parentOrganizationUnitData.First().ProductionMetricId;

                        productionQuantityUnitName = _unitRepository.GetAll().Where(x => x.Id == productionQunatityUnit).Single().Name;
                    }
                    
                    //If production quantity does not exists for the top level organization unit against selected period
                    //then get the production quantity against all the child level organization units against the selected period
                    if (sumProductionQuantity == 0)
                    {

                        var childOrganizationUnits = organizationUnitsList
                                                    .Where(x => x.ParentOrganizationUnitId != null)
                                                    .Select(x => x.Id)
                                                    .ToList();

                        sumProductionQuantity = await _emissionsSummaryRepository.GetAll()
                                             .Where(x => childOrganizationUnits.Contains(x.OrganizationUnitId) && x.Period == year)
                                             .SumAsync(x => x.ProductionQuantity);

                    }
                   
                }

                // will not convert the result set into GroupedEmissionsVM class because of Self join
                // were loosing nested data for childs in case of converting the anonymous result set into List<GroupedEmissionsVM> 

                var emissionGroupsDataList = await (from eg in _emissionsGroupRepository.GetAll()
                                                    .Include(x => x.Children)
                                                    .Include(x => x.EmissionsSource)
                                                    join ad in _activityRepository.GetAll()
                                                    .Where(x => x.ConsumptionStart.Date >= ConsumptionStart.Date && x.ConsumptionEnd.Date <= ConsumptionEnd.Date && x.Status != (int)Core.CarbonCompute.Enum.ActivityDataStatus.Draft)
                                                    on eg.Id equals ad.EmissionGroupId into egad
                                                    from ad in egad.DefaultIfEmpty()

                                                    let maxEmission = _emissionRepository.GetAll()
                                                    .Where(emission => emission.ActivityDataId == ad.Id)
                                                    .Max(m => m.Version)
                                                    
                                                    join e in _emissionRepository.GetAll()
                                                    on ad.Id equals e.ActivityDataId into ade
                                                    from e in ade.DefaultIfEmpty()
                                                    where eg.OrganizationId == organizationId && e.Version == maxEmission
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
                    CO2e = group.Sum(x => x.CO2e),
                    ActivityDataId = group.First().ActivityDataId,
                    // need to fine tune these properties to use item approach per group
                    CO2eUnitId = group.Any(x => x.CO2eUnitId != null) ? group.Where(x => x.CO2eUnitId != null).First().CO2eUnitId : null,
                    Quantity = group.First().Quantity, // need to check if we are using these properties
                    QuantityUnitId = group.First().QuantityUnitId, // need to check if we are using these properties
                    ConsumptionStart = group.First().ConsumptionStart, // need to check if we are using these properties
                    ConsumptionEnd = group.First().ConsumptionEnd // need to check if we are using these properties
                }));

                //getting only the first level parent nodes
                var recursiveEmissionGroupsData = emissionGroupsDataList.Where(x => x.ParentEmissionGroupId == null).ToList()[0].Children.OrderBy(x => x.Name);

                 kgId = _unitRepository.GetAll().Where(x => x.Name.ToLower() == "kg").Single().Id;
                 tId = _unitRepository.GetAll().Where(x => x.Name.ToLower() == "t").Single().Id;

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
                        CO2e = perUnitVolumeProduction ? GetPerUnitCO2e(emissionData, sumProductionQuantity, productionQunatityUnit, kgId, tId) : emissionData.CO2e,
                        CO2eUnitId = emissionData.CO2eUnitId,
                        Quantity = emissionData.Quantity,
                        QuantityUnitId = emissionData.QuantityUnitId,
                        ConsumptionStart = emissionData.ConsumptionStart,
                        ConsumptionEnd = emissionData.ConsumptionEnd,
                        ProductionQuantityUnit = productionQuantityUnitName,
                        Children = emissionGroup.Children != null ? AssignChildNodesDataToVMRecursively(emissionGroup.Children.ToList(), allEmissionsGroupsData, perUnitVolumeProduction, sumProductionQuantity, productionQunatityUnit, productionQuantityUnitName, kgId, tId) : null,
                    });
                }

                //calculating the total emissions = Parent emission + All Nested child emissions again using recursion method

                emissionGroupsVM.ForEach(emissionGroup =>
                {
                    subTotalTons = 0;subTotalKg = 0;

                    if (emissionGroup.CO2eUnitId == tId)
                        subTotalTons += (double)emissionGroup.CO2e;
                    else if (emissionGroup.CO2eUnitId == kgId)
                        subTotalKg += (double)emissionGroup.CO2e;

                    if (emissionGroup.Children != null)
                        AddEmisisonsForChildGroups(emissionGroup.Children.ToList(),tId,kgId);

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

        private static float? GetPerUnitCO2e(GroupedEmissionsVM emissionData, float? sumProductionQuantity, int productionQuantityUnit, int kgId, int tId)
        {
            if (emissionData.CO2e == 0 || emissionData.CO2e == null)
            {
                return 0;
            }

            if (sumProductionQuantity == 0)
            {
                return emissionData.CO2e;
            }

            float? convertedProductionQuantity = 0;

            if (emissionData.CO2eUnitId == productionQuantityUnit)
            {
                convertedProductionQuantity = sumProductionQuantity;
            }
            else
            {
                if (emissionData.CO2eUnitId == kgId && productionQuantityUnit == tId)
                {
                    convertedProductionQuantity = sumProductionQuantity * 1000;
                }
                else if (emissionData.CO2eUnitId == tId && productionQuantityUnit == kgId)
                {
                    convertedProductionQuantity = sumProductionQuantity / 1000;
                }
                else
                {
                    convertedProductionQuantity = sumProductionQuantity;
                }
            }

            return emissionData.CO2e / convertedProductionQuantity;

        }

        private List<GroupedEmissionsVM> AssignChildNodesDataToVMRecursively(List<EmissionGroups> emissionGroups, List<GroupedEmissionsVM> emissionGroupsDataList, bool perUnitVolumeProduction, float? sumProductionQuantity, int productionQuantityUnit,string productionQuantityUnitName, int kgId, int tId)
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
                CO2e = perUnitVolumeProduction ? GetPerUnitCO2e(emissionGroupsDataList.Single(x => x.EmissionGroupId == emissionGroup.Id), sumProductionQuantity, productionQuantityUnit, kgId, tId)  : emissionGroupsDataList.Single(x => x.EmissionGroupId == emissionGroup.Id).CO2e,
                CO2eUnitId = emissionGroupsDataList.Single(x => x.EmissionGroupId == emissionGroup.Id).CO2eUnitId,
                Quantity = emissionGroupsDataList.Single(x => x.EmissionGroupId == emissionGroup.Id).Quantity,
                QuantityUnitId = emissionGroupsDataList.Single(x => x.EmissionGroupId == emissionGroup.Id).QuantityUnitId,
                ConsumptionStart = emissionGroupsDataList.Single(x => x.EmissionGroupId == emissionGroup.Id).ConsumptionStart,
                ConsumptionEnd = emissionGroupsDataList.Single(x => x.EmissionGroupId == emissionGroup.Id).ConsumptionEnd,
                ProductionQuantityUnit = productionQuantityUnitName,
                Children = emissionGroup.Children != null ? AssignChildNodesDataToVMRecursively(emissionGroup.Children.ToList(), emissionGroupsDataList, perUnitVolumeProduction, sumProductionQuantity, productionQuantityUnit, productionQuantityUnitName, kgId, tId) : null
            }).ToList();
        }


        private void AddEmisisonsForChildGroups(List<GroupedEmissionsVM> emissionGroups,int tId,int kgId)
        {
            foreach (var item in emissionGroups)
            {
                if (item.CO2eUnitId == tId)
                    subTotalTons += (double)item.CO2e;
                else if (item.CO2eUnitId == kgId)
                    subTotalKg += (double)item.CO2e;

                if (item.Children != null)
                    AddEmisisonsForChildGroups(item.Children.ToList(),tId,kgId);
            }
        }
    }
}