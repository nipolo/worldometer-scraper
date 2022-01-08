using System;

using BF.WorldometerScraper.Domain.Enums;

namespace BF.WorldometerScraper.Data.States
{
    public class CountryDailyCases
    {
        public string CountryName { get; set; }

        public DateTime Time { get; set; }

        public RegionType Region { get; set; }

        public string ActiveCases { get; set; }

        public string TotalCases { get; set; }

        public string TotalTests { get; set; }
    }
}
