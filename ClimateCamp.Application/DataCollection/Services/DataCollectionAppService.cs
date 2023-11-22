using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using ClimateCamp.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClimateCamp.Application
{
    [AbpAuthorize]
    public class DataCollectionAppService : AsyncCrudAppService<DataCollection, DataCollectionDto, Guid, PagedDataCollectionResultRequestDto, CreateDataCollectionDto, CreateDataCollectionDto>, IDataCollectionAppService
    {
        private readonly IRepository<DataCollection, Guid> _dataCollectionRepository;

        public DataCollectionAppService(
            IRepository<DataCollection, Guid> dataCollectionRepository) : base(dataCollectionRepository)
        {
            _dataCollectionRepository = dataCollectionRepository;
        }

        public async Task<PagedResultDto<DataCollectionDto>> GetAllByOrganizationIdAsync(GetAllDataCollectionsByOrganizationId input)
        {
            List<DataCollection> dataCollections = new List<DataCollection>();
            if (input.OrganizationId != Guid.Empty)
            {
                dataCollections = await _dataCollectionRepository.GetAll().Where(t => t.OrganizationId == input.OrganizationId).ToListAsync();
            }
            else
                dataCollections = await _dataCollectionRepository.GetAll().ToListAsync();

            var result = new PagedResultDto<DataCollectionDto>()
            {
                Items = ObjectMapper.Map<List<DataCollectionDto>>(dataCollections),
                TotalCount = dataCollections.Count
            };
            return result;
        }

    }
}
