using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Events.Bus;
using Abp.UI;
using ClimateCamp.CarbonCompute;
using ClimateCamp.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClimateCamp.Domain
{
    /// <summary>
    /// https://github.com/aspnetboilerplate/aspnetboilerplate/blob/master/doc/WebSite/Zero/Organization-Units.md
    /// </summary>
    public class ActivityDataManager : DomainService//, IActivityDataManager
    {
        public IEventBus EventBus { get; set; }

        private readonly IRepository<EmissionsFactor, Guid> _emissionsFactorRepository;
        private readonly IRepository<ActivityData, Guid> _activityDataRepository;
        private readonly IRepository<OrganizationUnit, Guid> _organizationUnitRepository;
        private readonly IRepository<ActivityType, int> _activityTypeRepository;

        public ActivityDataManager(
            IRepository<EmissionsFactor, Guid> emissionsFactorRepository,
            IRepository<ActivityData, Guid> activityDataRepository,
            IRepository<OrganizationUnit, Guid> organizationUnitRepository,
            IRepository<ActivityType, int> activityTypeRepository)
        {
            _emissionsFactorRepository = emissionsFactorRepository;
            _activityDataRepository = activityDataRepository;
            _organizationUnitRepository = organizationUnitRepository;
            _activityTypeRepository = activityTypeRepository;

            EventBus = NullEventBus.Instance;
        }

        public async Task<ActivityData> GetActivityDataAsync(Guid id)
        {
            var activity = await _activityDataRepository.FirstOrDefaultAsync(id);
            if (activity == null)
            {
                throw new UserFriendlyException("Could not found the ActivityData, maybe it's deleted!");
            }

            return activity;
        }

        /// <summary>
        /// TODO: Add the logic to extract also activities from child OrganizationUnits https://github.com/aspnetboilerplate/aspnetboilerplate/blob/master/doc/WebSite/Zero/Organization-Units.md
        /// </summary>


        public async Task CreateActivityDataAsync(ActivityData data)
        {
            await _activityDataRepository.InsertAsync(data);
        }

        public async Task<IReadOnlyList<EmissionsFactor>> GetRegisteredUsersAsync(EmissionsFactorsLibrary library)
        {
            return await _emissionsFactorRepository
                .GetAll()
                .Include(f => f.EmissionsSource)
                .Where(f => f.Library == library)
                .Select(f => f)
                .ToListAsync();
        }

        public async Task<ActivityType> GetActivityTypeByEmission(Emission emission)
        {
            return await _activityTypeRepository
                .GetAll()
                .Include(f => f.EmissionsSource)
                .Where(f => f.Id == emission.ActivityData.ActivityTypeId)
                .FirstOrDefaultAsync();
        }

        //[UnitOfWork]
        //public virtual async Task<IReadOnlyList<ActivityData>> GetOrganizationUnitActivityDataAsync(Guid ouId)
        //{
        //    var newQuery = _activityDataRepository.GetAll()
        //        .Where(a => a.OrganizationUnit.Id.Equals(ouId));

        //    return newQuery.ToList();
        //}

        //[UnitOfWork]
        //public virtual async Task<IReadOnlyList<ActivityData>> GetChildActivityDataAsync(Guid ouID)
        //{

        //    var organizationUnitIds = _organizationUnitRepository.GetAll()
        //        .Where(x => x.ParentOrganizationUnitId == ouID).Select(ou => ou.Id).ToList();

        //    var newQuery = _activityDataRepository.GetAll()
        //        .Where(a => organizationUnitIds.Contains(a.OrganizationUnit.Id));

        //    return newQuery.ToList();
        //}
    }
}
