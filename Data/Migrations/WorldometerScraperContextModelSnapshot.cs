﻿// <auto-generated />
using System;
using BF.WorldometerScraper.Data.Adapter;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BF.WorldometerScraper.Data.Migrations
{
    [DbContext(typeof(WorldometerScraperContext))]
    partial class WorldometerScraperContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.13");

            modelBuilder.Entity("BF.WorldometerScraper.Data.States.CountryDailyCases", b =>
                {
                    b.Property<string>("CountryName")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Time")
                        .HasColumnType("TEXT");

                    b.Property<string>("ActiveCases")
                        .HasColumnType("TEXT");

                    b.Property<int>("Region")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TotalCases")
                        .HasColumnType("TEXT");

                    b.Property<string>("TotalTests")
                        .HasColumnType("TEXT");

                    b.HasKey("CountryName", "Time");

                    b.ToTable("CountryDailyCases");
                });
#pragma warning restore 612, 618
        }
    }
}
