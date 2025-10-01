using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeatherForecasts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    TemperatureC = table.Column<int>(type: "int", nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherForecasts", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "FirstName", "IsActive", "LastName", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 9, 25, 5, 40, 43, 805, DateTimeKind.Utc).AddTicks(1420), "juan.perez@email.com", "Juan", true, "Pérez", null },
                    { 2, new DateTime(2025, 9, 25, 5, 40, 43, 805, DateTimeKind.Utc).AddTicks(1426), "maria.gonzalez@email.com", "María", true, "González", null }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "FirstName", "LastName", "UpdatedAt" },
                values: new object[] { 3, new DateTime(2025, 9, 25, 5, 40, 43, 805, DateTimeKind.Utc).AddTicks(1429), "carlos.lopez@email.com", "Carlos", "López", null });

            migrationBuilder.InsertData(
                table: "WeatherForecasts",
                columns: new[] { "Id", "CreatedAt", "Date", "Summary", "TemperatureC", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 9, 25, 5, 40, 43, 805, DateTimeKind.Utc).AddTicks(2054), new DateOnly(2025, 9, 26), "Bracing", 30, null },
                    { 2, new DateTime(2025, 9, 25, 5, 40, 43, 805, DateTimeKind.Utc).AddTicks(2066), new DateOnly(2025, 9, 27), "Warm", -11, null },
                    { 3, new DateTime(2025, 9, 25, 5, 40, 43, 805, DateTimeKind.Utc).AddTicks(2069), new DateOnly(2025, 9, 28), "Chilly", -8, null },
                    { 4, new DateTime(2025, 9, 25, 5, 40, 43, 805, DateTimeKind.Utc).AddTicks(2072), new DateOnly(2025, 9, 29), "Warm", 34, null },
                    { 5, new DateTime(2025, 9, 25, 5, 40, 43, 805, DateTimeKind.Utc).AddTicks(2075), new DateOnly(2025, 9, 30), "Hot", -7, null },
                    { 6, new DateTime(2025, 9, 25, 5, 40, 43, 805, DateTimeKind.Utc).AddTicks(2084), new DateOnly(2025, 10, 1), "Chilly", -3, null },
                    { 7, new DateTime(2025, 9, 25, 5, 40, 43, 805, DateTimeKind.Utc).AddTicks(2087), new DateOnly(2025, 10, 2), "Cool", 17, null },
                    { 8, new DateTime(2025, 9, 25, 5, 40, 43, 805, DateTimeKind.Utc).AddTicks(2090), new DateOnly(2025, 10, 3), "Chilly", 8, null },
                    { 9, new DateTime(2025, 9, 25, 5, 40, 43, 805, DateTimeKind.Utc).AddTicks(2093), new DateOnly(2025, 10, 4), "Freezing", 18, null },
                    { 10, new DateTime(2025, 9, 25, 5, 40, 43, 805, DateTimeKind.Utc).AddTicks(2098), new DateOnly(2025, 10, 5), "Warm", 41, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "WeatherForecasts");
        }
    }
}
