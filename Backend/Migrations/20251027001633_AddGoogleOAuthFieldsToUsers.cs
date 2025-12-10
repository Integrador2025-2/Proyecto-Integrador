using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddGoogleOAuthFieldsToUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 27, 0, 16, 33, 43, DateTimeKind.Utc).AddTicks(4096));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 27, 0, 16, 33, 43, DateTimeKind.Utc).AddTicks(4104));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 10, 27, 0, 16, 33, 167, DateTimeKind.Utc).AddTicks(5607), "$2a$11$4odUBHFRxmGmya/1mGMmvOeDtqIHu6/nEUfzLI7zXgH7QWFkLUTs2" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 10, 27, 0, 16, 33, 290, DateTimeKind.Utc).AddTicks(547), "$2a$11$0sj/.t6SERwYShldcpjeCOV5wnmmrYV.Lp2gBeszIRUhdM6Sdmgxm" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 10, 27, 0, 16, 33, 411, DateTimeKind.Utc).AddTicks(8893), "$2a$11$eqzR36cgL.hKoFDWzcbYsuy3wAdP4XHZRAOk06TxoYrhPn2vZNq2." });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 10, 27, 0, 16, 33, 411, DateTimeKind.Utc).AddTicks(9247), new DateOnly(2025, 10, 27) });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 10, 27, 0, 16, 33, 411, DateTimeKind.Utc).AddTicks(9254), new DateOnly(2025, 10, 28) });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 10, 27, 0, 16, 33, 411, DateTimeKind.Utc).AddTicks(9256), new DateOnly(2025, 10, 29) });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 10, 27, 0, 16, 33, 411, DateTimeKind.Utc).AddTicks(9259), new DateOnly(2025, 10, 30) });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 10, 27, 0, 16, 33, 411, DateTimeKind.Utc).AddTicks(9261), new DateOnly(2025, 10, 31) });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 10, 27, 0, 16, 33, 411, DateTimeKind.Utc).AddTicks(9271), new DateOnly(2025, 11, 1) });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 10, 27, 0, 16, 33, 411, DateTimeKind.Utc).AddTicks(9273), new DateOnly(2025, 11, 2) });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 10, 27, 0, 16, 33, 411, DateTimeKind.Utc).AddTicks(9275), new DateOnly(2025, 11, 3) });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 10, 27, 0, 16, 33, 411, DateTimeKind.Utc).AddTicks(9277), new DateOnly(2025, 11, 4) });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 10, 27, 0, 16, 33, 411, DateTimeKind.Utc).AddTicks(9280), new DateOnly(2025, 11, 5) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 3, 46, 20, 699, DateTimeKind.Utc).AddTicks(6154));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 3, 46, 20, 699, DateTimeKind.Utc).AddTicks(6158));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 10, 16, 3, 46, 20, 819, DateTimeKind.Utc).AddTicks(3993), "$2a$11$xY0/3VJnZriSqb4bwYwtVerjBplO1vhBN1yuw8v5ZKjSF0/JUhJja" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 10, 16, 3, 46, 20, 941, DateTimeKind.Utc).AddTicks(9608), "$2a$11$doH/D3dtT4fiEF35PHxS2u3ZJPIP5QoB0OStAYdXd5b.hhe1tUXcC" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 10, 16, 3, 46, 21, 63, DateTimeKind.Utc).AddTicks(8992), "$2a$11$M0O.vH8dmyeTLVLTHU.HBugXnNQzlkeOg9T8Cd.N.mQ8qlWOQ0Hvi" });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 10, 16, 3, 46, 21, 63, DateTimeKind.Utc).AddTicks(9378), new DateOnly(2025, 10, 16) });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 10, 16, 3, 46, 21, 63, DateTimeKind.Utc).AddTicks(9390), new DateOnly(2025, 10, 17) });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 10, 16, 3, 46, 21, 63, DateTimeKind.Utc).AddTicks(9391), new DateOnly(2025, 10, 18) });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 10, 16, 3, 46, 21, 63, DateTimeKind.Utc).AddTicks(9392), new DateOnly(2025, 10, 19) });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 10, 16, 3, 46, 21, 63, DateTimeKind.Utc).AddTicks(9393), new DateOnly(2025, 10, 20) });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 10, 16, 3, 46, 21, 63, DateTimeKind.Utc).AddTicks(9403), new DateOnly(2025, 10, 21) });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 10, 16, 3, 46, 21, 63, DateTimeKind.Utc).AddTicks(9404), new DateOnly(2025, 10, 22) });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 10, 16, 3, 46, 21, 63, DateTimeKind.Utc).AddTicks(9405), new DateOnly(2025, 10, 23) });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 10, 16, 3, 46, 21, 63, DateTimeKind.Utc).AddTicks(9406), new DateOnly(2025, 10, 24) });

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "Date" },
                values: new object[] { new DateTime(2025, 10, 16, 3, 46, 21, 63, DateTimeKind.Utc).AddTicks(9408), new DateOnly(2025, 10, 25) });
        }
    }
}
