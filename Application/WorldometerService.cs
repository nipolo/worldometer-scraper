using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

using BF.WorldometerScraper.Application.Abstractions;
using BF.WorldometerScraper.Data.Adapter;
using BF.WorldometerScraper.Data.States;
using BF.WorldometerScraper.Domain.Enums;

using CsvHelper;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace BF.WorldometerScraper.Application
{
    public class WorldometerService : IWorldometerService, IDisposable
    {
        private readonly ChromeDriver _driver;
        private readonly WorldometerScraperContext _dbContext;
        private readonly WebDriverWait _wait;

        public WorldometerService(
            ChromeDriver driver,
            WorldometerScraperContext dbContext)
        {
            _driver = driver;
            _dbContext = dbContext;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        public void Dispose()
        {
            _driver.Dispose();
        }

        public IEnumerable<CountryDailyCases> GetCountryDailyCases(RegionType region)
        {
            _driver.Navigate().GoToUrl("https://www.worldometers.info/coronavirus/");

            var regionNavigationButton = _wait.Until(e =>
                        e.FindElement(
                            By.CssSelector($"nav#ctabstoday #{GetHtmlNavigationButtonId(region)}>a")));

            _driver.ExecuteScript("arguments[0].scrollIntoView();", regionNavigationButton);
            regionNavigationButton.Click();

            var now = DateTime.Now;
            var countryCasesRows = _wait.Until(e =>
                        e.FindElements(
                            By.CssSelector("#main_table_countries_today>tbody>tr:not(:first-child):not([style*='display: none'])[role=row]")));

            foreach (var countryCasesRow in countryCasesRows)
            {
                var searchResultColumns = countryCasesRow.FindElements(By.CssSelector("td"));
                var activeCases = searchResultColumns[8].Text.Replace(",", "");
                var totalCases = searchResultColumns[2].Text.Replace(",", "");
                var totalTests = searchResultColumns[12].Text.Replace(",", "");
                string countryName;
                try
                {
                    countryName = searchResultColumns[1].FindElement(By.CssSelector("a")).Text;
                }
                catch
                {
                    try
                    {
                        countryName = searchResultColumns[1].FindElement(By.CssSelector("span")).Text;
                    }
                    catch
                    {
                        countryName = searchResultColumns[1].Text;
                    }
                }
                yield return new CountryDailyCases()
                {
                    ActiveCases = activeCases,
                    CountryName = countryName,
                    Region = region,
                    Time = now,
                    TotalCases = totalCases,
                    TotalTests = totalTests
                };
            }
        }

        public void OutputToConsole(IEnumerable<CountryDailyCases> dailyCasesPerCountry)
        {
            Console.WriteLine(new string('-', 80));
            Console.WriteLine(
                    string.Format("|{0,30}|{1,15}|{2,15}|{3,15}|",
                    "Country Name",
                    "Active Cases",
                    "Total Cases",
                    "Total Tests"
                    ));
            Console.WriteLine(new string('-', 80));

            foreach (var countryDailyCases in dailyCasesPerCountry)
            {
                Console.WriteLine(
                    string.Format("|{0,30}|{1,15}|{2,15}|{3,15}|",
                    countryDailyCases.CountryName,
                    countryDailyCases.ActiveCases,
                    countryDailyCases.TotalCases,
                    countryDailyCases.TotalTests
                    ));
            }
            Console.WriteLine(new string('-', 80));
        }

        public async Task SaveCountryDailyCasesAsync(IEnumerable<CountryDailyCases> dailyCasesPerCountry)
        {
            _dbContext.AddRange(dailyCasesPerCountry);

            await _dbContext.SaveChangesAsync();
        }

        public void ExportInCSV(string location, IEnumerable<CountryDailyCases> dailyCasesPerCountry)
        {
            using var csvFileStream = new StreamWriter(location);
            using var csv = new CsvWriter(csvFileStream, new CultureInfo("en-US"));

            csv.WriteRecords(dailyCasesPerCountry);
        }

        private static string GetHtmlNavigationButtonId(RegionType region)
        {
            return region switch
            {
                RegionType.Africa => "nav-africa-tab",
                RegionType.Asia => "nav-asia-tab",
                RegionType.Europe => "nav-europe-tab",
                RegionType.NorthAmerica => "nav-na-tab",
                RegionType.Oceania => "nav-oceania-tab",
                RegionType.SouthAmerica => "nav-sa-tab",
                _ => "nav-all-tab"
            };
        }
    }
}
