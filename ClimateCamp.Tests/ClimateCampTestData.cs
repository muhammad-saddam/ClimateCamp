using Abp.Dependency;
using System;

namespace ClimateCamp.Tests
{
    public class ClimateCampTestData : ISingletonDependency
    {
        #region Users

        public Guid UserAdminId { get; internal set; }
        public string UserAdminUserName { get; } = "admin@brewer.eu";

        public Guid UserLisaId { get; } = Guid.NewGuid();
        public string UserLisaUserName { get; } = "lisa@brewer.eu";

        public Guid UserDaveId { get; } = Guid.NewGuid();
        public string UserDaveUserName { get; } = "dave@ecomalt.eu";

        public Guid UserKathyId { get; } = Guid.NewGuid();
        public string UserKathyUserName { get; } = "kathy@brewer.eu";

        #endregion

        #region ORGANIZATIONS

        public Guid OrganizationBreweryId { get; } = Guid.NewGuid();
        public string OrganizationBreweryName { get; } = "EU Brewer";

        public Guid OrganizationBrewerySuplierId { get; } = Guid.NewGuid();
        public string OrganizationBrewerySuplierName { get; } = "EcoMalt";

        #endregion

        #region Products
        public Guid ProductMalt10Id { get; } = Guid.NewGuid();
        public string ProductMalt10IdName { get; } = "Malt 10%";
        #endregion

        public ClimateCampTestData()
        {

        }
    }
}
