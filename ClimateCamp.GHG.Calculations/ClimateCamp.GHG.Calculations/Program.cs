using Calculations.Services.StationaryCombustionCalculation;
using ClimateCamp.EntityFrameworkCore;
using ClimateCamp.GHG.Calculations.DataService;
using ClimateCamp.GHG.Calculations.DataService.Interface;
using ClimateCamp.GHG.Calculations.DataService.Services;
using ClimateCamp.GHG.Calculations.Services.GenericEmissionCalculation;
using ClimateCamp.GHG.Calculations.Services.PathfinderApi;
using ClimateCamp.GHG.Calculations.Services.RollForwardActivityData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mobile.Combustion.Calculation.DataService;
using Mobile.Combustion.Calculation.Services;
using Mobile.Combustion.Calculation.Services.Common;
using Purchased.Energy.Calculation.Services;
using System;

var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices(services =>
                {
                    // calculation service registration
                    services.AddScoped<IMobileCombustionCalculationService, MobileCombustionCalculationService>();
                    services.AddScoped<IPurchasedElectricityCalculationService, PurchasedElectricityCalculationService>();
                    services.AddScoped<IStationaryCombustionCalculationService, StationaryCombustionCalculationService>();
                    services.AddScoped<IGenericEmissionCalculation, GenericEmissionCalculation>();
                    services.AddScoped<IBlobFileService, BlobFileService>();
                    services.AddScoped<IRollForwardActivityData, RollForwardActivityData>();
                    services.AddScoped<IPathfinderApi, PathfinderApiService>();

                    //data access services
                    services.AddScoped<IGreenhouseGasesDataService, GreenhouseGasesDataService>();
                    services.AddScoped<IActivityDataService, ActivityDataService>();
                    services.AddScoped<IQueueMessageService, QueueMessageService>();
                    services.AddScoped<IEmissionDataService, EmissionsDataService>();
                    services.AddScoped<IEmissionsFactorsDataService, EmissionsFactorsDataService>();
                    services.AddScoped<IStationaryCombustionDataService, StationaryCombustionDataService>();
                    services.AddScoped<IPurchasedEnergyDataService, PurchasedEnergyDataService>();
                    services.AddScoped<IEmissionGroupsDataService, EmissionGroupsDataService>();
                    services.AddScoped<IProductEmissionsDataService, ProductEmissionsDataService>();

                    string connectionString = Environment.GetEnvironmentVariable("Default");
                    services.AddDbContext<CommonDbContext>(
                      options => SqlServerDbContextOptionsExtensions.UseSqlServer(options, connectionString, x => x.UseNetTopologySuite()));
                })
                .Build();

             host.Run();
