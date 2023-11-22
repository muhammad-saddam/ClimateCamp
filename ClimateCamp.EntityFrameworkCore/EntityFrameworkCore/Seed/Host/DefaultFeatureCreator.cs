using ClimateCamp.Core.Features;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ClimateCamp.EntityFrameworkCore.Seed.Host
{
    public class DefaultFeatureCreator
    {
        private readonly CommonDbContext _context;
        
        
        private List<EditionFeatureSettingCustom> InitialFeatures => GetInitialFeatures();

        public DefaultFeatureCreator(CommonDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateFeatures();
        }

        private void CreateFeatures()
        {
            foreach (var feature in InitialFeatures)
            {
                CreateFeatureIfNotExists(feature);
            }
        }

        private void CreateFeatureIfNotExists(EditionFeatureSettingCustom customFeature)
        {
            if (_context.EditionFeatureSettings.IgnoreQueryFilters().Any(ef => ef.EditionId == customFeature.EditionId && ef.Name == customFeature.Name))
            {
                return;
            }

            var parentFeature = new EditionFeatureSettingCustom
            {
                Name = customFeature.Name,
                Icon = customFeature.Icon,
                Type = customFeature.Type,
                IsActive = customFeature.IsActive,
                ShowActiveLabel = customFeature.ShowActiveLabel,
                EditionId = customFeature.EditionId,
                ParentId = null
            };

            _context.EditionFeatureSettings.Add(parentFeature);

            if (customFeature.ChildFeatures.Any())
            {
                _context.SaveChanges();

                var childFeaturesList = new List<EditionFeatureSettingCustom>();

                foreach (var childFeature in customFeature.ChildFeatures)
                {
                    var feature = new EditionFeatureSettingCustom
                    {
                        Name = childFeature.Name,
                        Type = childFeature.Type,
                        Icon = childFeature.Icon,
                        IsActive = childFeature.IsActive,
                        ShowActiveLabel = childFeature.ShowActiveLabel,
                        EditionId = parentFeature.EditionId,
                        ParentId = parentFeature.Id
                    };
                    childFeaturesList.Add(feature);
                }

                _context.EditionFeatureSettings.AddRange(childFeaturesList);
            }

            _context.SaveChanges();
        }

        private int GetEditionIdByName(string editionName)
        {
            return _context.CustomEditions.Where(x => x.DisplayName == editionName).Single().Id;
        }

        private List<EditionFeatureSettingCustom> GetInitialFeatures()
        {

            return new List<EditionFeatureSettingCustom>
            {
                #region Features
                new EditionFeatureSettingCustom{Name = "Custom Modules In Co-Creation", Icon = "fas fa-home", EditionId = GetEditionIdByName("Climatecamp Enterprise+ Edition"), Type = (int)EditionFeartureType.Feature},
                new EditionFeatureSettingCustom{Name = "Custom Modules In Co-Creation", Icon = "fas fa-home", EditionId = GetEditionIdByName("Climatecamp Enterprise Edition"), IsActive = false, Type = (int)EditionFeartureType.Feature},
                new EditionFeatureSettingCustom{Name = "Custom Modules In Co-Creation", Icon = "fas fa-home", EditionId = GetEditionIdByName("Climatecamp Business Edition"), IsActive = false, Type = (int)EditionFeartureType.Feature},
                new EditionFeatureSettingCustom{Name = "Custom Modules In Co-Creation", Icon = "fas fa-home", EditionId = GetEditionIdByName("Climatecamp Expense Partner Edition"), IsActive = false, Type = (int)EditionFeartureType.Feature},

                new EditionFeatureSettingCustom{Name = "Users", Icon = "fas fa-users", EditionId = GetEditionIdByName("Climatecamp Enterprise+ Edition"), ShowActiveLabel = true, Type = (int)EditionFeartureType.Feature},
                new EditionFeatureSettingCustom{Name = "Users", Icon = "fas fa-users", EditionId = GetEditionIdByName("Climatecamp Enterprise Edition"), ShowActiveLabel = true, Type = (int)EditionFeartureType.Feature},
                new EditionFeatureSettingCustom{Name = "Users", Icon = "fas fa-users", EditionId = GetEditionIdByName("Climatecamp Business Edition"), ShowActiveLabel = true, Type = (int)EditionFeartureType.Feature},
                new EditionFeatureSettingCustom{Name = "Users", Icon = "fas fa-users", EditionId = GetEditionIdByName("Climatecamp Expense Partner Edition"), ShowActiveLabel = true, Type = (int)EditionFeartureType.Feature},

                new EditionFeatureSettingCustom{Name = "Overview", Icon = "fas fa-home", EditionId = GetEditionIdByName("Climatecamp Enterprise+ Edition"), Type = (int)EditionFeartureType.Feature},
                new EditionFeatureSettingCustom{Name = "Overview", Icon = "fas fa-home", EditionId = GetEditionIdByName("Climatecamp Enterprise Edition"), Type = (int)EditionFeartureType.Feature},
                new EditionFeatureSettingCustom{Name = "Overview", Icon = "fas fa-home", EditionId = GetEditionIdByName("Climatecamp Business Edition"), Type = (int)EditionFeartureType.Feature},
                new EditionFeatureSettingCustom{Name = "Overview", Icon = "fas fa-home", EditionId = GetEditionIdByName("Climatecamp Expense Partner Edition"), Type = (int)EditionFeartureType.Feature},

                new EditionFeatureSettingCustom
                {  Name = "My Data",
                    Icon = "fas fa-file",
                    EditionId = GetEditionIdByName("Climatecamp Enterprise+ Edition"),
                    Type = (int)EditionFeartureType.Feature,
                    ChildFeatures = new List<EditionFeatureSettingCustom>
                    {
                        new EditionFeatureSettingCustom{Name = "Manual Data Entry", Icon = null, Type = (int)EditionFeartureType.Feature},
                        new EditionFeatureSettingCustom{Name = "Data Connections", Icon = null, Type = (int)EditionFeartureType.Feature},
                        new EditionFeatureSettingCustom{Name = "Expense Partner Connectors (* Rydoo)", Icon = null, Type = (int)EditionFeartureType.Feature},
                        new EditionFeatureSettingCustom{Name = "Custom Data Connectors", Icon = null, ShowActiveLabel = true, Type = (int)EditionFeartureType.Feature},
                        new EditionFeatureSettingCustom{Name = "Suppliers", Icon = null, Type = (int)EditionFeartureType.Feature}
                    }
                },

                new EditionFeatureSettingCustom
                {  Name = "My Data",
                    Icon = "fas fa-file",
                    EditionId = GetEditionIdByName("Climatecamp Enterprise Edition"),
                    Type = (int)EditionFeartureType.Feature,
                    ChildFeatures = new List<EditionFeatureSettingCustom>
                    {
                        new EditionFeatureSettingCustom{Name = "Manual Data Entry", Icon = null, Type = (int)EditionFeartureType.Feature},
                       new EditionFeatureSettingCustom{Name = "Data Connections", Icon = null, Type = (int)EditionFeartureType.Feature},
                        new EditionFeatureSettingCustom{Name = "Custom Data Connectors", Icon = null, ShowActiveLabel = true, Type = (int)EditionFeartureType.Feature},
                        new EditionFeatureSettingCustom{Name = "Suppliers", Icon = null, Type = (int)EditionFeartureType.Feature}
                    }
                },

                new EditionFeatureSettingCustom
                {  Name = "My Data",
                    Icon = "fas fa-file",
                    EditionId = GetEditionIdByName("Climatecamp Business Edition"),
                    Type = (int)EditionFeartureType.Feature,
                    ChildFeatures = new List<EditionFeatureSettingCustom>
                    {
                        new EditionFeatureSettingCustom{Name = "Manual Data Entry", Icon = null, Type = (int)EditionFeartureType.Feature},
                        new EditionFeatureSettingCustom{Name = "Data Connections", Icon = null, Type = (int)EditionFeartureType.Feature},
                        new EditionFeatureSettingCustom{Name = "Custom Data Connectors", Icon = null, IsActive = false, Type = (int)EditionFeartureType.Feature},
                        new EditionFeatureSettingCustom{Name = "Suppliers", Icon = null, IsActive = false, Type = (int)EditionFeartureType.Feature}
                    }
                },

                new EditionFeatureSettingCustom
                {  Name = "My Data",
                    Icon = "fas fa-file",
                    EditionId = GetEditionIdByName("Climatecamp Expense Partner Edition"),
                    Type = (int)EditionFeartureType.Feature,
                    ChildFeatures = new List<EditionFeatureSettingCustom>
                    {
                        new EditionFeatureSettingCustom{Name = "Manual Data Entry", Icon = null, Type = (int)EditionFeartureType.Feature},
                        new EditionFeatureSettingCustom{Name = "Data Connections", Icon = null, Type = (int)EditionFeartureType.Feature},
                        new EditionFeatureSettingCustom{Name = "Expense Partner Connectors (* Rydoo)", Icon = null, ShowActiveLabel = true, Type = (int)EditionFeartureType.Feature},
                        new EditionFeatureSettingCustom{Name = "Custom Data Connectors", Icon = null, IsActive = false, Type = (int)EditionFeartureType.Feature},
                        new EditionFeatureSettingCustom{Name = "Suppliers", Icon = null, IsActive = false, Type = (int)EditionFeartureType.Feature}
                    }
                },

                new EditionFeatureSettingCustom{Name = "Target", Icon = "fas fa-bullseye", EditionId = GetEditionIdByName("Climatecamp Enterprise+ Edition"), Type = (int)EditionFeartureType.Feature},
                new EditionFeatureSettingCustom{Name = "Target", Icon = "fas fa-bullseye", EditionId = GetEditionIdByName("Climatecamp Enterprise Edition"), Type = (int)EditionFeartureType.Feature},
                new EditionFeatureSettingCustom{Name = "Target", Icon = "fas fa-bullseye", EditionId = GetEditionIdByName("Climatecamp Business Edition"), Type = (int)EditionFeartureType.Feature},
                new EditionFeatureSettingCustom{Name = "Target", Icon = "fas fa-bullseye", EditionId = GetEditionIdByName("Climatecamp Expense Partner Edition"), IsActive = false, Type = (int)EditionFeartureType.Feature},

                new EditionFeatureSettingCustom{Name = "Your Suppliers", Icon = "fas fa-angle-double-left", EditionId = GetEditionIdByName("Climatecamp Enterprise+ Edition"), Type = (int)EditionFeartureType.Feature},
                new EditionFeatureSettingCustom{Name = "Your Suppliers", Icon = "fas fa-angle-double-left", EditionId = GetEditionIdByName("Climatecamp Enterprise Edition"), Type = (int)EditionFeartureType.Feature},
                new EditionFeatureSettingCustom{Name = "Your Suppliers", Icon = "fas fa-angle-double-left", EditionId = GetEditionIdByName("Climatecamp Business Edition"), Type = (int)EditionFeartureType.Feature},
                new EditionFeatureSettingCustom{Name = "Your Suppliers", Icon = "fas fa-angle-double-left", EditionId = GetEditionIdByName("Climatecamp Expense Partner Edition"), IsActive = false, Type = (int)EditionFeartureType.Feature},

                new EditionFeatureSettingCustom{Name = "Collaboration", Icon = "fas fa-angle-double-right", EditionId = GetEditionIdByName("Climatecamp Enterprise+ Edition"), Type = (int)EditionFeartureType.Feature},
                new EditionFeatureSettingCustom{Name = "Collaboration", Icon = "fas fa-angle-double-right", EditionId = GetEditionIdByName("Climatecamp Enterprise Edition"), Type = (int)EditionFeartureType.Feature},
                new EditionFeatureSettingCustom{Name = "Collaboration", Icon = "fas fa-angle-double-right", EditionId = GetEditionIdByName("Climatecamp Business Edition"), Type = (int)EditionFeartureType.Feature},
                new EditionFeatureSettingCustom{Name = "Collaboration", Icon = "fas fa-angle-double-right", EditionId = GetEditionIdByName("Climatecamp Expense Partner Edition"), IsActive = false, Type = (int)EditionFeartureType.Feature},

                new EditionFeatureSettingCustom{Name = "Products", Icon = "fas fa-cube", EditionId = GetEditionIdByName("Climatecamp Enterprise+ Edition"), Type = (int)EditionFeartureType.Feature},
                new EditionFeatureSettingCustom{Name = "Products", Icon = "fas fa-cube", EditionId = GetEditionIdByName("Climatecamp Enterprise Edition"), Type = (int)EditionFeartureType.Feature},
                new EditionFeatureSettingCustom{Name = "Products", Icon = "fas fa-cube", EditionId = GetEditionIdByName("Climatecamp Business Edition"), Type = (int)EditionFeartureType.Feature},
                new EditionFeatureSettingCustom{Name = "Products", Icon = "fas fa-cube", EditionId = GetEditionIdByName("Climatecamp Expense Partner Edition"), IsActive = false, Type = (int)EditionFeartureType.Feature},

                new EditionFeatureSettingCustom{Name = "Reduce", Icon = "fas fa-angle-double-down", EditionId = GetEditionIdByName("Climatecamp Enterprise+ Edition"), Type = (int)EditionFeartureType.Feature},
                new EditionFeatureSettingCustom{Name = "Reduce", Icon = "fas fa-angle-double-down", EditionId = GetEditionIdByName("Climatecamp Enterprise Edition"), Type = (int)EditionFeartureType.Feature},
                new EditionFeatureSettingCustom{Name = "Reduce", Icon = "fas fa-angle-double-down", EditionId = GetEditionIdByName("Climatecamp Business Edition"), Type = (int)EditionFeartureType.Feature},
                new EditionFeatureSettingCustom{Name = "Reduce", Icon = "fas fa-angle-double-down", EditionId = GetEditionIdByName("Climatecamp Expense Partner Edition"), IsActive = false, Type = (int)EditionFeartureType.Feature},

                new EditionFeatureSettingCustom{Name = "Compensate", Icon = "fas fa-globe", EditionId = GetEditionIdByName("Climatecamp Enterprise+ Edition"), Type = (int)EditionFeartureType.Feature},
                new EditionFeatureSettingCustom{Name = "Compensate", Icon = "fas fa-globe", EditionId = GetEditionIdByName("Climatecamp Enterprise Edition"), Type = (int)EditionFeartureType.Feature},
                new EditionFeatureSettingCustom{Name = "Compensate", Icon = "fas fa-globe", EditionId = GetEditionIdByName("Climatecamp Business Edition"), Type = (int)EditionFeartureType.Feature},
                new EditionFeatureSettingCustom{Name = "Compensate", Icon = "fas fa-globe", EditionId = GetEditionIdByName("Climatecamp Expense Partner Edition"), IsActive = false, Type = (int)EditionFeartureType.Feature},

                new EditionFeatureSettingCustom{Name = "Report", Icon = "fas fa-file-export", EditionId = GetEditionIdByName("Climatecamp Enterprise+ Edition"), Type = (int)EditionFeartureType.Feature},
                new EditionFeatureSettingCustom{Name = "Report", Icon = "fas fa-file-export", EditionId = GetEditionIdByName("Climatecamp Enterprise Edition"), Type = (int)EditionFeartureType.Feature},
                new EditionFeatureSettingCustom{Name = "Report", Icon = "fas fa-file-export", EditionId = GetEditionIdByName("Climatecamp Business Edition"), Type = (int)EditionFeartureType.Feature},
                new EditionFeatureSettingCustom{Name = "Report", Icon = "fas fa-file-export", EditionId = GetEditionIdByName("Climatecamp Expense Partner Edition"), ShowActiveLabel = true, Type = (int)EditionFeartureType.Feature},

                new EditionFeatureSettingCustom
                {
                    Name = "Settings",
                    Icon = "fas fa-cog",
                    EditionId = GetEditionIdByName("Climatecamp Enterprise+ Edition"),
                    Type = (int)EditionFeartureType.Feature,
                    ChildFeatures = new List<EditionFeatureSettingCustom>
                    {
                        new EditionFeatureSettingCustom{Name = "Organization", Icon = null, Type = (int)EditionFeartureType.Feature},
                        new EditionFeatureSettingCustom{Name = "Organization Hierarchy", Icon = null, Type = (int)EditionFeartureType.Feature},
                        new EditionFeatureSettingCustom{Name = "Users", Icon = null, Type = (int)EditionFeartureType.Feature},
                        new EditionFeatureSettingCustom{Name = "Style", Icon = null, Type = (int)EditionFeartureType.Feature},
                        new EditionFeatureSettingCustom{Name = "Benchmarking", Icon = null, Type = (int)EditionFeartureType.Feature}
                    }
                },
                new EditionFeatureSettingCustom
                {
                    Name = "Settings",
                    Icon = "fas fa-cog",
                    EditionId = GetEditionIdByName("Climatecamp Enterprise Edition"),
                    Type = (int)EditionFeartureType.Feature,
                    ChildFeatures = new List<EditionFeatureSettingCustom>
                    {
                        new EditionFeatureSettingCustom{Name = "Organization", Icon = null, Type = (int)EditionFeartureType.Feature},
                        new EditionFeatureSettingCustom{Name = "Organization Hierarchy", Icon = null, Type = (int)EditionFeartureType.Feature},
                        new EditionFeatureSettingCustom{Name = "Users", Icon = null, Type = (int)EditionFeartureType.Feature},
                        new EditionFeatureSettingCustom{Name = "Style", Icon = null, Type = (int)EditionFeartureType.Feature},
                        new EditionFeatureSettingCustom{Name = "Benchmarking", Icon = null, Type = (int)EditionFeartureType.Feature}
                    }
                },
                new EditionFeatureSettingCustom
                {
                    Name = "Settings",
                    Icon = "fas fa-cog",
                    EditionId = GetEditionIdByName("Climatecamp Business Edition"),
                    Type = (int)EditionFeartureType.Feature,
                    ChildFeatures = new List<EditionFeatureSettingCustom>
                    {
                        new EditionFeatureSettingCustom{Name = "Organization", Icon = null, Type = (int)EditionFeartureType.Feature},
                        new EditionFeatureSettingCustom{Name = "Organization Hierarchy", Icon = null, Type = (int)EditionFeartureType.Feature},
                        new EditionFeatureSettingCustom{Name = "Users", Icon = null, Type = (int)EditionFeartureType.Feature},
                        new EditionFeatureSettingCustom{Name = "Style", Icon = null, Type = (int)EditionFeartureType.Feature},
                        new EditionFeatureSettingCustom{Name = "Benchmarking", Icon = null, Type = (int)EditionFeartureType.Feature}
                    }
                },
                new EditionFeatureSettingCustom
                {
                    Name = "Settings",
                    Icon = "fas fa-cog",
                    EditionId = GetEditionIdByName("Climatecamp Expense Partner Edition"),
                    Type = (int)EditionFeartureType.Feature,
                    ChildFeatures = new List<EditionFeatureSettingCustom>
                    {
                        new EditionFeatureSettingCustom{Name = "Organization", Icon = null, Type = (int)EditionFeartureType.Feature},
                        new EditionFeatureSettingCustom{Name = "Organization Hierarchy",  Icon = null, Type = (int)EditionFeartureType.Feature},
                        new EditionFeatureSettingCustom{Name = "Users", Icon = null, Type = (int)EditionFeartureType.Feature},
                        new EditionFeatureSettingCustom{Name = "Style", Icon = null, Type = (int)EditionFeartureType.Feature},
                        new EditionFeatureSettingCustom{Name = "Benchmarking", Icon = null, Type = (int)EditionFeartureType.Feature}
                    }
                },
                new EditionFeatureSettingCustom{Name = "Roles", Icon = "fas fa-file-export", EditionId = GetEditionIdByName("Climatecamp Enterprise+ Edition"), Type = (int)EditionFeartureType.Feature},
                new EditionFeatureSettingCustom{Name = "Roles", Icon = "fas fa-file-export", EditionId = GetEditionIdByName("Climatecamp Enterprise Edition"), Type = (int)EditionFeartureType.Feature},
                new EditionFeatureSettingCustom{Name = "Roles", Icon = "fas fa-file-export", EditionId = GetEditionIdByName("Climatecamp Business Edition"), Type = (int)EditionFeartureType.Feature},
                new EditionFeatureSettingCustom{Name = "Roles", Icon = "fas fa-file-export", EditionId = GetEditionIdByName("Climatecamp Expense Partner Edition"), ShowActiveLabel = true, Type = (int)EditionFeartureType.Feature},
                #endregion

                 #region Emission Sources
                new EditionFeatureSettingCustom{Name = "Mobile Combustion", Icon = null, EditionId = GetEditionIdByName("Climatecamp Enterprise+ Edition"), Type = (int)EditionFeartureType.EmissionSource},
                new EditionFeatureSettingCustom{Name = "Mobile Combustion", Icon = null, EditionId = GetEditionIdByName("Climatecamp Enterprise Edition"), Type = (int)EditionFeartureType.EmissionSource},
                new EditionFeatureSettingCustom{Name = "Mobile Combustion", Icon = null, EditionId = GetEditionIdByName("Climatecamp Business Edition"), Type = (int)EditionFeartureType.EmissionSource},
                new EditionFeatureSettingCustom{Name = "Mobile Combustion", Icon = null, EditionId = GetEditionIdByName("Climatecamp Expense Partner Edition"), Type = (int)EditionFeartureType.EmissionSource},

                new EditionFeatureSettingCustom{Name = "Employee Commuting", Icon = null, EditionId = GetEditionIdByName("Climatecamp Enterprise+ Edition"), Type = (int)EditionFeartureType.EmissionSource},
                new EditionFeatureSettingCustom{Name = "Employee Commuting", Icon = null, EditionId = GetEditionIdByName("Climatecamp Enterprise Edition"), Type = (int)EditionFeartureType.EmissionSource},
                new EditionFeatureSettingCustom{Name = "Employee Commuting", Icon = null, EditionId = GetEditionIdByName("Climatecamp Business Edition"), Type = (int)EditionFeartureType.EmissionSource},
                new EditionFeatureSettingCustom{Name = "Employee Commuting", Icon = null, EditionId = GetEditionIdByName("Climatecamp Expense Partner Edition"), Type = (int)EditionFeartureType.EmissionSource},

                new EditionFeatureSettingCustom{Name = "Business Travel", Icon = null, EditionId = GetEditionIdByName("Climatecamp Enterprise+ Edition"), Type = (int)EditionFeartureType.EmissionSource},
                new EditionFeatureSettingCustom{Name = "Business Travel", Icon = null, EditionId = GetEditionIdByName("Climatecamp Enterprise Edition"), Type = (int)EditionFeartureType.EmissionSource},
                new EditionFeatureSettingCustom{Name = "Business Travel", Icon = null, EditionId = GetEditionIdByName("Climatecamp Business Edition"), Type = (int)EditionFeartureType.EmissionSource},
                new EditionFeatureSettingCustom{Name = "Business Travel", Icon = null, EditionId = GetEditionIdByName("Climatecamp Expense Partner Edition"), Type = (int)EditionFeartureType.EmissionSource},
	             #endregion
            };
        }
    }
}
