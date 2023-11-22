using ClimateCamp.CarbonCompute;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ClimateCamp.EntityFrameworkCore.Seed.Host
{
    public class DefaultActivityTypeCreator
    {
        private readonly CommonDbContext _context;
        public List<ActivityType> InitialActivityType => GetInitialActivityTypes();
        public DefaultActivityTypeCreator(CommonDbContext context)
        {
            _context = context;
        }
        public void Create()
        {
            CreateActivityTypes();
        }

        private void CreateActivityTypes()
        {
            foreach (var ActivityTypes in InitialActivityType)
            {
                AddActivityTypeIfNotExists(ActivityTypes);
            }
        }

        private void AddActivityTypeIfNotExists(ActivityType activityType)
        {
            if (_context.ActivityTypes.IgnoreQueryFilters().Any(t => t.Id == activityType.Id))
            {
                return;
            }

            _context.ActivityTypes.Add(activityType);
            _context.SaveChanges();
        }

        private List<ActivityType> GetInitialActivityTypes()
        {

            return new List<ActivityType>
            {
                new ActivityType { Id = 1, Name = "Distance Activity", EmissionsSourceId = 1 },
                new ActivityType { Id = 3, Name = "Fuel Usage", EmissionsSourceId = 1},
                new ActivityType { Id = 5, Name = "Purchased Electricity - Location Based", EmissionsSourceId = 4},
                new ActivityType { Id = 6, Name = "Purchased Natural Gas", EmissionsSourceId = 5},
                new ActivityType { Id = 7, Name = "Fuel Usage - Stationary Combustion", EmissionsSourceId = 5},
                new ActivityType { Id = 8, Name = "Solar Panels Generated Electricity", EmissionsSourceId = 4},
                new ActivityType { Id = 27, Name = "Purchased goods & services", EmissionsSourceId = 10},
            };
        }
    }
}
