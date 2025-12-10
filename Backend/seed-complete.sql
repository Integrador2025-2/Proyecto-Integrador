-- ============================================
-- SCRIPT DE SEED DATA COMPLETO PARA SIGPI
-- Estructura: Primero RecursosEspecificos, luego tablas detalle
-- ============================================

USE ProyectoIntegradorDb;
GO

-- ELIMINAR DATOS EXISTENTES
DELETE FROM GastosViaje;
DELETE FROM CapacitacionEventos;
DELETE FROM ServiciosTecnologicos;
DELETE FROM MaterialesInsumos;
DELETE FROM EquiposSoftware;
DELETE FROM TalentoHumano;
DELETE FROM RecursosEspecificos;
DELETE FROM Recursos;
DELETE FROM Tareas;
DELETE FROM Actividades;
DELETE FROM CadenasDeValor;
DELETE FROM Objetivos;
DELETE FROM Proyectos WHERE ProyectoId = 1;
DELETE FROM Rubros;
DELETE FROM Entidades;
DELETE FROM Contrataciones;

PRINT '🗑️  Datos existentes eliminados';

-- 1. PROYECTO
SET IDENTITY_INSERT Proyectos ON;
INSERT INTO Proyectos (ProyectoId, Nombre, FechaCreacion, Estado, UsuarioId)
VALUES (1, 'SIGPI - Sistema de Gestión de Proyectos de Investigación', GETDATE(), 'En ejecución', 3);
SET IDENTITY_INSERT Proyectos OFF;

-- 2. OBJETIVO
SET IDENTITY_INSERT Objetivos ON;
INSERT INTO Objetivos (ObjetivoId, ProyectoId, Nombre, Descripcion, ResultadoEsperado)
VALUES (1, 1, 'Desarrollar plataforma web integral', 'Crear un sistema completo para la gestión de proyectos de CTeI', 'Plataforma funcional con IA');
SET IDENTITY_INSERT Objetivos OFF;

-- 3. CADENA DE VALOR
SET IDENTITY_INSERT CadenasDeValor ON;
INSERT INTO CadenasDeValor (CadenaDeValorId, ObjetivoId, Nombre, ObjetivoEspecifico)
VALUES (1, 1, 'Desarrollo de Software', 'Implementar frontend, backend y servicios RAG con IA');
SET IDENTITY_INSERT CadenasDeValor OFF;

-- 4. ACTIVIDADES
SET IDENTITY_INSERT Actividades ON;
INSERT INTO Actividades (ActividadId, CadenaDeValorId, Nombre, Descripcion, Justificacion, DuracionAnios, EspecificacionesTecnicas, ValorUnitario)
VALUES (1, 1, 'Desarrollo Frontend React', 'UI con React 19 y TypeScript', 'Experiencia de usuario', 2, 'React 19, TypeScript, Vite', 50000000);
INSERT INTO Actividades (ActividadId, CadenaDeValorId, Nombre, Descripcion, Justificacion, DuracionAnios, EspecificacionesTecnicas, ValorUnitario)
VALUES (2, 1, 'Desarrollo Backend .NET', 'API REST con .NET 8', 'Lógica de negocio', 2, '.NET 8, EF Core, SQL Server', 60000000);
INSERT INTO Actividades (ActividadId, CadenaDeValorId, Nombre, Descripcion, Justificacion, DuracionAnios, EspecificacionesTecnicas, ValorUnitario)
VALUES (3, 1, 'Servicio RAG con IA', 'RAG con Gemini', 'Automatización presupuestos', 2, 'Python, FastAPI, ChromaDB, Gemini', 40000000);
INSERT INTO Actividades (ActividadId, CadenaDeValorId, Nombre, Descripcion, Justificacion, DuracionAnios, EspecificacionesTecnicas, ValorUnitario)
VALUES (4, 1, 'Infraestructura DevOps', 'CI/CD y Docker', 'Despliegue automatizado', 1, 'Docker, GitHub Actions, Azure', 25000000);
SET IDENTITY_INSERT Actividades OFF;

-- 5. RUBROS
SET IDENTITY_INSERT Rubros ON;
INSERT INTO Rubros (RubroId, Descripcion) VALUES (1, 'Talento Humano');
INSERT INTO Rubros (RubroId, Descripcion) VALUES (2, 'Equipos y Software');
INSERT INTO Rubros (RubroId, Descripcion) VALUES (3, 'Materiales e Insumos');
INSERT INTO Rubros (RubroId, Descripcion) VALUES (4, 'Servicios Tecnológicos');
INSERT INTO Rubros (RubroId, Descripcion) VALUES (5, 'Capacitación y Eventos');
INSERT INTO Rubros (RubroId, Descripcion) VALUES (6, 'Gastos de Viaje');
SET IDENTITY_INSERT Rubros OFF;

