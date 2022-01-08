﻿using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using BF.WorldometerScraper.Application;
using BF.WorldometerScraper.Application.Abstractions;
using BF.WorldometerScraper.Data.Adapter;
using BF.WorldometerScraper.Domain.Enums;

using CommandLine;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using OpenQA.Selenium.Chrome;

namespace BF.WorldometerScraper.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            RegionType region = RegionType.All;
            Parser
                .Default
                .ParseArguments<Options>(args)
                .WithParsed(o =>
                {
                    if (o.Region != 0)
                    {
                        region = o.Region;
                    }
                });

            try
            {
                // Chrome Driver
                var chromeOptions = new ChromeOptions();
                chromeOptions.AddArguments("headless");
                var driver = new ChromeDriver("chrome", chromeOptions);

                // DB Context
                var connectionString = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build()
                    .GetConnectionString("DefaultConnection");
                var optionsBuilder = new DbContextOptionsBuilder<WorldometerScraperContext>();
                optionsBuilder.UseSqlite(connectionString);
                using var dbContext = new WorldometerScraperContext(optionsBuilder.Options);

                // Scrape and Save
                using IWorldometerService worldometerService = new WorldometerService(driver, dbContext);

                var dailyCasesPerCountry = worldometerService
                    .GetCountryDailyCases(region)
                    .ToList();

                await worldometerService.SaveCountryDailyCasesAsync(dailyCasesPerCountry);

                // Output to the console
                worldometerService.OutputToConsole(dailyCasesPerCountry);

                // Export CSV
                var now = dailyCasesPerCountry[0].Time;
                var exportedCSVFilename = $"export_{region}_{now:yy_MM_dd}.csv";
                var exportedCSVFolder = Path.Combine(Directory.GetCurrentDirectory(), "Exported");
                if (!Directory.Exists(exportedCSVFolder))
                {
                    Directory.CreateDirectory(exportedCSVFolder);
                }
                worldometerService.ExportInCSV(
                    Path.Combine(exportedCSVFolder, exportedCSVFilename),
                    dailyCasesPerCountry);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }

    public class Options
    {
        [Option("region", Required = false, HelpText = "Set region to filter. Can be one of these values: All, Europe, NorthAmerica, Asia, SouthAmerica, Africa, Oceania.")]
        public RegionType Region { get; set; }
    }
}
