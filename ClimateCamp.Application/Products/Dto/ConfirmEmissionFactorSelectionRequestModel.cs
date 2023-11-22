using System;
using System.Collections.Generic;

namespace ClimateCamp.Application.Products.Dto
{
    public class ConfirmEmissionFactorSelectionRequestModel
    {
        public Guid OrganizationId { get; set; }
        public Guid ProductId { get; set; }
        public ProductEmissionTypesVM SelectedProductEmissionType { get; set; }
        public List<int> SelectedPeriods { get; set; }
    }
}