-- 6. ENTIDADES
SET IDENTITY_INSERT Entidades ON;
INSERT INTO Entidades (EntidadId, Nombre) VALUES (1, 'Universidad de Caldas');
INSERT INTO Entidades (EntidadId, Nombre) VALUES (2, 'MinCiencias');
INSERT INTO Entidades (EntidadId, Nombre) VALUES (3, 'Gobernación Caldas');
SET IDENTITY_INSERT Entidades OFF;

-- 7. RECURSOS
SET IDENTITY_INSERT Recursos ON;
INSERT INTO Recursos (RecursoId, ActividadId, EntidadId, RubroId, TipoRecurso, MontoEfectivo, MontoEspecie, Descripcion)
VALUES (1, 1, 1, 1, 'Humano', 90000000, 0, 'Talento Humano Frontend');
INSERT INTO Recursos (RecursoId, ActividadId, EntidadId, RubroId, TipoRecurso, MontoEfectivo, MontoEspecie, Descripcion)
VALUES (2, 2, 1, 1, 'Humano', 60000000, 0, 'Talento Humano Backend');
INSERT INTO Recursos (RecursoId, ActividadId, EntidadId, RubroId, TipoRecurso, MontoEfectivo, MontoEspecie, Descripcion)
VALUES (3, 3, 1, 1, 'Humano', 25000000, 0, 'Talento Humano IA');
INSERT INTO Recursos (RecursoId, ActividadId, EntidadId, RubroId, TipoRecurso, MontoEfectivo, MontoEspecie, Descripcion)
VALUES (4, 1, 1, 2, 'Equipos', 195000000, 0, 'Equipos y Software');
INSERT INTO Recursos (RecursoId, ActividadId, EntidadId, RubroId, TipoRecurso, MontoEfectivo, MontoEspecie, Descripcion)
VALUES (5, 1, 1, 3, 'Material', 16000000, 0, 'Materiales');
INSERT INTO Recursos (RecursoId, ActividadId, EntidadId, RubroId, TipoRecurso, MontoEfectivo, MontoEspecie, Descripcion)
VALUES (6, 2, 1, 4, 'Servicios', 48000000, 0, 'Servicios Cloud');
INSERT INTO Recursos (RecursoId, ActividadId, EntidadId, RubroId, TipoRecurso, MontoEfectivo, MontoEspecie, Descripcion)
VALUES (7, 1, 1, 5, 'Capacitación', 25000000, 0, 'Cursos y certificaciones');
INSERT INTO Recursos (RecursoId, ActividadId, EntidadId, RubroId, TipoRecurso, MontoEfectivo, MontoEspecie, Descripcion)
VALUES (8, 2, 1, 6, 'Viajes', 18000000, 0, 'Viajes técnicos');
SET IDENTITY_INSERT Recursos OFF;

-- 8. CONTRATACIONES
SET IDENTITY_INSERT Contrataciones ON;
INSERT INTO Contrataciones (ContratacionId, NivelGestion, Categoria, IdentidadAcademica, ExperienciaMinima, Iva, ValorMensual)
VALUES (1, 'Senior', 'Desarrollador', 'Ing. Sistemas', '5 años', 0, 8000000);
INSERT INTO Contrataciones (ContratacionId, NivelGestion, Categoria, IdentidadAcademica, ExperienciaMinima, Iva, ValorMensual)
VALUES (2, 'Semi-Senior', 'Desarrollador', 'Ing. Sistemas', '3 años', 0, 6000000);
INSERT INTO Contrataciones (ContratacionId, NivelGestion, Categoria, IdentidadAcademica, ExperienciaMinima, Iva, ValorMensual)
VALUES (3, 'Senior', 'Científico Datos', 'Magíster IA', '5 años', 0, 9000000);
SET IDENTITY_INSERT Contrataciones OFF;

-- 9. RECURSOS ESPECÍFICOS (TODOS - 52 registros)
SET IDENTITY_INSERT RecursosEspecificos ON;

