using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddTareasTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                BEGIN TRY
                    IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_ActXEntidades_Actividades_ActividadId')
                        ALTER TABLE [ActXEntidades] DROP CONSTRAINT [FK_ActXEntidades_Actividades_ActividadId];

                    IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_ActXEntidades_Entidad_EntidadId')
                        ALTER TABLE [ActXEntidades] DROP CONSTRAINT [FK_ActXEntidades_Entidad_EntidadId];

                    IF EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = 'PK_ActXEntidades')
                        ALTER TABLE [ActXEntidades] DROP CONSTRAINT [PK_ActXEntidades];

                    IF EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = 'PK_Entidad')
                        ALTER TABLE [Entidad] DROP CONSTRAINT [PK_Entidad];

                    IF OBJECT_ID('dbo.ActXEntidades','U') IS NOT NULL AND OBJECT_ID('dbo.ActxEntidades','U') IS NULL
                        EXEC sp_rename 'ActXEntidades', 'ActxEntidades';

                    IF OBJECT_ID('dbo.Entidad','U') IS NOT NULL AND OBJECT_ID('dbo.Entidades','U') IS NULL
                        EXEC sp_rename 'Entidad', 'Entidades';

                    IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('ActxEntidades') AND name = 'EntidadId')
                        EXEC sp_rename 'ActxEntidades.EntidadId','entidad_id','COLUMN';

                    IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('ActxEntidades') AND name = 'ActividadId')
                        EXEC sp_rename 'ActxEntidades.ActividadId','act_id','COLUMN';

                    IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('ActxEntidades') AND name = 'ActXEntidadId')
                        EXEC sp_rename 'ActxEntidades.ActXEntidadId','Id','COLUMN';

                    IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ActXEntidades_EntidadId' AND object_id=OBJECT_ID('ActxEntidades'))
                        EXEC sp_rename N'ActxEntidades.IX_ActXEntidades_EntidadId', N'IX_ActxEntidades_entidad_id', N'INDEX';

                    IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ActXEntidades_ActividadId' AND object_id=OBJECT_ID('ActxEntidades'))
                        EXEC sp_rename N'ActxEntidades.IX_ActXEntidades_ActividadId', N'IX_ActxEntidades_act_id', N'INDEX';
                END TRY
                BEGIN CATCH
                END CATCH
            ");

            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Entidades') AND name = 'EntidadId')
                    EXEC sp_rename N'dbo.Entidades.EntidadId', N'Id_Entidad', N'COLUMN';
            ");

            migrationBuilder.AddColumn<int>(
                name: "CadenaDeValorId",
                table: "Actividades",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Entidades",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = 'PK_ActxEntidades' AND parent_object_id = OBJECT_ID('ActxEntidades'))
                BEGIN
                    ALTER TABLE [ActxEntidades] ADD CONSTRAINT [PK_ActxEntidades] PRIMARY KEY ([Id]);
                END
            ");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = 'PK_Entidades' AND parent_object_id = OBJECT_ID('Entidades'))
                BEGIN
                    ALTER TABLE [Entidades] ADD CONSTRAINT [PK_Entidades] PRIMARY KEY ([Id_Entidad]);
                END
            ");

            migrationBuilder.CreateTable(
                name: "CadenasDeValor",
                columns: table => new
                {
                    CadenaDeValorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ObjetivoEspecifico = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Producto = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CadenasDeValor", x => x.CadenaDeValorId);
                });

            migrationBuilder.CreateTable(
                name: "Tareas",
                columns: table => new
                {
                    TareaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Periodo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Monto = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ActividadId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tareas", x => x.TareaId);
                    table.ForeignKey(
                        name: "FK_Tareas_Actividades_ActividadId",
                        column: x => x.ActividadId,
                        principalTable: "Actividades",
                        principalColumn: "ActividadId",
                        onDelete: ReferentialAction.Cascade);
                });

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
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 3, 46, 21, 63, DateTimeKind.Utc).AddTicks(9378));

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 3, 46, 21, 63, DateTimeKind.Utc).AddTicks(9390));

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 3, 46, 21, 63, DateTimeKind.Utc).AddTicks(9391));

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 3, 46, 21, 63, DateTimeKind.Utc).AddTicks(9392));

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 3, 46, 21, 63, DateTimeKind.Utc).AddTicks(9393));

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 3, 46, 21, 63, DateTimeKind.Utc).AddTicks(9403));

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 3, 46, 21, 63, DateTimeKind.Utc).AddTicks(9404));

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 3, 46, 21, 63, DateTimeKind.Utc).AddTicks(9405));

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 3, 46, 21, 63, DateTimeKind.Utc).AddTicks(9406));

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 3, 46, 21, 63, DateTimeKind.Utc).AddTicks(9408));

            migrationBuilder.CreateIndex(
                name: "IX_Actividades_CadenaDeValorId",
                table: "Actividades",
                column: "CadenaDeValorId");

            migrationBuilder.CreateIndex(
                name: "IX_Tareas_ActividadId",
                table: "Tareas",
                column: "ActividadId");

            migrationBuilder.AddForeignKey(
                name: "FK_Actividades_CadenasDeValor_CadenaDeValorId",
                table: "Actividades",
                column: "CadenaDeValorId",
                principalTable: "CadenasDeValor",
                principalColumn: "CadenaDeValorId",
                onDelete: ReferentialAction.SetNull);

            // Add FK ActxEntidades -> Actividades only if referential integrity holds (no orphan act_id values)
            migrationBuilder.Sql(@"
                IF NOT EXISTS (
                    SELECT 1 FROM ActxEntidades a
                    LEFT JOIN Actividades act ON a.act_id = act.ActividadId
                    WHERE act.ActividadId IS NULL
                )
                BEGIN
                    ALTER TABLE [ActxEntidades] ADD CONSTRAINT [FK_ActxEntidades_Actividades_act_id]
                    FOREIGN KEY ([act_id]) REFERENCES [Actividades] ([ActividadId]) ON DELETE CASCADE;
                END
            ");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_ActxEntidades_Entidades_entidad_id')
                BEGIN
                    ALTER TABLE [ActxEntidades] ADD CONSTRAINT [FK_ActxEntidades_Entidades_entidad_id]
                    FOREIGN KEY ([entidad_id]) REFERENCES [Entidades] ([Id_Entidad]) ON DELETE CASCADE;
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Actividades_CadenasDeValor_CadenaDeValorId",
                table: "Actividades");

            migrationBuilder.DropForeignKey(
                name: "FK_ActxEntidades_Actividades_act_id",
                table: "ActxEntidades");

            migrationBuilder.DropForeignKey(
                name: "FK_ActxEntidades_Entidades_entidad_id",
                table: "ActxEntidades");

            migrationBuilder.DropTable(
                name: "CadenasDeValor");

            migrationBuilder.DropTable(
                name: "Tareas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ActxEntidades",
                table: "ActxEntidades");

            migrationBuilder.DropIndex(
                name: "IX_Actividades_CadenaDeValorId",
                table: "Actividades");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Entidades",
                table: "Entidades");

            migrationBuilder.DropColumn(
                name: "CadenaDeValorId",
                table: "Actividades");

            migrationBuilder.RenameTable(
                name: "ActxEntidades",
                newName: "ActXEntidades");

            migrationBuilder.RenameTable(
                name: "Entidades",
                newName: "Entidad");

            migrationBuilder.RenameColumn(
                name: "entidad_id",
                table: "ActXEntidades",
                newName: "EntidadId");

            migrationBuilder.RenameColumn(
                name: "act_id",
                table: "ActXEntidades",
                newName: "ActividadId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ActXEntidades",
                newName: "ActXEntidadId");

            migrationBuilder.RenameIndex(
                name: "IX_ActxEntidades_entidad_id",
                table: "ActXEntidades",
                newName: "IX_ActXEntidades_EntidadId");

            migrationBuilder.RenameIndex(
                name: "IX_ActxEntidades_act_id",
                table: "ActXEntidades",
                newName: "IX_ActXEntidades_ActividadId");

            migrationBuilder.RenameColumn(
                name: "Id_Entidad",
                table: "Entidad",
                newName: "EntidadId");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Entidad",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActXEntidades",
                table: "ActXEntidades",
                column: "ActXEntidadId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Entidad",
                table: "Entidad",
                column: "EntidadId");

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
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 1, 25, 21, 964, DateTimeKind.Utc).AddTicks(3399));

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 1, 25, 21, 964, DateTimeKind.Utc).AddTicks(3413));

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 1, 25, 21, 964, DateTimeKind.Utc).AddTicks(3415));

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 1, 25, 21, 964, DateTimeKind.Utc).AddTicks(3416));

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 1, 25, 21, 964, DateTimeKind.Utc).AddTicks(3417));

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 1, 25, 21, 964, DateTimeKind.Utc).AddTicks(3427));

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 1, 25, 21, 964, DateTimeKind.Utc).AddTicks(3428));

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 1, 25, 21, 964, DateTimeKind.Utc).AddTicks(3429));

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 1, 25, 21, 964, DateTimeKind.Utc).AddTicks(3430));

            migrationBuilder.UpdateData(
                table: "WeatherForecasts",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 16, 1, 25, 21, 964, DateTimeKind.Utc).AddTicks(3432));

            migrationBuilder.AddForeignKey(
                name: "FK_ActXEntidades_Actividades_ActividadId",
                table: "ActXEntidades",
                column: "ActividadId",
                principalTable: "Actividades",
                principalColumn: "ActividadId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ActXEntidades_Entidad_EntidadId",
                table: "ActXEntidades",
                column: "EntidadId",
                principalTable: "Entidad",
                principalColumn: "EntidadId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
