using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductsApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Details_Brand = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Details_Category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Details_SubCategory = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Details_Manufacturer = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Details_CountryOfOrigin = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Details_Tags = table.Column<List<string>>(type: "jsonb", nullable: false),
                    Pricing_BasePrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Pricing_DiscountedPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    Pricing_Currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Pricing_IsOnSale = table.Column<bool>(type: "boolean", nullable: false),
                    Pricing_SaleEndAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Inventory_StockQuanitity = table.Column<int>(type: "integer", nullable: false),
                    Inventory_Sku = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Inventory_WarehouseLocation = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Inventory_ReorderPoint = table.Column<int>(type: "integer", nullable: false),
                    Specifications_Dimensions = table.Column<Dictionary<string, string>>(type: "jsonb", nullable: false),
                    Specifications_WeightInKg = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    Specifications_Materials = table.Column<List<string>>(type: "jsonb", nullable: false),
                    Specifications_TechnicalSpecs = table.Column<Dictionary<string, string>>(type: "jsonb", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products",
                schema: "public");
        }
    }
}
