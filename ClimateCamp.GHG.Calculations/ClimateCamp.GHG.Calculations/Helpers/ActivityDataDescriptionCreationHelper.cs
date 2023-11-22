namespace ClimateCamp.GHG.Calculations.Helpers
{
    public static class ActivityDataDescriptionCreationHelper
    {
        public static string CreateActivityDataDescription(float? quantity, string quantityUnitName, float? co2e)
        {
            if(quantity != null && quantityUnitName != null && co2e != null)
            {
                return $"Roll Forward: Quantity: {quantity}, Unit: {quantityUnitName}, TCO2e: {co2e}";
            }
            else
            {
                string q = quantity != null ? quantity.ToString() : "N/A";
                string qUnit = quantityUnitName ?? "N/A";
                string c = co2e != null ? co2e.ToString() : "N/A";
                return $"Roll Forward: Quantity: {q}, Unit: {qUnit}, TCO2e: {c}";
            }
        }
    }
}
