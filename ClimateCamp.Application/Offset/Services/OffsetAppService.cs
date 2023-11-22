using Abp.Application.Services;
using Abp.Domain.Repositories;
using ClimateCamp.Core;
using System;

namespace ClimateCamp.Application
{
    public class OffsetAppService : AsyncCrudAppService<Offset, OffsetDto, Guid, PagedOffsetResponseDto, CreateOffsetDto, CreateOffsetDto>, IOffsetAppService
    {
        private readonly IRepository<Offset, Guid> _offsetRepository;

        public OffsetAppService(
            IRepository<Offset, Guid> offsetRepository) : base(offsetRepository)
        {
            _offsetRepository = offsetRepository;
        }

    }
}
