using ClimateCamp.CarbonCompute;
using Mobile.Combustion.Calculation.Models;

namespace Calculation.Helpers
{
    public static class GHGCalculationHelper
    {
        public static double CalculateCO2EmissionFactors(double activityQuantity, EmissionFactorModel emissionFactor)
        {
            return activityQuantity * emissionFactor.Co2;
        }

        public static double CalculateCH4EmissionFactors(double activityQuantity, EmissionFactorModel emissionFactor)
        {
            return activityQuantity * emissionFactor.Ch4;
        }

        public static double CalculateN20EmissionFactors(double activityQuantity, EmissionFactorModel emissionFactor)
        {
            return activityQuantity * emissionFactor.N20;
        }

        public static double ConvertLitreToGallon(double FuelInLitre)
        {
            return (FuelInLitre / 3.78541);
        }

        public static double CalculateCO2eEmissionFactors(Emission emmision, CalculateCO2eEmissionFactorModel gwpFactors)
        {
            var CO2eEmission = ((emmision.CO2 * gwpFactors.CO2) + (emmision.CH4 * gwpFactors.CH4) + (emmision.N20 * gwpFactors.N2O));
            // need to find way to convert that to unit dynamically instead of static conversion to tonnes
            return (double)(CO2eEmission / 1000);
        }

        public static double CalculateCO2eEmissionFactorsDynamically(Emission emmision, CalculateCO2eEmissionFactorModel gwpFactors)
        {
            var CO2eEmission = ((emmision.CO2 * gwpFactors.CO2) + (emmision.CH4 * gwpFactors.CH4) + (emmision.N20 * gwpFactors.N2O));
            // need to find way to convert that to unit dynamically instead of static conversion to tonnes
            return (double)(CO2eEmission);
        }


        public static double CalculateStationaryCombustionCO2EmissionFactors(double activityQuantity, EmissionFactorModel emissionFactor)
        {
            return activityQuantity * emissionFactor.Co2;
        }

        public static double CalculateStationaryCombustionCH4EmissionFactors(double activityQuantity, EmissionFactorModel emissionFactor)
        {
            return activityQuantity * emissionFactor.Ch4;
        }

        public static double CalculateStationaryCombustionN20EmissionFactors(double activityQuantity, EmissionFactorModel emissionFactor)
        {
            return activityQuantity * emissionFactor.N20;
        }

        public static double CalculatePurchasedElectricityCO2EmissionFactors(double activityQuantity, EmissionFactorModel emissionFactor)
        {
            return activityQuantity * emissionFactor.Co2;
        }

        public static double CalculatePurchasedElectricityCH4EmissionFactors(double activityQuantity, EmissionFactorModel emissionFactor)
        {
            return activityQuantity * emissionFactor.Ch4;
        }

        public static double CalculatePurchasedElectricityN20EmissionFactors(double activityQuantity, EmissionFactorModel emissionFactor)
        {
            return activityQuantity * emissionFactor.N20;
        }
    }
}
