using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using BF.WorldometerScraper.Data.States;
using BF.WorldometerScraper.Domain.Enums;

namespace BF.WorldometerScraper.Application.Abstractions
{
    public interface IWorldometerService : IDisposable
    {
        public IEnumerable<CountryDailyCases> GetCountryDailyCases(RegionType region);

        public void ExportInCSV(string location, IEnumerable<CountryDailyCases> dailyCasesPerCountry);

        public void OutputToConsole(IEnumerable<CountryDailyCases> dailyCasesPerCountry);

        public Task SaveCountryDailyCasesAsync(IEnumerable<CountryDailyCases> dailyCasesPerCountry);
    }
}
