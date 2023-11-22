using Abp.Zero.EntityFrameworkCore;
using ClimateCamp.CarbonCompute;
using ClimateCamp.Common.Authorization.Roles;
using ClimateCamp.Common.Authorization.Users;
using ClimateCamp.Common.MultiTenancy;
using ClimateCamp.Core;
using ClimateCamp.Core.CarbonCompute;
using ClimateCamp.Core.Editions;
using ClimateCamp.Core.Features;
using ClimateCamp.Lookup;
using Microsoft.EntityFrameworkCore;

namespace ClimateCamp.EntityFrameworkCore
{
    public class CommonDbContext : AbpZeroDbContext<Tenant, Role, User, CommonDbContext>
    {
        public DbSet<Organization> Organizations { get; set; }

        //TODO: this class has the OrganizationUnits property but also the is already contianed in the AbpZeroDbContext base class. Base class is of the same type, so removing this line shoudl be enough
        //Created the Error CS0029  Cannot implicitly convert type 'Abp.Organizations.OrganizationUnit' to 'ClimateCamp.Core.OrganizationUnit'	ClimateCamp.EntityFrameworkCore C:\Users\vitalie\source\repos\Climate-Camp-Portal\ClimateCamp.EntityFrameworkCore\EntityFrameworkCore\Seed\Host\CarbonCompute\DefaultEmissionsFactorsCreator.cs	53	Active

        public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
        public DbSet<DataCollection> DataCollections { get; set; }
        public DbSet<VehicleType> VehicleTypes { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<IndustrialProcess> IndustrialProcesses { get; set; }
        public DbSet<GreenhouseGas> GreenHouseGases { get; set; }
        public DbSet<FuelType> FuelTypes { get; set; }
        public DbSet<EmissionsSource> EmissionsSources { get; set; }
        public DbSet<EmissionsFactorsLibrary> EmissionsFactorsLibrary { get; set; }
        public DbSet<EmissionsFactor> EmissionsFactors { get; set; }
        public DbSet<ActivityType> ActivityTypes { get; set; }
        public DbSet<Emission> Emissions { get; set; }
        public DbSet<ActivityData> ActivityData { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Reduction> Reductions { get; set; }
        public DbSet<Offset> Offsets { get; set; }
        public DbSet<MobileCombustionData> MobileCombustionData { get; set; }
        public DbSet<PurchasedEnergyData> PurchasedEnergyData { get; set; }
        public DbSet<StationaryCombustionData> StationaryCombustionData { get; set; }

        public DbSet<LifeCycleActivity> LifeCycleActivity { get; set; }

        public DbSet<Sector> Sector { get; set; }

        //public DbSet<ContractualInstrument> ContractualInstruments { get; set; }
        public DbSet<CustomEdition> CustomEditions { get; set; }

        public DbSet<Industry> Industries { get; set; }
        public DbSet<OrganizationIndustry> OrganizationIndustries { get; set; }

        public DbSet<CustomerSupplier> CustomerSuppliers { get; set; }

        public DbSet<EditionFeatureSettingCustom> EditionFeatureSettingCustom { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<PurchasedProductsData> PurchaseProductsData { get; set; }
        public DbSet<TransportAndDistributionData> TransportAndDistributionData { get; set; }
        public DbSet<FugitiveEmissionsData> FugitiveEmissionsData { get; set; }
        public DbSet<EmissionGroups> EmissionGroups { get; set; }
        public DbSet<EmployeeCommuteData> EmployeeCommuteData { get; set; }
        public DbSet<BusinessTravelData> BusinessTravelData { get; set; }
        public DbSet<WasteGeneratedData> WasteGeneratedData { get; set; }
        public DbSet<EndOfLifeTreatmentData> EndOfLifeTreatmentData { get; set; }
        public DbSet<EmissionsSummary> EmissionsSummary { get; set; }
        public DbSet<EmissionsSummaryScopeDetails> EmissionsSummaryScopeDetails { get; set; }
        public DbSet<ProductGroups> ProductGroups { get; set; }
        public DbSet<ConversionFactors> ConversionFactors { get; set; }
        public DbSet<ProductsEmissionSources> ProductsEmissionSources { get; set; }
        public DbSet<ProductEmissions> ProductsEmissions { get; set; }
        public DbSet<UseOfSoldProductsData> UseOfSoldProducts { get; set; }
        public DbSet<OrganizationTarget> OrganizationTargets { get; set; }
        public DbSet<TargetIndependant> TargetIndependant { get; set; }
        public DbSet<ScienceBasedTarget> ScienceBasedTargets { get; set; }
        public DbSet<FuelAndEnergyData> FuelAndEnergy { get; set; }
        public DbSet<CustomerProduct> CustomerProducts { get; set; }
        public DbSet<CarbonFootprints> CarbonFootprints { get; set; }
        public CommonDbContext(DbContextOptions<CommonDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<OrganizationIndustry>().HasKey(oi => new { oi.OrganizationId, oi.IndustryId });
            modelBuilder.Entity<EmissionsFactor>().HasOne(e => e.EmissionsSource).WithMany();
            //TODO: Confirm is to use a complex primary key, or keep simpler with a long Id
            // modelBuilder.Entity<Organization>().HasMany(x => x.CustomerOrganizations).WithOne(x => x.CustomerOrganization);
            modelBuilder.Entity<Organization>().HasMany(x => x.SupplierOrganizations).WithOne(x => x.SupplierOrganization).OnDelete(DeleteBehavior.NoAction);
            //builder.Entity<User>().Ignore(x => x.Surname);

            modelBuilder.Entity<ProductsEmissionSources>()
                .HasKey(pes => pes.Id);
                 modelBuilder.Entity<ProductsEmissionSources>()
                .HasOne(es => es.EmissionsSource)
                .WithMany(pes => pes.ProductsEmissionSources)
                .HasForeignKey(es => es.EmissionsSourceId);

            modelBuilder.Entity<ProductEmissions>()
                .HasOne(x => x.CarbonFootprint)
                .WithOne(x => x.ProductEmission)
                .HasForeignKey<CarbonFootprints>(x => x.Id);

        }
    }
}
