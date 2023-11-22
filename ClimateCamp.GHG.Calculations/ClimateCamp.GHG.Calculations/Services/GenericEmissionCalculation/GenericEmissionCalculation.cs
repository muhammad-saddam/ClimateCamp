using Abp.UI;
using ClimateCamp.EntityFrameworkCore;
using ClimateCamp.GHG.Calculations.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ClimateCamp.GHG.Calculations.Services.GenericEmissionCalculation
{
    public class GenericEmissionCalculation : IGenericEmissionCalculation
    {
        private readonly CommonDbContext _dbContext;
        private readonly ILogger _logger;

        public GenericEmissionCalculation(CommonDbContext dbContext, ILoggerFactory loggerFactory)
        {
            _dbContext = dbContext;
            _logger = loggerFactory.CreateLogger<GenericEmissionCalculation>();
        }
        public async Task<GenericEmissionCalculationModel> CalculateGenericEmission(Guid emissionFactorId, float quantity, int unitId, float? userConversionFactor, Guid productId = default(Guid))
        {
            try
            {
                //get unit object for product benchmark unit
                var quantityUnit = await _dbContext.Units.FirstOrDefaultAsync(x => x.Id == unitId);
                //get emission factor filtered by emission factor selected from frontend dropdown
                var emissionFactor = await _dbContext.EmissionsFactors
                    .Include(x => x.CO2EUnit)
                    .Include(x => x.Unit)
                    .FirstOrDefaultAsync(x => x.Id == emissionFactorId);

                if (emissionFactor == null)
                {
                    throw new UserFriendlyException("Emission factor not found.");
                }
                //existing product
                if (productId != Guid.Empty)
                {
                    //get product conversion factor
                    var productConversionFactor = await _dbContext.ConversionFactors.FirstOrDefaultAsync(x => x.ProductId == productId);

                    if (productConversionFactor != null)
                    {
                        //get conversion factor unit object
                        var conversionFactorUnit = await _dbContext.Units.SingleAsync(x => x.Id == productConversionFactor.ConversionUnit);
                        // check if productUnit and product conversion factor unit have same group
                        // OR if conversion factor and emission factor have the same unit group
                        // not sure if the first check is needed since by definition, conversion factor unit group will not be the same as the quantity unit group
                        // will leave it just in case.
                        // TODO: check if first condition is actually needed 
                        if (quantityUnit.Group == conversionFactorUnit.Group || conversionFactorUnit.Group == emissionFactor.Unit.Group)
                        {
                            // convert to base unit and conversion factor
                            var convertedQuantity = quantity * quantityUnit.Multiplier;
                            var convertedEmissionFactor = emissionFactor.CO2E / emissionFactor.Unit.Multiplier;

                            return new GenericEmissionCalculationModel
                            {
                                UnitId = emissionFactor.CO2EUnit.Id,
                                Emission = convertedQuantity * convertedEmissionFactor * productConversionFactor.ConversionFactor,
                                IsConversionFactorExist = true,
                                ConversionFactor = productConversionFactor.ConversionFactor,
                            };
                        }

                    }

                    // handling the case when editing an existing activity data, that has a product assigned but without a conversion factor linked to the product
                    else if (userConversionFactor != null && userConversionFactor != 0)
                    {
                        var convertedQuantity = quantity * quantityUnit.Multiplier;
                        var convertedEmissionFactor = emissionFactor.CO2E / emissionFactor.Unit.Multiplier;

                        return new GenericEmissionCalculationModel
                        {
                            UnitId = emissionFactor.CO2EUnit.Id,
                            Emission = convertedQuantity * convertedEmissionFactor * (float)userConversionFactor,
                            IsConversionFactorExist = true,
                            ConversionFactor = (float)userConversionFactor,
                        };
                    }

                }

                if (userConversionFactor != 0 && productId == Guid.Empty)   
                {
                    var convertedQuantity = quantity * quantityUnit.Multiplier;
                    var convertedEmissionFactor = emissionFactor.CO2E / emissionFactor.Unit.Multiplier;

                    return new GenericEmissionCalculationModel
                    {
                        UnitId = emissionFactor.CO2EUnit.Id,
                        Emission = convertedQuantity * convertedEmissionFactor * (float)userConversionFactor,
                        IsConversionFactorExist = true,
                        ConversionFactor = (float)userConversionFactor,
                    };
                }

                if (quantityUnit.Group != emissionFactor.Unit.Group)
                {
                    throw new UserFriendlyException("Cannot calculate emissions if quantity unit and emission factor unit are not in the same unit group.");
                }

                // This checks if the quantity unit is the same as the emission factor unit and if they are not then we take the quantity and bring it at the base unit
                // and we do the same for the emission factor as well. This is so we can convert within the same unit group.  
                if (unitId != emissionFactor.Unit.Id)
                {
                    var convertedQuantity = quantity * quantityUnit.Multiplier;
                    var convertedEmissionFactor = emissionFactor.CO2E / emissionFactor.Unit.Multiplier;

                    return new GenericEmissionCalculationModel
                    {
                        UnitId = emissionFactor.CO2EUnit.Id,
                        Emission = convertedQuantity * convertedEmissionFactor
                    };
                }
                else
                {
                    return new GenericEmissionCalculationModel
                    {
                        UnitId = emissionFactor.CO2EUnit.Id,
                        Emission = emissionFactor.CO2E * quantity
                    };
                }
            }
            catch (UserFriendlyException userException)
            {
                throw new UserFriendlyException(userException.Message);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Method: CalculateGenericEmission - Exception: {exception}");
                return null;
            }
        }
    }
}