-- RecursosEspecificos para TalentoHumano (1-14)
INSERT INTO RecursosEspecificos (RecursoEspecificoId, RecursoId, Tipo, Detalle, Cantidad, Total, PeriodoNum, PeriodoTipo) VALUES (1, 1, 'TalentoHumano', 'Dev Frontend Senior React', 1, 12000000, 48, 'Semanas');
INSERT INTO RecursosEspecificos (RecursoEspecificoId, RecursoId, Tipo, Detalle, Cantidad, Total, PeriodoNum, PeriodoTipo) VALUES (2, 1, 'TalentoHumano', 'Dev Frontend React', 1, 9000000, 48, 'Semanas');
INSERT INTO RecursosEspecificos (RecursoEspecificoId, RecursoId, Tipo, Detalle, Cantidad, Total, PeriodoNum, PeriodoTipo) VALUES (3, 1, 'TalentoHumano', 'UI/UX Designer', 1, 9000000, 36, 'Semanas');
INSERT INTO RecursosEspecificos (RecursoEspecificoId, RecursoId, Tipo, Detalle, Cantidad, Total, PeriodoNum, PeriodoTipo) VALUES (4, 1, 'TalentoHumano', 'QA Tester', 1, 5000000, 24, 'Semanas');
INSERT INTO RecursosEspecificos (RecursoEspecificoId, RecursoId, Tipo, Detalle, Cantidad, Total, PeriodoNum, PeriodoTipo) VALUES (5, 1, 'TalentoHumano', 'Scrum Master', 1, 8000000, 48, 'Semanas');
INSERT INTO RecursosEspecificos (RecursoEspecificoId, RecursoId, Tipo, Detalle, Cantidad, Total, PeriodoNum, PeriodoTipo) VALUES (6, 2, 'TalentoHumano', 'Arquitecto Software .NET', 1, 13000000, 48, 'Semanas');
INSERT INTO RecursosEspecificos (RecursoEspecificoId, RecursoId, Tipo, Detalle, Cantidad, Total, PeriodoNum, PeriodoTipo) VALUES (7, 2, 'TalentoHumano', 'Dev Backend .NET Senior', 1, 12000000, 48, 'Semanas');
INSERT INTO RecursosEspecificos (RecursoEspecificoId, RecursoId, Tipo, Detalle, Cantidad, Total, PeriodoNum, PeriodoTipo) VALUES (8, 2, 'TalentoHumano', 'Dev Backend .NET', 1, 10000000, 48, 'Semanas');
INSERT INTO RecursosEspecificos (RecursoEspecificoId, RecursoId, Tipo, Detalle, Cantidad, Total, PeriodoNum, PeriodoTipo) VALUES (9, 2, 'TalentoHumano', 'DevOps Engineer', 1, 11000000, 36, 'Semanas');
INSERT INTO RecursosEspecificos (RecursoEspecificoId, RecursoId, Tipo, Detalle, Cantidad, Total, PeriodoNum, PeriodoTipo) VALUES (10, 2, 'TalentoHumano', 'DBA SQL Server', 1, 7000000, 24, 'Semanas');
INSERT INTO RecursosEspecificos (RecursoEspecificoId, RecursoId, Tipo, Detalle, Cantidad, Total, PeriodoNum, PeriodoTipo) VALUES (11, 3, 'TalentoHumano', 'Científico Datos Senior', 1, 13000000, 48, 'Semanas');
INSERT INTO RecursosEspecificos (RecursoEspecificoId, RecursoId, Tipo, Detalle, Cantidad, Total, PeriodoNum, PeriodoTipo) VALUES (12, 3, 'TalentoHumano', 'Ingeniero ML/AI', 1, 12000000, 48, 'Semanas');
INSERT INTO RecursosEspecificos (RecursoEspecificoId, RecursoId, Tipo, Detalle, Cantidad, Total, PeriodoNum, PeriodoTipo) VALUES (13, 3, 'TalentoHumano', 'Analista de Datos', 1, 8000000, 36, 'Semanas');
INSERT INTO RecursosEspecificos (RecursoEspecificoId, RecursoId, Tipo, Detalle, Cantidad, Total, PeriodoNum, PeriodoTipo) VALUES (14, 1, 'TalentoHumano', 'Technical Writer', 1, 3000000, 12, 'Semanas');

