using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddRolesAndPasswordHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Permissions = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "Description", "IsActive", "Name", "Permissions", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 10, 2, 21, 40, 4, 512, DateTimeKind.Utc).AddTicks(9767), "Rol con permisos completos del sistema", true, "Administrador", "[\"users.create\", \"users.read\", \"users.update\", \"users.delete\", \"weather.create\", \"weather.read\", \"weather.update\", \"weather.delete\"]", null },
                    { 2, new DateTime(2025, 10, 2, 21, 40, 4, 512, DateTimeKind.Utc).AddTicks(9782), "Rol con permisos básicos del sistema", true, "Usuario", "[\"users.read\", \"weather.read\"]", null }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "RoleId" },
                values: new object[] { new DateTime(2025, 10, 2, 21, 40, 4, 831, DateTimeKind.Utc).AddTicks(9716), "$2a$11$z.mS7rxE2lXTZa/FLBRZQ.Mjb0ApUDKiZvGyRQzMwvjQQO/zZf4CC", 1 });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash", "RoleId" },
                values: new object[] { new DateTime(2025, 10, 2, 21, 40, 5, 185, DateTimeKind.Utc).AddTicks(7445), "$2a$11$xL.4FeqN2HV.2tvvAcERY./i/GL3XDUQ7.LNAWJzRkFaWuMfrPLSK", 2 });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "PasswordHash", "RoleId" },
                values: new object[] { new DateTime(2025, 10, 2, 21, 40, 5, 545, DateTimeKind.Utc).AddTicks(6474), "$2a$11$oAqO/zHXCIpmBrf.BmT3HuqNpfIPWfI03DmReZhYCBuS4HZG5HF4i", 2 });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 10, 2, 21, 40, 5, 545, DateTimeKind.Utc).AddTicks(7349), new DateOnly(2025, 10, 3) });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 10, 2, 21, 40, 5, 545, DateTimeKind.Utc).AddTicks(7377), new DateOnly(2025, 10, 4) });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 10, 2, 21, 40, 5, 545, DateTimeKind.Utc).AddTicks(7382), new DateOnly(2025, 10, 5) });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 10, 2, 21, 40, 5, 545, DateTimeKind.Utc).AddTicks(7387), new DateOnly(2025, 10, 6) });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 10, 2, 21, 40, 5, 545, DateTimeKind.Utc).AddTicks(7391), new DateOnly(2025, 10, 7) });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 10, 2, 21, 40, 5, 545, DateTimeKind.Utc).AddTicks(7412), new DateOnly(2025, 10, 8) });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 10, 2, 21, 40, 5, 545, DateTimeKind.Utc).AddTicks(7417), new DateOnly(2025, 10, 9) });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 10, 2, 21, 40, 5, 545, DateTimeKind.Utc).AddTicks(7489), new DateOnly(2025, 10, 10) });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 10, 2, 21, 40, 5, 545, DateTimeKind.Utc).AddTicks(7494), new DateOnly(2025, 10, 11) });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 10, 2, 21, 40, 5, 545, DateTimeKind.Utc).AddTicks(7503), new DateOnly(2025, 10, 12) });

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Users_RoleId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 25, 5, 40, 43, 805, DateTimeKind.Utc).AddTicks(1420));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 25, 5, 40, 43, 805, DateTimeKind.Utc).AddTicks(1426));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 25, 5, 40, 43, 805, DateTimeKind.Utc).AddTicks(1429));

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 9, 25, 5, 40, 43, 805, DateTimeKind.Utc).AddTicks(2054), new DateOnly(2025, 9, 26) });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 9, 25, 5, 40, 43, 805, DateTimeKind.Utc).AddTicks(2066), new DateOnly(2025, 9, 27) });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 9, 25, 5, 40, 43, 805, DateTimeKind.Utc).AddTicks(2069), new DateOnly(2025, 9, 28) });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 9, 25, 5, 40, 43, 805, DateTimeKind.Utc).AddTicks(2072), new DateOnly(2025, 9, 29) });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 9, 25, 5, 40, 43, 805, DateTimeKind.Utc).AddTicks(2075), new DateOnly(2025, 9, 30) });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 9, 25, 5, 40, 43, 805, DateTimeKind.Utc).AddTicks(2084), new DateOnly(2025, 10, 1) });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 9, 25, 5, 40, 43, 805, DateTimeKind.Utc).AddTicks(2087), new DateOnly(2025, 10, 2) });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 9, 25, 5, 40, 43, 805, DateTimeKind.Utc).AddTicks(2090), new DateOnly(2025, 10, 3) });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 9, 25, 5, 40, 43, 805, DateTimeKind.Utc).AddTicks(2093), new DateOnly(2025, 10, 4) });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 9, 25, 5, 40, 43, 805, DateTimeKind.Utc).AddTicks(2098), new DateOnly(2025, 10, 5) });
        }
    }
}
