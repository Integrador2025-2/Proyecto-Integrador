using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddEntidadAndActXEntidad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Entidad",
                columns: table => new
                {
                    EntidadId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entidad", x => x.EntidadId);
                });

            migrationBuilder.CreateTable(
                name: "ActXEntidades",
                columns: table => new
                {
                    ActXEntidadId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActividadId = table.Column<int>(type: "int", nullable: false),
                    EntidadId = table.Column<int>(type: "int", nullable: false),
                    Efectivo = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Especie = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActXEntidades", x => x.ActXEntidadId);
                    table.ForeignKey(
                        name: "FK_ActXEntidades_Actividades_ActividadId",
                        column: x => x.ActividadId,
                        principalTable: "Actividades",
                        principalColumn: "ActividadId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActXEntidades_Entidad_EntidadId",
                        column: x => x.EntidadId,
                        principalTable: "Entidad",
                        principalColumn: "EntidadId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 1, 25, 21, 590, DateTimeKind.Utc).AddTicks(4579));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 1, 25, 21, 590, DateTimeKind.Utc).AddTicks(4585));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 10, 16, 1, 25, 21, 715, DateTimeKind.Utc).AddTicks(2569), "$2a$11$CpDuebgC7CL7gqNqc.N0XO3qEOvC3GWH7Q/7w9K65Yk42l9TT/5LC" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 10, 16, 1, 25, 21, 840, DateTimeKind.Utc).AddTicks(6675), "$2a$11$YzCmqmPuq4hmsofPZaV0o.iIiGXFv8.Q.O56Cg1n80UsCx.TxjtyC" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 10, 16, 1, 25, 21, 964, DateTimeKind.Utc).AddTicks(2975), "$2a$11$W/ZEyoh9LKd3MF1lcj1JWOMLwKcZEsyigWr4ieeuQAJPIY34NUAkW" });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 10, 16, 1, 25, 21, 964, DateTimeKind.Utc).AddTicks(3399), new DateOnly(2025, 10, 16) });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 10, 16, 1, 25, 21, 964, DateTimeKind.Utc).AddTicks(3413), new DateOnly(2025, 10, 17) });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 10, 16, 1, 25, 21, 964, DateTimeKind.Utc).AddTicks(3415), new DateOnly(2025, 10, 18) });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 10, 16, 1, 25, 21, 964, DateTimeKind.Utc).AddTicks(3416), new DateOnly(2025, 10, 19) });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 10, 16, 1, 25, 21, 964, DateTimeKind.Utc).AddTicks(3417), new DateOnly(2025, 10, 20) });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 10, 16, 1, 25, 21, 964, DateTimeKind.Utc).AddTicks(3427), new DateOnly(2025, 10, 21) });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 10, 16, 1, 25, 21, 964, DateTimeKind.Utc).AddTicks(3428), new DateOnly(2025, 10, 22) });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 10, 16, 1, 25, 21, 964, DateTimeKind.Utc).AddTicks(3429), new DateOnly(2025, 10, 23) });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 10, 16, 1, 25, 21, 964, DateTimeKind.Utc).AddTicks(3430), new DateOnly(2025, 10, 24) });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 10, 16, 1, 25, 21, 964, DateTimeKind.Utc).AddTicks(3432), new DateOnly(2025, 10, 25) });

            migrationBuilder.CreateIndex(
                name: "IX_ActXEntidades_ActividadId",
                table: "ActXEntidades",
                column: "ActividadId");

            migrationBuilder.CreateIndex(
                name: "IX_ActXEntidades_EntidadId",
                table: "ActXEntidades",
                column: "EntidadId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActXEntidades");

            migrationBuilder.DropTable(
                name: "Entidad");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 11, 1, 57, 2, 325, DateTimeKind.Utc).AddTicks(6065));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 11, 1, 57, 2, 325, DateTimeKind.Utc).AddTicks(6071));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 10, 11, 1, 57, 2, 512, DateTimeKind.Utc).AddTicks(5419), "$2a$11$eqaL2AaBqoAVQZWpdgs/iegNjoBSR.iOg95Gev0Pj1LNHGMRBBvXO" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 10, 11, 1, 57, 2, 695, DateTimeKind.Utc).AddTicks(7772), "$2a$11$pVYOnfnDCbcjg3lvnHq9reXVAZe9ZbXQCW3Qkq5OWZm0XLBYMjcPS" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 10, 11, 1, 57, 2, 894, DateTimeKind.Utc).AddTicks(7013), "$2a$11$Ijifh3yVXkOsFUH8mQwrIu6h7iyoRE6WUrs2/NALEyxOmOsVl4DO6" });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 10, 11, 1, 57, 2, 894, DateTimeKind.Utc).AddTicks(7544), new DateOnly(2025, 10, 11) });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 10, 11, 1, 57, 2, 894, DateTimeKind.Utc).AddTicks(7555), new DateOnly(2025, 10, 12) });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 10, 11, 1, 57, 2, 894, DateTimeKind.Utc).AddTicks(7558), new DateOnly(2025, 10, 13) });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 10, 11, 1, 57, 2, 894, DateTimeKind.Utc).AddTicks(7561), new DateOnly(2025, 10, 14) });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 10, 11, 1, 57, 2, 894, DateTimeKind.Utc).AddTicks(7563), new DateOnly(2025, 10, 15) });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 10, 11, 1, 57, 2, 894, DateTimeKind.Utc).AddTicks(7577), new DateOnly(2025, 10, 16) });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 10, 11, 1, 57, 2, 894, DateTimeKind.Utc).AddTicks(7579), new DateOnly(2025, 10, 17) });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 10, 11, 1, 57, 2, 894, DateTimeKind.Utc).AddTicks(7582), new DateOnly(2025, 10, 18) });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 10, 11, 1, 57, 2, 894, DateTimeKind.Utc).AddTicks(7585), new DateOnly(2025, 10, 19) });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 10, 11, 1, 57, 2, 894, DateTimeKind.Utc).AddTicks(7589), new DateOnly(2025, 10, 20) });
        }
    }
}