-- RecursosEspecificos para EquiposSoftware (15-27)
INSERT INTO RecursosEspecificos (RecursoEspecificoId, RecursoId, Tipo, Detalle, Cantidad, Total, PeriodoNum, PeriodoTipo) VALUES (15, 4, 'EquiposSoftware', 'Laptop Dell XPS 15', 10, 30000000, 1, 'Única vez');
INSERT INTO RecursosEspecificos (RecursoEspecificoId, RecursoId, Tipo, Detalle, Cantidad, Total, PeriodoNum, PeriodoTipo) VALUES (16, 4, 'EquiposSoftware', 'MacBook Pro M3', 5, 25000000, 1, 'Única vez');
INSERT INTO RecursosEspecificos (RecursoEspecificoId, RecursoId, Tipo, Detalle, Cantidad, Total, PeriodoNum, PeriodoTipo) VALUES (17, 4, 'EquiposSoftware', 'Monitor LG 27" 4K', 15, 15000000, 1, 'Única vez');
INSERT INTO RecursosEspecificos (RecursoEspecificoId, RecursoId, Tipo, Detalle, Cantidad, Total, PeriodoNum, PeriodoTipo) VALUES (18, 4, 'EquiposSoftware', 'Visual Studio Enterprise', 10, 15000000, 1, 'Año');
INSERT INTO RecursosEspecificos (RecursoEspecificoId, RecursoId, Tipo, Detalle, Cantidad, Total, PeriodoNum, PeriodoTipo) VALUES (19, 4, 'EquiposSoftware', 'JetBrains IntelliJ', 10, 10000000, 1, 'Año');
INSERT INTO RecursosEspecificos (RecursoEspecificoId, RecursoId, Tipo, Detalle, Cantidad, Total, PeriodoNum, PeriodoTipo) VALUES (20, 4, 'EquiposSoftware', 'GitHub Copilot Team', 15, 20000000, 1, 'Año');
INSERT INTO RecursosEspecificos (RecursoEspecificoId, RecursoId, Tipo, Detalle, Cantidad, Total, PeriodoNum, PeriodoTipo) VALUES (21, 4, 'EquiposSoftware', 'Dell PowerEdge R740', 2, 40000000, 1, 'Única vez');
INSERT INTO RecursosEspecificos (RecursoEspecificoId, RecursoId, Tipo, Detalle, Cantidad, Total, PeriodoNum, PeriodoTipo) VALUES (22, 4, 'EquiposSoftware', 'Windows Server 2022', 2, 5000000, 1, 'Única vez');
INSERT INTO RecursosEspecificos (RecursoEspecificoId, RecursoId, Tipo, Detalle, Cantidad, Total, PeriodoNum, PeriodoTipo) VALUES (23, 4, 'EquiposSoftware', 'SQL Server Standard', 2, 10000000, 1, 'Única vez');
INSERT INTO RecursosEspecificos (RecursoEspecificoId, RecursoId, Tipo, Detalle, Cantidad, Total, PeriodoNum, PeriodoTipo) VALUES (24, 4, 'EquiposSoftware', 'Azure DevOps', 1, 5000000, 1, 'Año');
INSERT INTO RecursosEspecificos (RecursoEspecificoId, RecursoId, Tipo, Detalle, Cantidad, Total, PeriodoNum, PeriodoTipo) VALUES (25, 4, 'EquiposSoftware', 'Figma Professional', 5, 5000000, 1, 'Año');
INSERT INTO RecursosEspecificos (RecursoEspecificoId, RecursoId, Tipo, Detalle, Cantidad, Total, PeriodoNum, PeriodoTipo) VALUES (26, 4, 'EquiposSoftware', 'Postman Enterprise', 10, 10000000, 1, 'Año');
INSERT INTO RecursosEspecificos (RecursoEspecificoId, RecursoId, Tipo, Detalle, Cantidad, Total, PeriodoNum, PeriodoTipo) VALUES (27, 4, 'EquiposSoftware', 'Docker Enterprise', 2, 5000000, 1, 'Año');

-- RecursosEspecificos para MaterialesInsumos (28-35)
INSERT INTO RecursosEspecificos (RecursoEspecificoId, RecursoId, Tipo, Detalle, Cantidad, Total, PeriodoNum, PeriodoTipo) VALUES (28, 5, 'MaterialesInsumos', 'Cables HDMI USB-C', 50, 2000000, 1, 'Única vez');
INSERT INTO RecursosEspecificos (RecursoEspecificoId, RecursoId, Tipo, Detalle, Cantidad, Total, PeriodoNum, PeriodoTipo) VALUES (29, 5, 'MaterialesInsumos', 'Teclados mecánicos', 15, 2000000, 1, 'Única vez');
INSERT INTO RecursosEspecificos (RecursoEspecificoId, RecursoId, Tipo, Detalle, Cantidad, Total, PeriodoNum, PeriodoTipo) VALUES (30, 5, 'MaterialesInsumos', 'Mouse ergonómico', 15, 2000000, 1, 'Única vez');
INSERT INTO RecursosEspecificos (RecursoEspecificoId, RecursoId, Tipo, Detalle, Cantidad, Total, PeriodoNum, PeriodoTipo) VALUES (31, 5, 'MaterialesInsumos', 'Webcam 4K', 15, 2000000, 1, 'Única vez');
INSERT INTO RecursosEspecificos (RecursoEspecificoId, RecursoId, Tipo, Detalle, Cantidad, Total, PeriodoNum, PeriodoTipo) VALUES (32, 5, 'MaterialesInsumos', 'Auriculares', 15, 2000000, 1, 'Única vez');
INSERT INTO RecursosEspecificos (RecursoEspecificoId, RecursoId, Tipo, Detalle, Cantidad, Total, PeriodoNum, PeriodoTipo) VALUES (33, 5, 'MaterialesInsumos', 'Sillas ergonómicas', 15, 2000000, 1, 'Única vez');
INSERT INTO RecursosEspecificos (RecursoEspecificoId, RecursoId, Tipo, Detalle, Cantidad, Total, PeriodoNum, PeriodoTipo) VALUES (34, 5, 'MaterialesInsumos', 'Escritorios ajustables', 10, 2000000, 1, 'Única vez');
INSERT INTO RecursosEspecificos (RecursoEspecificoId, RecursoId, Tipo, Detalle, Cantidad, Total, PeriodoNum, PeriodoTipo) VALUES (35, 5, 'MaterialesInsumos', 'Discos duros 2TB', 10, 2000000, 1, 'Única vez');

