namespace ClimateCamp.Application.Common
{
    public static class EmissionsCalculator
    {
        /// <summary>
        /// Used to determine if there is already an emission calculation coming from the frontend. <br/>
        /// If there is, then the emission property will not be null in which case the calculatedEmissions is the emision value. <br/>
        /// Otherwise, the calculatedEmissions will be the multiplication between quantity and co2e factor.
        /// </summary>
        /// <param name="emission"></param>
        /// <param name="co2e"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public static float? CalculateEmissions(float? emission, float? co2e, float? quantity)
        {
            float? calculatedEmissions = emission;

            if (calculatedEmissions == null)
            {
                if (co2e != null)
                {
                    calculatedEmissions = quantity * co2e;
                }
                else
                {
                    calculatedEmissions = co2e;
                }
            }

            return calculatedEmissions;
        }
    }
}
