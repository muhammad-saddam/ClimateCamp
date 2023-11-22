using Abp.Application.Services;
using Abp.Domain.Repositories;
using ClimateCamp.Common.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClimateCamp.Lookup
{
    public class CountryAppService : ApplicationService, ICountryAppService
    {
        private readonly IRepository<Country, int> _countryRepository;

        public CountryAppService(
            IRepository<Country, int> countryRepository)
        {
            _countryRepository = countryRepository ?? throw new ArgumentNullException(nameof(countryRepository));

        }
        public async Task<List<DDLDto<int>>> GetCountriesDropDown()
        {
            var countries = await _countryRepository.GetAll().Select(t => new DDLDto<int>() { Id = t.Id, Name = t.Name, DisplayName = t.TwoCharCode }).ToListAsync();
            return countries;
        }
    }
}