-- RecursosEspecificos para ServiciosTecnologicos (36-41)
INSERT INTO RecursosEspecificos (RecursoEspecificoId, RecursoId, Tipo, Detalle, Cantidad, Total, PeriodoNum, PeriodoTipo) VALUES (36, 6, 'ServiciosTecnologicos', 'Azure App Service', 1, 8000000, 24, 'Meses');
INSERT INTO RecursosEspecificos (RecursoEspecificoId, RecursoId, Tipo, Detalle, Cantidad, Total, PeriodoNum, PeriodoTipo) VALUES (37, 6, 'ServiciosTecnologicos', 'Azure SQL Database', 1, 8000000, 24, 'Meses');
INSERT INTO RecursosEspecificos (RecursoEspecificoId, RecursoId, Tipo, Detalle, Cantidad, Total, PeriodoNum, PeriodoTipo) VALUES (38, 6, 'ServiciosTecnologicos', 'Azure Storage', 1, 8000000, 24, 'Meses');
INSERT INTO RecursosEspecificos (RecursoEspecificoId, RecursoId, Tipo, Detalle, Cantidad, Total, PeriodoNum, PeriodoTipo) VALUES (39, 6, 'ServiciosTecnologicos', 'Gemini API', 1, 8000000, 24, 'Meses');
INSERT INTO RecursosEspecificos (RecursoEspecificoId, RecursoId, Tipo, Detalle, Cantidad, Total, PeriodoNum, PeriodoTipo) VALUES (40, 6, 'ServiciosTecnologicos', 'GitHub Enterprise', 1, 8000000, 24, 'Meses');
INSERT INTO RecursosEspecificos (RecursoEspecificoId, RecursoId, Tipo, Detalle, Cantidad, Total, PeriodoNum, PeriodoTipo) VALUES (41, 6, 'ServiciosTecnologicos', 'Cloudflare CDN', 1, 8000000, 24, 'Meses');

-- RecursosEspecificos para CapacitacionEventos (42-46)
INSERT INTO RecursosEspecificos (RecursoEspecificoId, RecursoId, Tipo, Detalle, Cantidad, Total, PeriodoNum, PeriodoTipo) VALUES (42, 7, 'CapacitacionEventos', 'Curso React 19', 10, 5000000, 1, 'Única vez');
INSERT INTO RecursosEspecificos (RecursoEspecificoId, RecursoId, Tipo, Detalle, Cantidad, Total, PeriodoNum, PeriodoTipo) VALUES (43, 7, 'CapacitacionEventos', 'Certificación .NET', 5, 5000000, 1, 'Única vez');
INSERT INTO RecursosEspecificos (RecursoEspecificoId, RecursoId, Tipo, Detalle, Cantidad, Total, PeriodoNum, PeriodoTipo) VALUES (44, 7, 'CapacitacionEventos', 'Workshop ML Gemini', 8, 5000000, 1, 'Única vez');
INSERT INTO RecursosEspecificos (RecursoEspecificoId, RecursoId, Tipo, Detalle, Cantidad, Total, PeriodoNum, PeriodoTipo) VALUES (45, 7, 'CapacitacionEventos', 'Scrum Master Cert', 3, 5000000, 1, 'Única vez');
INSERT INTO RecursosEspecificos (RecursoEspecificoId, RecursoId, Tipo, Detalle, Cantidad, Total, PeriodoNum, PeriodoTipo) VALUES (46, 7, 'CapacitacionEventos', 'DevOps Azure', 6, 5000000, 1, 'Única vez');

-- RecursosEspecificos para GastosViaje (47-52)
INSERT INTO RecursosEspecificos (RecursoEspecificoId, RecursoId, Tipo, Detalle, Cantidad, Total, PeriodoNum, PeriodoTipo) VALUES (47, 8, 'GastosViaje', 'Conferencia 1', 1, 2500000, 1, 'Viaje');
INSERT INTO RecursosEspecificos (RecursoEspecificoId, RecursoId, Tipo, Detalle, Cantidad, Total, PeriodoNum, PeriodoTipo) VALUES (48, 8, 'GastosViaje', 'Conferencia 2', 1, 3000000, 1, 'Viaje');
INSERT INTO RecursosEspecificos (RecursoEspecificoId, RecursoId, Tipo, Detalle, Cantidad, Total, PeriodoNum, PeriodoTipo) VALUES (49, 8, 'GastosViaje', 'Conferencia 3', 1, 2800000, 1, 'Viaje');
INSERT INTO RecursosEspecificos (RecursoEspecificoId, RecursoId, Tipo, Detalle, Cantidad, Total, PeriodoNum, PeriodoTipo) VALUES (50, 8, 'GastosViaje', 'Reunión cliente 1', 1, 1500000, 1, 'Viaje');
INSERT INTO RecursosEspecificos (RecursoEspecificoId, RecursoId, Tipo, Detalle, Cantidad, Total, PeriodoNum, PeriodoTipo) VALUES (51, 8, 'GastosViaje', 'Reunión cliente 2', 1, 2000000, 1, 'Viaje');
INSERT INTO RecursosEspecificos (RecursoEspecificoId, RecursoId, Tipo, Detalle, Cantidad, Total, PeriodoNum, PeriodoTipo) VALUES (52, 8, 'GastosViaje', 'Capacitación externa', 1, 2200000, 1, 'Viaje');

