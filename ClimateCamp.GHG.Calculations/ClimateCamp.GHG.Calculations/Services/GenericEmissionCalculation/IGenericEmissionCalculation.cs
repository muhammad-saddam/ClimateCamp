using ClimateCamp.GHG.Calculations.Models;
using System;
using System.Threading.Tasks;

namespace ClimateCamp.GHG.Calculations.Services.GenericEmissionCalculation
{
    public interface IGenericEmissionCalculation
    {
        Task<GenericEmissionCalculationModel> CalculateGenericEmission(Guid emissionFactorId, float quantity, int unitId, float? userConversionFactor, Guid productId = default(Guid));
    }
}
