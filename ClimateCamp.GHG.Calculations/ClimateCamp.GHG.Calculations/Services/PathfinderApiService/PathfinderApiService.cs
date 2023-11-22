using ClimateCamp.GHG.Calculations.DataService;
using ClimateCamp.GHG.Calculations.Pathfinder;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ClimateCamp.GHG.Calculations.Services.PathfinderApi
{
    public class PathfinderApiService : IPathfinderApi
    {
        private readonly IProductEmissionsDataService _productEmissionsDataService;
        private readonly ILogger _logger;

        public PathfinderApiService(
            IProductEmissionsDataService productEmissionsDataService,
            ILoggerFactory loggerFactory)
        {
            _productEmissionsDataService = productEmissionsDataService;
            _logger = loggerFactory.CreateLogger<PathfinderApiService>();
        }


        public async Task<ProductFootprintResponse> CreatePathfinderPcfObject(Guid productId)
        {
            try
            {
                var pcfData = await _productEmissionsDataService.GetProductFootprintData(productId);
                if (pcfData != null)
                {
                    var productFootprint = new ProductFootprint
                    {
                        Id = pcfData.Id,
                        SpecVersion = "Spec Version - string - MANDATORY",
                        Version = 0,
                        Created = DateTime.Now,
                        Updated = DateTime.Now,
                        CompanyName = pcfData.Product.Organization.Name,
                        CompanyIds = new CompanyIdSet(),
                        ProductDescription = pcfData.Product.Description,
                        ProductIds = new ProductIdSet(),
                        ProductCategoryCpc = "ProductCategoryCpc - string - optional",
                        ProductNameCompany = $"{pcfData.Product.Name} ({pcfData.Product.Organization.Name})",
                        Comment = "info related to the calc of the footprint - string - MANDATORY",
                        Pcf = new CarbonFootprint
                        {
                            DeclaredUnit = DeclaredUnit.LiterEnum,
                            UnitaryProductAmount = "Unitary Product Amount - string",
                            FossilGhgEmissions = "Fossil GHG Emissions - string",
                            BiogenicEmissions = new AllOfCarbonFootprintBiogenicEmissions
                            {
                                LandUseEmissions = new AllOfBiogenicEmissionsLandUseEmissions(),
                                LandUseChangeEmissions = new AllOfBiogenicEmissionsLandUseChangeEmissions(),
                                OtherEmissions = new AllOfBiogenicEmissionsOtherEmissions(),
                            },
                            BiogenicCarbonContent = "Biogenic Carbon Content - string - MANDATORY",
                            ReferencePeriodStart = new DateTime(Convert.ToInt32(pcfData.Year), 1, 1),
                            ReferencePeriodEnd = new DateTime(Convert.ToInt32(pcfData.Year), 12, 31),
                            PrimaryDataShare = 0.0,
                            EmissionFactorSources = new AllOfCarbonFootprintEmissionFactorSources(),
                            BoundaryProcessesDescription = "Boundary Processes Description - string - optional",
                            CrossSectoralStandardsUsed = new CrossSectoralStandardSet(),
                            ProductOrSectorSpecificRules = new ProductOrSectorSpecificRuleSet
                            {
                                new ProductOrSectorSpecificRuleSetInner
                                {
                                    _Operator = ProductOrSectorSpecificRuleOperator.PEFEnum,
                                    RuleNames = new NonEmptyStringVec
                                    {
                                        "String"
                                    },
                                    OtherOperatorName = new AllOfProductOrSectorSpecificRuleSetInnerOtherOperatorName()
                                }
                            },
                            AllocationRulesDescription = "Allocation Rules Description - string"
                        }
                    };

                    var productFootprintResponse = new ProductFootprintResponse
                    {
                        Data = productFootprint
                    };

                    return productFootprintResponse;
                }
                else
                {
                    return default;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Method: GetProductPcf - Exception: {ex}");
                return default;
            }
        }

    }
}