SET IDENTITY_INSERT RecursosEspecificos OFF;

-- 10. TALENTO HUMANO (14 registros)
SET IDENTITY_INSERT TalentoHumano ON;
INSERT INTO TalentoHumano (TalentoHumanoId, RecursoEspecificoId, ContratacionId, CargoEspecifico, Semanas, Total) VALUES (1, 1, 1, 'Desarrollador Frontend Senior React', 48, 12000000);
INSERT INTO TalentoHumano (TalentoHumanoId, RecursoEspecificoId, ContratacionId, CargoEspecifico, Semanas, Total) VALUES (2, 2, 2, 'Desarrollador Frontend React', 48, 9000000);
INSERT INTO TalentoHumano (TalentoHumanoId, RecursoEspecificoId, ContratacionId, CargoEspecifico, Semanas, Total) VALUES (3, 3, 2, 'UI/UX Designer', 36, 9000000);
INSERT INTO TalentoHumano (TalentoHumanoId, RecursoEspecificoId, ContratacionId, CargoEspecifico, Semanas, Total) VALUES (4, 4, 2, 'QA Tester', 24, 5000000);
INSERT INTO TalentoHumano (TalentoHumanoId, RecursoEspecificoId, ContratacionId, CargoEspecifico, Semanas, Total) VALUES (5, 5, 2, 'Scrum Master', 48, 8000000);
INSERT INTO TalentoHumano (TalentoHumanoId, RecursoEspecificoId, ContratacionId, CargoEspecifico, Semanas, Total) VALUES (6, 6, 1, 'Arquitecto de Software .NET', 48, 13000000);
INSERT INTO TalentoHumano (TalentoHumanoId, RecursoEspecificoId, ContratacionId, CargoEspecifico, Semanas, Total) VALUES (7, 7, 1, 'Desarrollador Backend .NET Senior', 48, 12000000);
INSERT INTO TalentoHumano (TalentoHumanoId, RecursoEspecificoId, ContratacionId, CargoEspecifico, Semanas, Total) VALUES (8, 8, 2, 'Desarrollador Backend .NET', 48, 10000000);
INSERT INTO TalentoHumano (TalentoHumanoId, RecursoEspecificoId, ContratacionId, CargoEspecifico, Semanas, Total) VALUES (9, 9, 1, 'DevOps Engineer', 36, 11000000);
INSERT INTO TalentoHumano (TalentoHumanoId, RecursoEspecificoId, ContratacionId, CargoEspecifico, Semanas, Total) VALUES (10, 10, 1, 'DBA SQL Server', 24, 7000000);
INSERT INTO TalentoHumano (TalentoHumanoId, RecursoEspecificoId, ContratacionId, CargoEspecifico, Semanas, Total) VALUES (11, 11, 3, 'Científico de Datos Senior', 48, 13000000);
INSERT INTO TalentoHumano (TalentoHumanoId, RecursoEspecificoId, ContratacionId, CargoEspecifico, Semanas, Total) VALUES (12, 12, 2, 'Ingeniero ML/AI', 48, 12000000);
INSERT INTO TalentoHumano (TalentoHumanoId, RecursoEspecificoId, ContratacionId, CargoEspecifico, Semanas, Total) VALUES (13, 13, 2, 'Analista de Datos', 36, 8000000);
INSERT INTO TalentoHumano (TalentoHumanoId, RecursoEspecificoId, ContratacionId, CargoEspecifico, Semanas, Total) VALUES (14, 14, 2, 'Technical Writer', 12, 3000000);
SET IDENTITY_INSERT TalentoHumano OFF;

