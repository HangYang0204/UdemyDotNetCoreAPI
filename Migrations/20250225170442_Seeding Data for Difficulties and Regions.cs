using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NZWalks.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedingDataforDifficultiesandRegions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Difficulties",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("6278ab91-3902-4abd-8299-3cc108b23077"), "Hard" },
                    { new Guid("9fe7d108-36bb-45df-baef-49416ec73aee"), "Easy" },
                    { new Guid("c9a7b395-1447-46cc-832e-b22127126c01"), "Medium" }
                });

            migrationBuilder.InsertData(
                table: "Regions",
                columns: new[] { "Id", "Code", "Name", "RegionImageUrl" },
                values: new object[,]
                {
                    { new Guid("12686546-0ffa-4828-b0d5-272784554c33"), "ON", "Ontario", "" },
                    { new Guid("987bf8c2-1401-4313-998b-249fe6244396"), "AB", "Alberta", "" },
                    { new Guid("c10712b0-f9f8-4d11-8185-78e18b5f65da"), "BC", "British Columbia", "" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("6278ab91-3902-4abd-8299-3cc108b23077"));

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("9fe7d108-36bb-45df-baef-49416ec73aee"));

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("c9a7b395-1447-46cc-832e-b22127126c01"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("12686546-0ffa-4828-b0d5-272784554c33"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("987bf8c2-1401-4313-998b-249fe6244396"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("c10712b0-f9f8-4d11-8185-78e18b5f65da"));
        }
    }
}
