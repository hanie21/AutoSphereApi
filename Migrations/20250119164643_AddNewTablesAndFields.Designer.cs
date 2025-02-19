﻿// <auto-generated />
using System;
using System.Collections.Generic;
using AutoSphere.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AutoSphere.Api.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250119164643_AddNewTablesAndFields")]
    partial class AddNewTablesAndFields
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AutoSphere.Api.Model.SavedSearch", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("SearchCriteria")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SearchName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("SavedSearches");
                });

            modelBuilder.Entity("AutoSphere.Api.Model.SearchHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("SearchCriteria")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("SearchedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("SearchHistories");
                });

            modelBuilder.Entity("AutoSphere.Api.Model.Vehicle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("BodyType")
                        .HasColumnType("text");

                    b.Property<string>("Condition")
                        .HasColumnType("text");

                    b.Property<string>("EngineType")
                        .HasColumnType("text");

                    b.Property<string>("ExteriorColor")
                        .IsRequired()
                        .HasColumnType("text");

                    b.PrimitiveCollection<List<string>>("Features")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<string>("FuelType")
                        .HasColumnType("text");

                    b.Property<bool>("HasAccidentHistory")
                        .HasColumnType("boolean");

                    b.Property<string>("InteriorColor")
                        .HasColumnType("text");

                    b.Property<string>("Location")
                        .HasColumnType("text");

                    b.Property<string>("Make")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Mileage")
                        .HasColumnType("integer");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("NumberOfOwners")
                        .HasColumnType("integer");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<bool>("ServiceHistoryVerified")
                        .HasColumnType("boolean");

                    b.Property<string>("Transmission")
                        .HasColumnType("text");

                    b.Property<int>("Year")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Make", "Model", "Year", "Price");

                    b.ToTable("Vehicles");
                });
#pragma warning restore 612, 618
        }
    }
}
