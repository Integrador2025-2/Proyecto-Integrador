using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialMinCienciasDBSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contrataciones",
                columns: table => new
                {
                    ContratacionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NivelGestion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Categoria = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdentidadAcademica = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExperienciaMinima = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Iva = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorMensual = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contrataciones", x => x.ContratacionId);
                });

            migrationBuilder.CreateTable(
                name: "Entidades",
                columns: table => new
                {
                    EntidadId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entidades", x => x.EntidadId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Permissions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rubro",
                columns: table => new
                {
                    RubroId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rubro", x => x.RubroId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    GoogleId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfilePictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Provider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Proyectos",
                columns: table => new
                {
                    ProyectoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proyectos", x => x.ProyectoId);
                    table.ForeignKey(
                        name: "FK_Proyectos_Users_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Objetivos",
                columns: table => new
                {
                    ObjetivoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProyectoId = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResultadoEsperado = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Objetivos", x => x.ObjetivoId);
                    table.ForeignKey(
                        name: "FK_Objetivos_Proyectos_ProyectoId",
                        column: x => x.ProyectoId,
                        principalTable: "Proyectos",
                        principalColumn: "ProyectoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CadenasDeValor",
                columns: table => new
                {
                    CadenaDeValorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ObjetivoId = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ObjetivoEspecifico = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CadenasDeValor", x => x.CadenaDeValorId);
                    table.ForeignKey(
                        name: "FK_CadenasDeValor_Objetivos_ObjetivoId",
                        column: x => x.ObjetivoId,
                        principalTable: "Objetivos",
                        principalColumn: "ObjetivoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Actividades",
                columns: table => new
                {
                    ActividadId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CadenaDeValorId = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Justificacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DuracionAnios = table.Column<int>(type: "int", nullable: false),
                    EspecificacionesTecnicas = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValorUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actividades", x => x.ActividadId);
                    table.ForeignKey(
                        name: "FK_Actividades_CadenasDeValor_CadenaDeValorId",
                        column: x => x.CadenaDeValorId,
                        principalTable: "CadenasDeValor",
                        principalColumn: "CadenaDeValorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActXEntidades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActividadId = table.Column<int>(type: "int", nullable: false),
                    EntidadId = table.Column<int>(type: "int", nullable: false),
                    Efectivo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Especie = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalContribucion = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActXEntidades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActXEntidades_Actividades_ActividadId",
                        column: x => x.ActividadId,
                        principalTable: "Actividades",
                        principalColumn: "ActividadId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActXEntidades_Entidades_EntidadId",
                        column: x => x.EntidadId,
                        principalTable: "Entidades",
                        principalColumn: "EntidadId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Recursos",
                columns: table => new
                {
                    RecursoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActividadId = table.Column<int>(type: "int", nullable: false),
                    EntidadId = table.Column<int>(type: "int", nullable: false),
                    RubroId = table.Column<int>(type: "int", nullable: false),
                    TipoRecurso = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MontoEfectivo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MontoEspecie = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recursos", x => x.RecursoId);
                    table.ForeignKey(
                        name: "FK_Recursos_Actividades_ActividadId",
                        column: x => x.ActividadId,
                        principalTable: "Actividades",
                        principalColumn: "ActividadId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Recursos_Entidades_EntidadId",
                        column: x => x.EntidadId,
                        principalTable: "Entidades",
                        principalColumn: "EntidadId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Recursos_Rubro_RubroId",
                        column: x => x.RubroId,
                        principalTable: "Rubro",
                        principalColumn: "RubroId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tareas",
                columns: table => new
                {
                    TareaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActividadId = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Periodo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Monto = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "RecursosEspecificos",
                columns: table => new
                {
                    RecursoEspecificoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecursoId = table.Column<int>(type: "int", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Detalle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PeriodoNum = table.Column<int>(type: "int", nullable: false),
                    PeriodoTipo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecursosEspecificos", x => x.RecursoEspecificoId);
                    table.ForeignKey(
                        name: "FK_RecursosEspecificos_Recursos_RecursoId",
                        column: x => x.RecursoId,
                        principalTable: "Recursos",
                        principalColumn: "RecursoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CronogramaTareas",
                columns: table => new
                {
                    CronogramaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TareaId = table.Column<int>(type: "int", nullable: false),
                    DuracionMeses = table.Column<int>(type: "int", nullable: false),
                    DuracionDias = table.Column<int>(type: "int", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CronogramaTareas", x => x.CronogramaId);
                    table.ForeignKey(
                        name: "FK_CronogramaTareas_Tareas_TareaId",
                        column: x => x.TareaId,
                        principalTable: "Tareas",
                        principalColumn: "TareaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Administrativos",
                columns: table => new
                {
                    AdministrativoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecursoEspecificoId = table.Column<int>(type: "int", nullable: false),
                    Cargo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RazonSocial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Justificacion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administrativos", x => x.AdministrativoId);
                    table.ForeignKey(
                        name: "FK_Administrativos_RecursosEspecificos_RecursoEspecificoId",
                        column: x => x.RecursoEspecificoId,
                        principalTable: "RecursosEspecificos",
                        principalColumn: "RecursoEspecificoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CapacitacionEventos",
                columns: table => new
                {
                    CapacitacionEventosId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecursoEspecificoId = table.Column<int>(type: "int", nullable: false),
                    Tema = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CapacitacionEventos", x => x.CapacitacionEventosId);
                    table.ForeignKey(
                        name: "FK_CapacitacionEventos_RecursosEspecificos_RecursoEspecificoId",
                        column: x => x.RecursoEspecificoId,
                        principalTable: "RecursosEspecificos",
                        principalColumn: "RecursoEspecificoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Divulgacion",
                columns: table => new
                {
                    DivulgacionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecursoEspecificoId = table.Column<int>(type: "int", nullable: false),
                    MedioDivulgacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Alcance = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Justificacion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Divulgacion", x => x.DivulgacionId);
                    table.ForeignKey(
                        name: "FK_Divulgacion_RecursosEspecificos_RecursoEspecificoId",
                        column: x => x.RecursoEspecificoId,
                        principalTable: "RecursosEspecificos",
                        principalColumn: "RecursoEspecificoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EquiposSoftware",
                columns: table => new
                {
                    EquiposSoftwareId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecursoEspecificoId = table.Column<int>(type: "int", nullable: false),
                    EspecificacionesTecnicas = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquiposSoftware", x => x.EquiposSoftwareId);
                    table.ForeignKey(
                        name: "FK_EquiposSoftware_RecursosEspecificos_RecursoEspecificoId",
                        column: x => x.RecursoEspecificoId,
                        principalTable: "RecursosEspecificos",
                        principalColumn: "RecursoEspecificoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GastosViaje",
                columns: table => new
                {
                    GastosViajeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecursoEspecificoId = table.Column<int>(type: "int", nullable: false),
                    Costo = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GastosViaje", x => x.GastosViajeId);
                    table.ForeignKey(
                        name: "FK_GastosViaje_RecursosEspecificos_RecursoEspecificoId",
                        column: x => x.RecursoEspecificoId,
                        principalTable: "RecursosEspecificos",
                        principalColumn: "RecursoEspecificoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Infraestructura",
                columns: table => new
                {
                    InfraestructuraId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecursoEspecificoId = table.Column<int>(type: "int", nullable: false),
                    TipoInfraestructura = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Enlace = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CaracteristicasTecnicas = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Infraestructura", x => x.InfraestructuraId);
                    table.ForeignKey(
                        name: "FK_Infraestructura_RecursosEspecificos_RecursoEspecificoId",
                        column: x => x.RecursoEspecificoId,
                        principalTable: "RecursosEspecificos",
                        principalColumn: "RecursoEspecificoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaterialesInsumos",
                columns: table => new
                {
                    MaterialesInsumosId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecursoEspecificoId = table.Column<int>(type: "int", nullable: false),
                    Materiales = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialesInsumos", x => x.MaterialesInsumosId);
                    table.ForeignKey(
                        name: "FK_MaterialesInsumos_RecursosEspecificos_RecursoEspecificoId",
                        column: x => x.RecursoEspecificoId,
                        principalTable: "RecursosEspecificos",
                        principalColumn: "RecursoEspecificoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Otros",
                columns: table => new
                {
                    OtrosId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecursoEspecificoId = table.Column<int>(type: "int", nullable: false),
                    Justificacion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Otros", x => x.OtrosId);
                    table.ForeignKey(
                        name: "FK_Otros_RecursosEspecificos_RecursoEspecificoId",
                        column: x => x.RecursoEspecificoId,
                        principalTable: "RecursosEspecificos",
                        principalColumn: "RecursoEspecificoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProteccionConocimientoDivulgacion",
                columns: table => new
                {
                    ProteccionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecursoEspecificoId = table.Column<int>(type: "int", nullable: false),
                    ActividadHapat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntidadResponsable = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Justificacion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProteccionConocimientoDivulgacion", x => x.ProteccionId);
                    table.ForeignKey(
                        name: "FK_ProteccionConocimientoDivulgacion_RecursosEspecificos_RecursoEspecificoId",
                        column: x => x.RecursoEspecificoId,
                        principalTable: "RecursosEspecificos",
                        principalColumn: "RecursoEspecificoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SeguimientoEvaluacion",
                columns: table => new
                {
                    SeguimientoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecursoEspecificoId = table.Column<int>(type: "int", nullable: false),
                    CargoResponsable = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MetodoEvaluacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Frecuencia = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeguimientoEvaluacion", x => x.SeguimientoId);
                    table.ForeignKey(
                        name: "FK_SeguimientoEvaluacion_RecursosEspecificos_RecursoEspecificoId",
                        column: x => x.RecursoEspecificoId,
                        principalTable: "RecursosEspecificos",
                        principalColumn: "RecursoEspecificoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiciosTecnologicos",
                columns: table => new
                {
                    ServiciosTecnologicosId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecursoEspecificoId = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiciosTecnologicos", x => x.ServiciosTecnologicosId);
                    table.ForeignKey(
                        name: "FK_ServiciosTecnologicos_RecursosEspecificos_RecursoEspecificoId",
                        column: x => x.RecursoEspecificoId,
                        principalTable: "RecursosEspecificos",
                        principalColumn: "RecursoEspecificoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TalentoHumano",
                columns: table => new
                {
                    TalentoHumanoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecursoEspecificoId = table.Column<int>(type: "int", nullable: false),
                    ContratacionId = table.Column<int>(type: "int", nullable: false),
                    CargoEspecifico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Semanas = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TalentoHumano", x => x.TalentoHumanoId);
                    table.ForeignKey(
                        name: "FK_TalentoHumano_Contrataciones_ContratacionId",
                        column: x => x.ContratacionId,
                        principalTable: "Contrataciones",
                        principalColumn: "ContratacionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TalentoHumano_RecursosEspecificos_RecursoEspecificoId",
                        column: x => x.RecursoEspecificoId,
                        principalTable: "RecursosEspecificos",
                        principalColumn: "RecursoEspecificoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RemuneracionesPorAnio",
                columns: table => new
                {
                    RemuneracionPorAnioId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TalentoHumanoId = table.Column<int>(type: "int", nullable: false),
                    Anio = table.Column<int>(type: "int", nullable: false),
                    Honorarios = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorHora = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SemanasAnio = table.Column<int>(type: "int", nullable: false),
                    TotalAnio = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RemuneracionesPorAnio", x => x.RemuneracionPorAnioId);
                    table.ForeignKey(
                        name: "FK_RemuneracionesPorAnio_TalentoHumano_TalentoHumanoId",
                        column: x => x.TalentoHumanoId,
                        principalTable: "TalentoHumano",
                        principalColumn: "TalentoHumanoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TalentoHumanoTareas",
                columns: table => new
                {
                    TalentoHumanoTareasId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TalentoHumanoId = table.Column<int>(type: "int", nullable: false),
                    Tarea = table.Column<int>(type: "int", nullable: false),
                    HorasAsignadas = table.Column<int>(type: "int", nullable: false),
                    RolenTarea = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaAsignacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TareaNavigationTareaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TalentoHumanoTareas", x => x.TalentoHumanoTareasId);
                    table.ForeignKey(
                        name: "FK_TalentoHumanoTareas_TalentoHumano_TalentoHumanoId",
                        column: x => x.TalentoHumanoId,
                        principalTable: "TalentoHumano",
                        principalColumn: "TalentoHumanoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TalentoHumanoTareas_Tareas_TareaNavigationTareaId",
                        column: x => x.TareaNavigationTareaId,
                        principalTable: "Tareas",
                        principalColumn: "TareaId");
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "Description", "IsActive", "Name", "Permissions", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 11, 6, 1, 36, 48, 170, DateTimeKind.Utc).AddTicks(6495), "Rol con permisos completos del sistema", true, "Administrador", "[\"users.create\", \"users.read\", \"users.update\", \"users.delete\"]", null },
                    { 2, new DateTime(2025, 11, 6, 1, 36, 48, 170, DateTimeKind.Utc).AddTicks(6498), "Rol con permisos básicos del sistema", true, "Usuario", "[\"users.read\"]", null }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "FirstName", "GoogleId", "IsActive", "LastName", "PasswordHash", "ProfilePictureUrl", "Provider", "RoleId", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 11, 6, 1, 36, 48, 288, DateTimeKind.Utc).AddTicks(333), "juan.perez@email.com", "Juan", null, true, "Pérez", "$2a$11$R3ri5/ExUlHfWM83YHxwN.7R8OLsE1TFsLvWYDquRKAEDt7He8dAO", null, "local", 1, null },
                    { 2, new DateTime(2025, 11, 6, 1, 36, 48, 406, DateTimeKind.Utc).AddTicks(2145), "maria.gonzalez@email.com", "María", null, true, "González", "$2a$11$evROIYC4kMk5mF/HxldmTOpnuTmSBgkCIzkbKt5n3V08y0UTLKza2", null, "local", 2, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Actividades_CadenaDeValorId",
                table: "Actividades",
                column: "CadenaDeValorId");

            migrationBuilder.CreateIndex(
                name: "IX_ActXEntidades_ActividadId",
                table: "ActXEntidades",
                column: "ActividadId");

            migrationBuilder.CreateIndex(
                name: "IX_ActXEntidades_EntidadId",
                table: "ActXEntidades",
                column: "EntidadId");

            migrationBuilder.CreateIndex(
                name: "IX_Administrativos_RecursoEspecificoId",
                table: "Administrativos",
                column: "RecursoEspecificoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CadenasDeValor_ObjetivoId",
                table: "CadenasDeValor",
                column: "ObjetivoId");

            migrationBuilder.CreateIndex(
                name: "IX_CapacitacionEventos_RecursoEspecificoId",
                table: "CapacitacionEventos",
                column: "RecursoEspecificoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CronogramaTareas_TareaId",
                table: "CronogramaTareas",
                column: "TareaId");

            migrationBuilder.CreateIndex(
                name: "IX_Divulgacion_RecursoEspecificoId",
                table: "Divulgacion",
                column: "RecursoEspecificoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EquiposSoftware_RecursoEspecificoId",
                table: "EquiposSoftware",
                column: "RecursoEspecificoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GastosViaje_RecursoEspecificoId",
                table: "GastosViaje",
                column: "RecursoEspecificoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Infraestructura_RecursoEspecificoId",
                table: "Infraestructura",
                column: "RecursoEspecificoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MaterialesInsumos_RecursoEspecificoId",
                table: "MaterialesInsumos",
                column: "RecursoEspecificoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Objetivos_ProyectoId",
                table: "Objetivos",
                column: "ProyectoId");

            migrationBuilder.CreateIndex(
                name: "IX_Otros_RecursoEspecificoId",
                table: "Otros",
                column: "RecursoEspecificoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProteccionConocimientoDivulgacion_RecursoEspecificoId",
                table: "ProteccionConocimientoDivulgacion",
                column: "RecursoEspecificoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Proyectos_UsuarioId",
                table: "Proyectos",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Recursos_ActividadId",
                table: "Recursos",
                column: "ActividadId");

            migrationBuilder.CreateIndex(
                name: "IX_Recursos_EntidadId",
                table: "Recursos",
                column: "EntidadId");

            migrationBuilder.CreateIndex(
                name: "IX_Recursos_RubroId",
                table: "Recursos",
                column: "RubroId");

            migrationBuilder.CreateIndex(
                name: "IX_RecursosEspecificos_RecursoId",
                table: "RecursosEspecificos",
                column: "RecursoId");

            migrationBuilder.CreateIndex(
                name: "IX_RemuneracionesPorAnio_TalentoHumanoId",
                table: "RemuneracionesPorAnio",
                column: "TalentoHumanoId");

            migrationBuilder.CreateIndex(
                name: "IX_SeguimientoEvaluacion_RecursoEspecificoId",
                table: "SeguimientoEvaluacion",
                column: "RecursoEspecificoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiciosTecnologicos_RecursoEspecificoId",
                table: "ServiciosTecnologicos",
                column: "RecursoEspecificoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TalentoHumano_ContratacionId",
                table: "TalentoHumano",
                column: "ContratacionId");

            migrationBuilder.CreateIndex(
                name: "IX_TalentoHumano_RecursoEspecificoId",
                table: "TalentoHumano",
                column: "RecursoEspecificoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TalentoHumanoTareas_TalentoHumanoId",
                table: "TalentoHumanoTareas",
                column: "TalentoHumanoId");

            migrationBuilder.CreateIndex(
                name: "IX_TalentoHumanoTareas_TareaNavigationTareaId",
                table: "TalentoHumanoTareas",
                column: "TareaNavigationTareaId");

            migrationBuilder.CreateIndex(
                name: "IX_Tareas_ActividadId",
                table: "Tareas",
                column: "ActividadId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActXEntidades");

            migrationBuilder.DropTable(
                name: "Administrativos");

            migrationBuilder.DropTable(
                name: "CapacitacionEventos");

            migrationBuilder.DropTable(
                name: "CronogramaTareas");

            migrationBuilder.DropTable(
                name: "Divulgacion");

            migrationBuilder.DropTable(
                name: "EquiposSoftware");

            migrationBuilder.DropTable(
                name: "GastosViaje");

            migrationBuilder.DropTable(
                name: "Infraestructura");

            migrationBuilder.DropTable(
                name: "MaterialesInsumos");

            migrationBuilder.DropTable(
                name: "Otros");

            migrationBuilder.DropTable(
                name: "ProteccionConocimientoDivulgacion");

            migrationBuilder.DropTable(
                name: "RemuneracionesPorAnio");

            migrationBuilder.DropTable(
                name: "SeguimientoEvaluacion");

            migrationBuilder.DropTable(
                name: "ServiciosTecnologicos");

            migrationBuilder.DropTable(
                name: "TalentoHumanoTareas");

            migrationBuilder.DropTable(
                name: "TalentoHumano");

            migrationBuilder.DropTable(
                name: "Tareas");

            migrationBuilder.DropTable(
                name: "Contrataciones");

            migrationBuilder.DropTable(
                name: "RecursosEspecificos");

            migrationBuilder.DropTable(
                name: "Recursos");

            migrationBuilder.DropTable(
                name: "Actividades");

            migrationBuilder.DropTable(
                name: "Entidades");

            migrationBuilder.DropTable(
                name: "Rubro");

            migrationBuilder.DropTable(
                name: "CadenasDeValor");

            migrationBuilder.DropTable(
                name: "Objetivos");

            migrationBuilder.DropTable(
                name: "Proyectos");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
