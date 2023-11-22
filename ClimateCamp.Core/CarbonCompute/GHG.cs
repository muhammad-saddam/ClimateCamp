

namespace ClimateCamp.CarbonCompute
{
    public static class GHG
    {
        public enum GreenhouseGasCategory
        {
            NonFluorinated = 0,
            /// <summary>
            /// Nitrogen Trifluoride (NF3)
            /// </summary>
            NF3 = 1,
            /// <summary>
            /// Sulfur hexafluoride (SF6)
            /// </summary>
            SF6 = 2,
            /// <summary>
            /// Hydrofluorocarbons (HFC)
            /// </summary>
            HFC = 3,
            /// <summary>
            /// Perfluorocarbons (PFC)
            /// </summary>
            PFC = 4,
            /// <summary>
            /// Chlorofluorocarbons (CFC)
            /// </summary>
            CFC = 5,
            /// <summary>
            /// Hydrofluoroolefins (HFO)
            /// </summary>
            HFO = 6,
            /// <summary>
            /// Hydrochlorofluorocarbons (HCFC)
            /// </summary>
            HCFC = 7,
            /// <summary>
            /// Fluorocarbons (FC)
            /// </summary>
            FC = 8,
            /// <summary>
            /// Halocarbons (HC)
            /// </summary>
            HC = 9,
            /// <summary>
            /// Hydrochlorofluoroolefins (HCFO)
            /// </summary>
            HCFO = 10,
            /// <summary>
            /// Blend of Hydrofluorocarbons (HFC) and Hydrochlorofluorocarbons (HFO)
            /// </summary>
            HFC_HFO = 11,
        }

        public enum DataQualityType
        {
            Unknown = 0,
            Estimated = 1,
            Actual = 2,
            Metered = 3
        }

        public enum EmissionsDataQualityScore
        {
            Unknown = 0,
            Estimated = 1,
            ProxyData = 2,
            Averaged = 3,
            NonAudited = 4,
            Audited = 5
        }

        /// <summary>
        /// To represent the "entity" responsible for the generated emissions: Organization unit, manufactured product or an activity/industry process.
        /// Used as a helper to categorize emissions in <see cref="Emission.ResponsibleEntityType"/>
        /// </summary>
        public enum ResponsibleEntityTypes
        {
            User = 0,
            Product = 1,
        }

        public enum EmissionScope
        {
            Unknown = 0,
            Scope1 = 1,
            Scope2 = 2,
            Scope3 = 3
        }

        public enum EnergyType
        {
            Unknown = 0,
            Electricity = 1,
            Steam = 2,
            Heating = 3,
            Cooling = 4
        }

        public enum EnergySource
        {
            Other = 0,
            Solar = 1,
            Wind = 2,
            Nuclear = 3,
            Water = 4,
            Coal = 5,
            Gas = 6,
            Mix = 7,
            MultipleRenewable = 8
        }

        public enum ModeOfTransport
        {
            Unknown = 0,
            Air = 1,
            Rail = 2,
            River = 3,
            Road = 4,
            Maritime = 5,
            PublicTransportation = 6,
            Bike = 7,
            Car = 8,
            Motorcycle = 9
        }

        public enum TransportationKind
        {
            Freight = 0,
            Travel = 1
        }

        /// <summary>
        /// Energy: kWh, TJ, GJ or MMBTU
        /// Money: usd, eur or gbp
        /// PassengerOverDistance: passengers / Distance
        /// Weight: kg,t
        /// Time: day, h, m, s or 
        /// WeightOverDistance: kg, t / Distance
        /// Example categories at https://www.climatiq.io/docs#estimation-unit-types
        /// </summary>
        public enum UnitGroup
        {
            Currency = 0,
            Volume = 1,
            Distance = 2,
            Weight = 3,
            Energy = 4,
            PassengerOverDistance = 5,
            WeightOverDistance = 6,
            Number = 7,
            VehicleOverDistance = 8,
        }

        public enum GreenhouseGasesEnum
        {
            CarbonDioxide = 1,
            Methane,
            NitrousOxide,
        }

        public enum GreenhouseGasesCodeEnum
        {
            CO2 = 1,
            CH4 = 2,
            N2O = 3,
        }

        /// <summary>
        /// Developent helper Enum for strong typing the identifiers of commonly used unit units.
        /// The full invetory is represented through the domain entity type <see cref="Unit"/>
        /// </summary>
        public enum UnitsEnum
        {
            kg = 1,
            t = 2,
            km = 3,
            miles = 4,
            EUR = 5,
            lb = 6,
            L = 7,
            kWh = 8,
            m3 = 9,
            Units = 10,
            Dozen = 11,
            MWh = 12,
            Mt = 13,
            hL = 14,
            TKM = 15,
            PKM = 16,
            VKM = 17,
            g = 18
        }

        /// <summary>
        /// Developent helper Enum for strong typing the identifiers of commonly used activity types. 
        /// The full invetory is represented through the domain entity type <see cref="ActivityType"/>
        /// </summary>
        public enum ActivityTypeEnum
        {
            DistanceActivity = 1,
            FuelUsage = 3,
            PurchasedElectricityMarketBased = 4,
            PurchasedElectricityLocationBased = 5,
            PurchasedNaturalGas = 6,
            FuelUsageStationaryCombustion = 7,
            UpstreamTransportationSpendBased = 28
        }

        public enum EmissionSourceEnum
        {
            MobileCombustion = 1,
            EmployeeCommuting = 2,
            BusinessTravel = 3,
            PurchasedElectricity = 4,
            StationaryCombustion = 5,
            PurchasedCooling = 6,
            FugitiveEmissions = 7,
            ProcessEmissions = 8,
            PurchasedSteam = 9,
            PurchasedGoodsAndServices = 10,
            UpstreamTransportation = 11,
            DownstreamTransportation = 12,
            UseOfSoldProducts = 13,
            DownstreamLeasedAssets = 14,
            UpstreamLeasedAssets = 15,
            Investments = 16,
            CapitalGoods = 17,
            ProcessingOfSoldProducts = 18,
            EndOfLifeTreatmentOfSoldProducts = 19,
            Franchises = 20,
            WasteGeneratedInOperations = 21,
            WaterSupply = 22,
            WaterTreatment = 23,
            FuelAndEnergyRelatedActivitites = 24
        }

        public enum ProductEmissionTypeEnum
        {
            Benchmark = 1,
            Product = 2,
            Organization = 3        
        }

        public enum ProductTypeEnum
        {
            Supplier = 1,
            Customer = 2
        }

    }
}
