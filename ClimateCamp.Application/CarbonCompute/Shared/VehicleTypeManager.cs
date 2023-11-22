using Abp.Domain.Repositories;
using ClimateCamp.CarbonCompute;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClimateCamp.Application.CarbonCompute
{
    public class VehicleTypeManager
    {
        private readonly IRepository<VehicleType, Guid> _vehicleTypeRepository;
        private readonly ILogger<VehicleTypeManager> _logger;

        public VehicleTypeManager(IRepository<VehicleType, Guid> vehicleTypeRepository)
        {
            _vehicleTypeRepository = vehicleTypeRepository;
        }
        public async Task<List<VehicleTypeGroup>> GetGroupedVehicleTypes()
        {
            try
            {

                List<VehicleTypeGroup> VehicleTypeGroupList = new List<VehicleTypeGroup>();

                List<GHG.ModeOfTransport> ModeOfTransportList = Enum.GetValues(typeof(GHG.ModeOfTransport)).Cast<GHG.ModeOfTransport>().ToList();

                List<VehicleType> VehicleTypeList = _vehicleTypeRepository.GetAll().ToList();

                foreach (var ModeOfTransport in ModeOfTransportList)
                {
                    VehicleTypeGroup model = new VehicleTypeGroup();
                    model.label = ModeOfTransport.ToString();
                    model.data = (int)ModeOfTransport;
                    model.expandedIcon = "pi pi-folder-open";
                    model.collapsedIcon = "pi pi-folder";
                    model.Children = VehicleTypeList.Any(x => x.ModeOfTransport == (int)ModeOfTransport) ?
                                     VehicleTypeList.Where(x => x.ModeOfTransport == (int)ModeOfTransport)
                                     .Select(x => new Child { data = x.Id, label = x.Name }).OrderBy(x => x.label)
                                     .ToList() : new List<Child>();
                    if (model.Children.Count > 0)
                    {
                        VehicleTypeGroupList.Add(model);
                    }

                }

                return VehicleTypeGroupList;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Method: GetGroupedVehicleTypes - Exception: {ex}");
                return null;
            }
        }
    }
}
    