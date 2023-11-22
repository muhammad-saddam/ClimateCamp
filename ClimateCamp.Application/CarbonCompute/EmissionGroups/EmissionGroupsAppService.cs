using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using ClimateCamp.CarbonCompute;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Index.HPRtree;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ClimateCamp.Application
{
    [AbpAuthorize]
    public class EmissionGroupsAppService : AsyncCrudAppService<EmissionGroups, EmissionGroupsDto, Guid, EmissionGroupsDto, CreateEmissionGroupDto, CreateEmissionGroupDto>, IEmissionGroupsAppService
    {
        private readonly IRepository<EmissionGroups, Guid> _emissionGroupsRepository;
        private readonly IRepository<ActivityData, Guid> _activityDataRepository;
        private readonly IRepository<EmissionsSource, int> _emissionSourceRepository;
        private readonly ILogger<EmissionGroupsAppService> _logger;

        /// <param name="emissionGroupsRepository"></param>
        /// <param name="activityDataRepository"></param>
        ///  /// <param name="emissionSourceRepository"></param>
        /// <param name="logger"></param>
        public EmissionGroupsAppService
            (
            IRepository<ClimateCamp.CarbonCompute.EmissionGroups, Guid> emissionGroupsRepository,
            IRepository<ClimateCamp.CarbonCompute.ActivityData, Guid> activityDataRepository,
            IRepository<ClimateCamp.CarbonCompute.EmissionsSource, int> emissionSourceRepository,
             ILogger<EmissionGroupsAppService> logger
            ) : base(emissionGroupsRepository)
        {
            _emissionGroupsRepository = emissionGroupsRepository;
            _activityDataRepository = activityDataRepository;
            _emissionSourceRepository = emissionSourceRepository;
            _logger = logger;
        }

        public override async Task<EmissionGroupsDto> CreateAsync(CreateEmissionGroupDto input)
        {
            try
            {
                var node = new EmissionGroupsDto();

                if(input.ParentEmissionGroupId != Guid.Empty) // handle case for all emissions
                {
                    var parentNode = await _emissionGroupsRepository.GetAll().Where(x => x.Id == input.ParentEmissionGroupId).SingleAsync();

                    var parentNodeActivityData = await _activityDataRepository.GetAll().Where(x => x.EmissionGroupId == parentNode.Id).ToListAsync();

                    if (input.EmissionSourceId != null && (parentNode.EmissionSourceId != null && parentNodeActivityData.Any())) // if child has emission source assigned and also parent has some emission source already assigned and also there is some activity data assigned to parent node
                        node = await CreateSelfAutoNode(input,parentNode);
                    else
                    {
                        parentNode = await _emissionGroupsRepository.GetAll().Where(x => x.Id == input.ParentEmissionGroupId).SingleAsync();

                        //Update parent node emission source if there is any emission source assigned already
                        if (parentNode.EmissionSourceId != null)
                        {
                            parentNode.EmissionSourceId = null;
                            await _emissionGroupsRepository.UpdateAsync(parentNode);
                        }
                        node = await base.CreateAsync(input);
                    }
                }
                else
                    node = await base.CreateAsync(input);

                await CurrentUnitOfWork.SaveChangesAsync();

                return node;
            }
            catch (Exception exception)
            {
                _logger.LogError($"AppService: EmissionGroupsAppService Method: CreateAsync - Exception: {exception}");
                return null;
            }
        }

        private async Task<EmissionGroupsDto> CreateSelfAutoNode(CreateEmissionGroupDto input, ClimateCamp.CarbonCompute.EmissionGroups parent)
        {
            var parentEmissionSourceId = parent.EmissionSourceId;

            //Update parent node emission source if there is any emission source assigned already
            if (parent.EmissionSourceId != null)
            {
                parent.EmissionSourceId = null;
                await _emissionGroupsRepository.UpdateAsync(parent);
            }

            //create new child node with parent attributes and emission source
            var newAutoNode = new ClimateCamp.CarbonCompute.EmissionGroups
            {
                Id = Guid.NewGuid(),
                Name = parent.Name,
                Icon = parent.Icon,
                OrganizationId = parent.OrganizationId,
                ParentEmissionGroupId = parent.Id,
                EmissionSourceId = parentEmissionSourceId
            };

            var newAutoNodeId = await _emissionGroupsRepository.InsertAndGetIdAsync(newAutoNode);

            var newNode = await _emissionGroupsRepository.InsertAsync(ObjectMapper.Map<ClimateCamp.CarbonCompute.EmissionGroups>(input));

            //check if activity data table has any references to the parent node

            var activityDataList = _activityDataRepository.GetAll().Where(x => x.EmissionGroupId == parent.Id).ToList();

            if (activityDataList.Any())
            {
                activityDataList.ForEach(async activityData =>
                {
                    activityData.EmissionGroupId = newAutoNodeId;

                    await _activityDataRepository.UpdateAsync(activityData);
                });
            }

            return ObjectMapper.Map<EmissionGroupsDto>(newNode);
        }

        public async Task<PagedResultDto<EmissionGroupsDto>> GetAllGroupedEmissionsData(Guid organizationId)
        {
            try
            {
                var emissionSources = await _emissionSourceRepository.GetAllListAsync();

                if (await _emissionGroupsRepository.FirstOrDefaultAsync(x => x.Name == "All Emissions" && x.OrganizationId == organizationId) == null)
                {
                    await _emissionGroupsRepository.InsertAsync(new ClimateCamp.CarbonCompute.EmissionGroups
                    {
                        Id = Guid.NewGuid(),
                        Name = "All Emissions",
                        Icon = "home",
                        OrganizationId = organizationId,
                        EmissionSourceId = null,
                        ParentEmissionGroupId = null,
                    });

                    await CurrentUnitOfWork.SaveChangesAsync();
                }

                var emissionsGroupedData = await _emissionGroupsRepository.GetAll()
                                            .Include(x => x.EmissionsSource)
                                            .Include(x => x.Children)
                                           .Where(x => x.OrganizationId == organizationId)
                                           .OrderBy(x => x.Name)
                                           .ToListAsync();

                var recursiveEmissionGroupsData = emissionsGroupedData.Where(x => x.ParentEmissionGroupId == null).ToList();

                var nodeTreeData = AssignAdditionalNodesDataRecursively(ObjectMapper.Map<List<EmissionGroupsDto>>(recursiveEmissionGroupsData), emissionSources);

                var result = new PagedResultDto<EmissionGroupsDto>()
                {
                    Items = nodeTreeData,
                    TotalCount = recursiveEmissionGroupsData.Count
                };

                return result;

            }
            catch (Exception exception)
            {
                _logger.LogError($"AppService: EmissionGroupsAppService Method: GetAllGroupedEmissionsData - Exception: {exception}");
                return null;
            }
        }

       public async Task<bool?> CheckIfEmissionGroupRelatedToAnyActivityData(Guid groupId)
        {
            try
            {
                return await _activityDataRepository.GetAll().Where(x => x.EmissionGroupId == groupId).AnyAsync(); 
            }
            catch (Exception exception)
            {
                _logger.LogError($"AppService: EmissionGroupsAppService Method: CheckIfEmissionGroupRelatedToAnyActivityData - Exception: {exception}");
                return null;
            }
        }

        public async Task InsertEmissionGroupsFromTemplate(string industryType, Guid organizationId)
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();

                var resource = Assembly.GetExecutingAssembly().GetManifestResourceNames().Single(x => x.Contains(industryType));

                using (var stream = assembly.GetManifestResourceStream(resource))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        string result = reader.ReadToEnd();

                        var emissionGroupsData = JsonConvert.DeserializeObject<EmissionGroupsVM[]>(result);

                        var allEmissionGroupNodeId = Guid.NewGuid();

                        foreach (var emissionGroupData in emissionGroupsData)
                        {
                            if (emissionGroupData.Name == "All Emissions")
                            {
                                var emissionGroup = new EmissionGroups
                                {
                                    Id = allEmissionGroupNodeId,
                                    Name = emissionGroupData.Name,
                                    Icon = emissionGroupData.Icon,
                                    EmissionSourceId = emissionGroupData.EmissionSourceId,
                                    OrganizationId = organizationId,
                                    ParentEmissionGroupId = null
                                };

                                await _emissionGroupsRepository.InsertAsync(emissionGroup);
                            }
                            else
                            {
                                if (emissionGroupData.Children.Any())
                                {
                                    var parentNodeId = Guid.NewGuid();

                                    // insert parent Node
                                    var emissionGroup = new EmissionGroups
                                    {
                                        Id = parentNodeId,
                                        Name = emissionGroupData.Name,
                                        Icon = emissionGroupData.Icon,
                                        EmissionSourceId = emissionGroupData.EmissionSourceId,
                                        OrganizationId = organizationId,
                                        ParentEmissionGroupId = allEmissionGroupNodeId
                                    };

                                    await _emissionGroupsRepository.InsertAsync(emissionGroup);

                                    await InsertChildNodesRecursively(emissionGroupData.Children, organizationId, parentNodeId);
                                }
                                else
                                {
                                    var emissionGroup = new EmissionGroups
                                    {
                                        Id = Guid.NewGuid(),
                                        Name = emissionGroupData.Name,
                                        Icon = emissionGroupData.Icon,
                                        EmissionSourceId = emissionGroupData.EmissionSourceId,
                                        OrganizationId = organizationId,
                                        ParentEmissionGroupId = allEmissionGroupNodeId
                                    };

                                    await _emissionGroupsRepository.InsertAsync(emissionGroup);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.LogError($"Method: InsertEmissionGroupsFromTemplate - Exception: {exception}");
            }
        }

        private async Task InsertChildNodesRecursively(ICollection<EmissionGroupsVM> children, Guid organizationId, Guid parentEmissionGroupId)
        {
            try
            {
                foreach (var child in children)
                {
                    // insert child Node
                    var emissionGroupChild = new EmissionGroups
                    {
                        Id = Guid.NewGuid(),
                        Name = child.Name,
                        Icon = child.Icon,
                        EmissionSourceId = child.EmissionSourceId,
                        OrganizationId = organizationId,
                        ParentEmissionGroupId = parentEmissionGroupId
                    };

                    await _emissionGroupsRepository.InsertAsync(emissionGroupChild);

                    if (child.Children.Any())
                        await InsertChildNodesRecursively(child.Children, organizationId, emissionGroupChild.Id);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError($"Method: InsertChildNodesRecursively - Exception: {exception}");
            }
        }

        private List<EmissionGroupsDto> AssignAdditionalNodesDataRecursively(List<EmissionGroupsDto> emissionGroups,List<EmissionsSource> emissionSources)
        {
            foreach (var emissionGroup in emissionGroups)
            {
                emissionGroup.Label = emissionGroup.Name;
                emissionGroup.Expanded = emissionGroup.Children.Any();
                emissionGroup.TreeSelectIcon = "pi pi-" + emissionGroup.Icon;  
                emissionGroup.Data = new Data
                {
                    Name = emissionGroup.Name,
                    Avatar = emissionGroup.Icon
                };
                emissionGroup.EmissionSourceId = emissionGroup.EmissionSourceId != null ? emissionSources.Single(x => x.Id == emissionGroup.EmissionSourceId).Id : null;
                emissionGroup.EmissionSourceName = emissionGroup.EmissionSourceId != null ? emissionSources.Single(x => x.Id == emissionGroup.EmissionSourceId).Name : null;  
                if (emissionGroup.Children.Count > 0)
                    AssignAdditionalNodesDataRecursively(emissionGroup.Children.ToList(), emissionSources);
            }

            return emissionGroups;
        }
        public override async Task DeleteAsync(EntityDto<Guid> input)
        {
            try
            {
                // Hard delete the soft delete activity data related to the current EmissionGroupId
                var activityDataToDelete = await _activityDataRepository.GetAll().Where(x => x.EmissionGroupId == input.Id).IgnoreQueryFilters().ToListAsync();
                activityDataToDelete.ForEach(async activityData =>
                {
                    await _activityDataRepository.HardDeleteAsync(activityData);
                });

                // Hard delete the EmissionGroup
                var emissionGroup = await _emissionGroupsRepository.GetAsync(input.Id);
                if (emissionGroup == null)
                {
                    throw new InvalidOperationException("EmissionGroup does not exist.");
                }
                await _emissionGroupsRepository.DeleteAsync(input.Id);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Method: DeleteAsync - Exception: {exception}");
            }
        }
    }
}