-- 11. EQUIPOS Y SOFTWARE (13 registros)
SET IDENTITY_INSERT EquiposSoftware ON;
INSERT INTO EquiposSoftware (EquiposSoftwareId, RecursoEspecificoId, EspecificacionesTecnicas) VALUES (1, 15, 'Laptop Dell XPS 15 - Intel i7, 32GB RAM, 1TB SSD');
INSERT INTO EquiposSoftware (EquiposSoftwareId, RecursoEspecificoId, EspecificacionesTecnicas) VALUES (2, 16, 'Laptop MacBook Pro M3 - 32GB RAM, 1TB SSD');
INSERT INTO EquiposSoftware (EquiposSoftwareId, RecursoEspecificoId, EspecificacionesTecnicas) VALUES (3, 17, 'Monitor LG 27" 4K UltraFine');
INSERT INTO EquiposSoftware (EquiposSoftwareId, RecursoEspecificoId, EspecificacionesTecnicas) VALUES (4, 18, 'Licencia Visual Studio Enterprise');
INSERT INTO EquiposSoftware (EquiposSoftwareId, RecursoEspecificoId, EspecificacionesTecnicas) VALUES (5, 19, 'Licencia JetBrains IntelliJ Ultimate');
INSERT INTO EquiposSoftware (EquiposSoftwareId, RecursoEspecificoId, EspecificacionesTecnicas) VALUES (6, 20, 'Licencia GitHub Copilot Team');
INSERT INTO EquiposSoftware (EquiposSoftwareId, RecursoEspecificoId, EspecificacionesTecnicas) VALUES (7, 21, 'Servidor Dell PowerEdge R740');
INSERT INTO EquiposSoftware (EquiposSoftwareId, RecursoEspecificoId, EspecificacionesTecnicas) VALUES (8, 22, 'Licencia Windows Server 2022');
INSERT INTO EquiposSoftware (EquiposSoftwareId, RecursoEspecificoId, EspecificacionesTecnicas) VALUES (9, 23, 'Licencia SQL Server Standard');
INSERT INTO EquiposSoftware (EquiposSoftwareId, RecursoEspecificoId, EspecificacionesTecnicas) VALUES (10, 24, 'Azure DevOps Server');
INSERT INTO EquiposSoftware (EquiposSoftwareId, RecursoEspecificoId, EspecificacionesTecnicas) VALUES (11, 25, 'Figma Professional Plan');
INSERT INTO EquiposSoftware (EquiposSoftwareId, RecursoEspecificoId, EspecificacionesTecnicas) VALUES (12, 26, 'Postman Enterprise');
INSERT INTO EquiposSoftware (EquiposSoftwareId, RecursoEspecificoId, EspecificacionesTecnicas) VALUES (13, 27, 'Docker Enterprise');
SET IDENTITY_INSERT EquiposSoftware OFF;

-- 12. MATERIALES E INSUMOS (8 registros)
SET IDENTITY_INSERT MaterialesInsumos ON;
INSERT INTO MaterialesInsumos (MaterialesInsumosId, RecursoEspecificoId, Materiales) VALUES (1, 28, 'Cables HDMI y USB-C');
INSERT INTO MaterialesInsumos (MaterialesInsumosId, RecursoEspecificoId, Materiales) VALUES (2, 29, 'Teclados mecánicos programables');
INSERT INTO MaterialesInsumos (MaterialesInsumosId, RecursoEspecificoId, Materiales) VALUES (3, 30, 'Mouse ergonómico inalámbrico');
INSERT INTO MaterialesInsumos (MaterialesInsumosId, RecursoEspecificoId, Materiales) VALUES (4, 31, 'Webcam Logitech 4K');
INSERT INTO MaterialesInsumos (MaterialesInsumosId, RecursoEspecificoId, Materiales) VALUES (5, 32, 'Auriculares con cancelación de ruido');
INSERT INTO MaterialesInsumos (MaterialesInsumosId, RecursoEspecificoId, Materiales) VALUES (6, 33, 'Sillas ergonómicas');
INSERT INTO MaterialesInsumos (MaterialesInsumosId, RecursoEspecificoId, Materiales) VALUES (7, 34, 'Escritorios ajustables');
INSERT INTO MaterialesInsumos (MaterialesInsumosId, RecursoEspecificoId, Materiales) VALUES (8, 35, 'Discos duros externos 2TB');
SET IDENTITY_INSERT MaterialesInsumos OFF;

-- 13. SERVICIOS TECNOLÓGICOS (6 registros)
SET IDENTITY_INSERT ServiciosTecnologicos ON;
INSERT INTO ServiciosTecnologicos (ServiciosTecnologicosId, RecursoEspecificoId, Descripcion) VALUES (1, 36, 'Azure App Service - Plan Premium');
INSERT INTO ServiciosTecnologicos (ServiciosTecnologicosId, RecursoEspecificoId, Descripcion) VALUES (2, 37, 'Azure SQL Database - DTU 100');
INSERT INTO ServiciosTecnologicos (ServiciosTecnologicosId, RecursoEspecificoId, Descripcion) VALUES (3, 38, 'Azure Storage - 1TB Blob');
INSERT INTO ServiciosTecnologicos (ServiciosTecnologicosId, RecursoEspecificoId, Descripcion) VALUES (4, 39, 'Google Gemini API - 1M tokens/mes');
INSERT INTO ServiciosTecnologicos (ServiciosTecnologicosId, RecursoEspecificoId, Descripcion) VALUES (5, 40, 'GitHub Enterprise Cloud');
INSERT INTO ServiciosTecnologicos (ServiciosTecnologicosId, RecursoEspecificoId, Descripcion) VALUES (6, 41, 'Cloudflare CDN Pro');
SET IDENTITY_INSERT ServiciosTecnologicos OFF;

