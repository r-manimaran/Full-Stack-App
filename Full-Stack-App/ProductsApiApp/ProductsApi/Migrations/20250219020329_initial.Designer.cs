﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ProductsApi.Data;

#nullable disable

namespace ProductsApi.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250219020329_initial")]
    partial class initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("public")
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ProductsApi.Models.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<DateTime?>("UpdatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Products", "public");
                });

            modelBuilder.Entity("ProductsApi.Models.Product", b =>
                {
                    b.OwnsOne("ProductsApi.Models.ProductDetails", "Details", b1 =>
                        {
                            b1.Property<Guid>("ProductId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Brand")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)");

                            b1.Property<string>("Category")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)");

                            b1.Property<string>("CountryOfOrigin")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)");

                            b1.Property<string>("Manufacturer")
                                .IsRequired()
                                .HasMaxLength(200)
                                .HasColumnType("character varying(200)");

                            b1.Property<string>("SubCategory")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)");

                            b1.PrimitiveCollection<List<string>>("Tags")
                                .IsRequired()
                                .HasColumnType("jsonb");

                            b1.HasKey("ProductId");

                            b1.ToTable("Products", "public");

                            b1.WithOwner()
                                .HasForeignKey("ProductId");
                        });

                    b.OwnsOne("ProductsApi.Models.ProductInventory", "Inventory", b1 =>
                        {
                            b1.Property<Guid>("ProductId")
                                .HasColumnType("uuid");

                            b1.Property<int>("ReorderPoint")
                                .HasColumnType("integer");

                            b1.Property<string>("Sku")
                                .IsRequired()
                                .HasMaxLength(20)
                                .HasColumnType("character varying(20)");

                            b1.Property<int>("StockQuanitity")
                                .HasColumnType("integer");

                            b1.Property<string>("WarehouseLocation")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)");

                            b1.HasKey("ProductId");

                            b1.ToTable("Products", "public");

                            b1.WithOwner()
                                .HasForeignKey("ProductId");
                        });

                    b.OwnsOne("ProductsApi.Models.ProductPricing", "Pricing", b1 =>
                        {
                            b1.Property<Guid>("ProductId")
                                .HasColumnType("uuid");

                            b1.Property<decimal>("BasePrice")
                                .HasColumnType("decimal(18,2)");

                            b1.Property<string>("Currency")
                                .IsRequired()
                                .HasMaxLength(3)
                                .HasColumnType("character varying(3)");

                            b1.Property<decimal>("DiscountedPrice")
                                .HasColumnType("decimal(18,2)");

                            b1.Property<bool>("IsOnSale")
                                .HasColumnType("boolean");

                            b1.Property<DateTime?>("SaleEndAt")
                                .HasColumnType("timestamp with time zone");

                            b1.HasKey("ProductId");

                            b1.ToTable("Products", "public");

                            b1.WithOwner()
                                .HasForeignKey("ProductId");
                        });

                    b.OwnsOne("ProductsApi.Models.ProductSpecifications", "Specifications", b1 =>
                        {
                            b1.Property<Guid>("ProductId")
                                .HasColumnType("uuid");

                            b1.Property<Dictionary<string, string>>("Dimensions")
                                .IsRequired()
                                .HasColumnType("jsonb");

                            b1.PrimitiveCollection<List<string>>("Materials")
                                .IsRequired()
                                .HasColumnType("jsonb");

                            b1.Property<Dictionary<string, string>>("TechnicalSpecs")
                                .IsRequired()
                                .HasColumnType("jsonb");

                            b1.Property<decimal>("WeightInKg")
                                .HasColumnType("decimal(10, 2)");

                            b1.HasKey("ProductId");

                            b1.ToTable("Products", "public");

                            b1.WithOwner()
                                .HasForeignKey("ProductId");
                        });

                    b.Navigation("Details")
                        .IsRequired();

                    b.Navigation("Inventory")
                        .IsRequired();

                    b.Navigation("Pricing")
                        .IsRequired();

                    b.Navigation("Specifications")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
