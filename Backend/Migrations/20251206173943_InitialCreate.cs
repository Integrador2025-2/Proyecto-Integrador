using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recursos_Rubro_RubroId",
                table: "Recursos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rubro",
                table: "Rubro");

            migrationBuilder.RenameTable(
                name: "Rubro",
                newName: "Rubros");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rubros",
                table: "Rubros",
                column: "RubroId");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 6, 17, 39, 42, 299, DateTimeKind.Utc).AddTicks(2766));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 6, 17, 39, 42, 299, DateTimeKind.Utc).AddTicks(2770));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 12, 6, 17, 39, 42, 460, DateTimeKind.Utc).AddTicks(5066), "$2a$11$7Wf.ZNhsxAehoc1oXTD5pOLrww28o17rl1bz1j14ZucOGezfpM6Hm" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 12, 6, 17, 39, 42, 632, DateTimeKind.Utc).AddTicks(4858), "$2a$11$lxAOAPDlCCffG1fv92h2u.wcRuyXDbMkZ1UkJgKeEZpa3Sox5BLWW" });

            migrationBuilder.AddForeignKey(
                name: "FK_Recursos_Rubros_RubroId",
                table: "Recursos",
                column: "RubroId",
                principalTable: "Rubros",
                principalColumn: "RubroId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recursos_Rubros_RubroId",
                table: "Recursos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rubros",
                table: "Rubros");

            migrationBuilder.RenameTable(
                name: "Rubros",
                newName: "Rubro");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rubro",
                table: "Rubro",
                column: "RubroId");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 6, 1, 36, 48, 170, DateTimeKind.Utc).AddTicks(6495));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 6, 1, 36, 48, 170, DateTimeKind.Utc).AddTicks(6498));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 11, 6, 1, 36, 48, 288, DateTimeKind.Utc).AddTicks(333), "$2a$11$R3ri5/ExUlHfWM83YHxwN.7R8OLsE1TFsLvWYDquRKAEDt7He8dAO" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 11, 6, 1, 36, 48, 406, DateTimeKind.Utc).AddTicks(2145), "$2a$11$evROIYC4kMk5mF/HxldmTOpnuTmSBgkCIzkbKt5n3V08y0UTLKza2" });

            migrationBuilder.AddForeignKey(
                name: "FK_Recursos_Rubro_RubroId",
                table: "Recursos",
                column: "RubroId",
                principalTable: "Rubro",
                principalColumn: "RubroId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