-- 14. CAPACITACIÓN Y EVENTOS (5 registros)
SET IDENTITY_INSERT CapacitacionEventos ON;
INSERT INTO CapacitacionEventos (CapacitacionEventosId, RecursoEspecificoId, Tema, Cantidad) VALUES (1, 42, 'Curso React 19 Avanzado', 10);
INSERT INTO CapacitacionEventos (CapacitacionEventosId, RecursoEspecificoId, Tema, Cantidad) VALUES (2, 43, 'Certificación .NET Enterprise Architect', 5);
INSERT INTO CapacitacionEventos (CapacitacionEventosId, RecursoEspecificoId, Tema, Cantidad) VALUES (3, 44, 'Workshop Machine Learning con Gemini', 8);
INSERT INTO CapacitacionEventos (CapacitacionEventosId, RecursoEspecificoId, Tema, Cantidad) VALUES (4, 45, 'Scrum Master Certification', 3);
INSERT INTO CapacitacionEventos (CapacitacionEventosId, RecursoEspecificoId, Tema, Cantidad) VALUES (5, 46, 'DevOps con Azure - Bootcamp', 6);
SET IDENTITY_INSERT CapacitacionEventos OFF;

-- 15. GASTOS DE VIAJE (6 registros)
SET IDENTITY_INSERT GastosViaje ON;
INSERT INTO GastosViaje (GastosViajeId, RecursoEspecificoId, Costo) VALUES (1, 47, 2500000);
INSERT INTO GastosViaje (GastosViajeId, RecursoEspecificoId, Costo) VALUES (2, 48, 3000000);
INSERT INTO GastosViaje (GastosViajeId, RecursoEspecificoId, Costo) VALUES (3, 49, 2800000);
INSERT INTO GastosViaje (GastosViajeId, RecursoEspecificoId, Costo) VALUES (4, 50, 1500000);
INSERT INTO GastosViaje (GastosViajeId, RecursoEspecificoId, Costo) VALUES (5, 51, 2000000);
INSERT INTO GastosViaje (GastosViajeId, RecursoEspecificoId, Costo) VALUES (6, 52, 2200000);
SET IDENTITY_INSERT GastosViaje OFF;

-- 16. TAREAS
SET IDENTITY_INSERT Tareas ON;
INSERT INTO Tareas (TareaId, ActividadId, Nombre, Descripcion, Periodo, Monto) VALUES (1, 1, 'Diseño arquitectura frontend', 'Estructura componentes', 'Mes 1', 5000000);
INSERT INTO Tareas (TareaId, ActividadId, Nombre, Descripcion, Periodo, Monto) VALUES (2, 1, 'Implementación autenticación', 'OAuth2 y JWT', 'Mes 2', 8000000);
INSERT INTO Tareas (TareaId, ActividadId, Nombre, Descripcion, Periodo, Monto) VALUES (3, 1, 'Módulo proyectos', 'CRUD proyectos', 'Mes 3-4', 15000000);
INSERT INTO Tareas (TareaId, ActividadId, Nombre, Descripcion, Periodo, Monto) VALUES (4, 2, 'Diseño base datos', 'Modelado entidades', 'Mes 1', 6000000);
INSERT INTO Tareas (TareaId, ActividadId, Nombre, Descripcion, Periodo, Monto) VALUES (5, 2, 'Implementación CQRS', 'Commands y Queries', 'Mes 2-3', 18000000);
INSERT INTO Tareas (TareaId, ActividadId, Nombre, Descripcion, Periodo, Monto) VALUES (6, 3, 'Integración Gemini API', 'Setup API', 'Mes 1', 7000000);
INSERT INTO Tareas (TareaId, ActividadId, Nombre, Descripcion, Periodo, Monto) VALUES (7, 3, 'Extractor presupuestos', 'Parser Excel/Word', 'Mes 2-3', 12000000);
INSERT INTO Tareas (TareaId, ActividadId, Nombre, Descripcion, Periodo, Monto) VALUES (8, 4, 'Setup CI/CD', 'GitHub Actions', 'Mes 1', 5000000);
SET IDENTITY_INSERT Tareas OFF;

PRINT '';
PRINT '✅ Seed data completado!';
PRINT '📊 52 RecursosEspecificos, 14 TalentoHumano, 13 EquiposSoftware';
PRINT '    8 MaterialesInsumos, 6 ServiciosTecnologicos';
PRINT '    5 CapacitacionEventos, 6 GastosViaje, 8 Tareas';
GO
