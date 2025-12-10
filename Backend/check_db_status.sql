-- Script para verificar el estado completo de la base de datos
-- SIGPI - Sistema de Gestión de Proyectos de Investigación

PRINT '========================================';
PRINT 'REPORTE COMPLETO DE BASE DE DATOS';
PRINT '========================================';
PRINT '';

-- 1. LISTAR TODAS LAS TABLAS
PRINT '1. TABLAS EN LA BASE DE DATOS:';
PRINT '----------------------------------------';
SELECT
    t.name AS 'Tabla',
    SUM(p.rows) AS 'Registros'
FROM
    sys.tables t
INNER JOIN
    sys.partitions p ON t.object_id = p.object_id
WHERE
    p.index_id IN (0,1)
    AND t.is_ms_shipped = 0
GROUP BY
    t.name
ORDER BY
    t.name;
PRINT '';

-- 2. ESTRUCTURA DE CADA TABLA
PRINT '2. ESTRUCTURA DE TABLAS:';
PRINT '----------------------------------------';

-- Users
PRINT '>>> USERS';
SELECT
    COLUMN_NAME AS 'Columna',
    DATA_TYPE AS 'Tipo',
    CHARACTER_MAXIMUM_LENGTH AS 'Longitud',
    IS_NULLABLE AS 'Nullable'
FROM
    INFORMATION_SCHEMA.COLUMNS
WHERE
    TABLE_NAME = 'Users'
ORDER BY
    ORDINAL_POSITION;
PRINT '';

-- Roles
PRINT '>>> ROLES';
SELECT
    COLUMN_NAME AS 'Columna',
    DATA_TYPE AS 'Tipo',
    CHARACTER_MAXIMUM_LENGTH AS 'Longitud',
    IS_NULLABLE AS 'Nullable'
FROM
    INFORMATION_SCHEMA.COLUMNS
WHERE
    TABLE_NAME = 'Roles'
ORDER BY
    ORDINAL_POSITION;
PRINT '';

-- Proyectos
PRINT '>>> PROYECTOS';
SELECT
    COLUMN_NAME AS 'Columna',
    DATA_TYPE AS 'Tipo',
    CHARACTER_MAXIMUM_LENGTH AS 'Longitud',
    IS_NULLABLE AS 'Nullable'
FROM
    INFORMATION_SCHEMA.COLUMNS
WHERE
    TABLE_NAME = 'Proyectos'
ORDER BY
    ORDINAL_POSITION;
PRINT '';

-- Actividades
PRINT '>>> ACTIVIDADES';
SELECT
    COLUMN_NAME AS 'Columna',
    DATA_TYPE AS 'Tipo',
    CHARACTER_MAXIMUM_LENGTH AS 'Longitud',
    IS_NULLABLE AS 'Nullable'
FROM
    INFORMATION_SCHEMA.COLUMNS
WHERE
    TABLE_NAME = 'Actividades'
ORDER BY
    ORDINAL_POSITION;
PRINT '';

-- Tareas
PRINT '>>> TAREAS';
SELECT
    COLUMN_NAME AS 'Columna',
    DATA_TYPE AS 'Tipo',
    CHARACTER_MAXIMUM_LENGTH AS 'Longitud',
    IS_NULLABLE AS 'Nullable'
FROM
    INFORMATION_SCHEMA.COLUMNS
WHERE
    TABLE_NAME = 'Tareas'
ORDER BY
    ORDINAL_POSITION;
PRINT '';

-- Rubros
PRINT '>>> RUBROS';
SELECT
    COLUMN_NAME AS 'Columna',
    DATA_TYPE AS 'Tipo',
    CHARACTER_MAXIMUM_LENGTH AS 'Longitud',
    IS_NULLABLE AS 'Nullable'
FROM
    INFORMATION_SCHEMA.COLUMNS
WHERE
    TABLE_NAME = 'Rubros'
ORDER BY
    ORDINAL_POSITION;
PRINT '';

-- TalentoHumano
PRINT '>>> TALENTO HUMANO';
SELECT
    COLUMN_NAME AS 'Columna',
    DATA_TYPE AS 'Tipo',
    CHARACTER_MAXIMUM_LENGTH AS 'Longitud',
    IS_NULLABLE AS 'Nullable'
FROM
    INFORMATION_SCHEMA.COLUMNS
WHERE
    TABLE_NAME = 'TalentoHumano'
ORDER BY
    ORDINAL_POSITION;
PRINT '';

-- EquiposSoftware
PRINT '>>> EQUIPOS Y SOFTWARE';
SELECT
    COLUMN_NAME AS 'Columna',
    DATA_TYPE AS 'Tipo',
    CHARACTER_MAXIMUM_LENGTH AS 'Longitud',
    IS_NULLABLE AS 'Nullable'
FROM
    INFORMATION_SCHEMA.COLUMNS
WHERE
    TABLE_NAME = 'EquiposSoftware'
ORDER BY
    ORDINAL_POSITION;
PRINT '';

-- MaterialesInsumos
PRINT '>>> MATERIALES E INSUMOS';
SELECT
    COLUMN_NAME AS 'Columna',
    DATA_TYPE AS 'Tipo',
    CHARACTER_MAXIMUM_LENGTH AS 'Longitud',
    IS_NULLABLE AS 'Nullable'
FROM
    INFORMATION_SCHEMA.COLUMNS
WHERE
    TABLE_NAME = 'MaterialesInsumos'
ORDER BY
    ORDINAL_POSITION;
PRINT '';

-- ServiciosTecnologicos
PRINT '>>> SERVICIOS TECNOLOGICOS';
SELECT
    COLUMN_NAME AS 'Columna',
    DATA_TYPE AS 'Tipo',
    CHARACTER_MAXIMUM_LENGTH AS 'Longitud',
    IS_NULLABLE AS 'Nullable'
FROM
    INFORMATION_SCHEMA.COLUMNS
WHERE
    TABLE_NAME = 'ServiciosTecnologicos'
ORDER BY
    ORDINAL_POSITION;
PRINT '';

-- CapacitacionEventos
PRINT '>>> CAPACITACION Y EVENTOS';
SELECT
    COLUMN_NAME AS 'Columna',
    DATA_TYPE AS 'Tipo',
    CHARACTER_MAXIMUM_LENGTH AS 'Longitud',
    IS_NULLABLE AS 'Nullable'
FROM
    INFORMATION_SCHEMA.COLUMNS
WHERE
    TABLE_NAME = 'CapacitacionEventos'
ORDER BY
    ORDINAL_POSITION;
PRINT '';

-- GastosViaje
PRINT '>>> GASTOS DE VIAJE';
SELECT
    COLUMN_NAME AS 'Columna',
    DATA_TYPE AS 'Tipo',
    CHARACTER_MAXIMUM_LENGTH AS 'Longitud',
    IS_NULLABLE AS 'Nullable'
FROM
    INFORMATION_SCHEMA.COLUMNS
WHERE
    TABLE_NAME = 'GastosViaje'
ORDER BY
    ORDINAL_POSITION;
PRINT '';

-- Entidades
PRINT '>>> ENTIDADES';
SELECT
    COLUMN_NAME AS 'Columna',
    DATA_TYPE AS 'Tipo',
    CHARACTER_MAXIMUM_LENGTH AS 'Longitud',
    IS_NULLABLE AS 'Nullable'
FROM
    INFORMATION_SCHEMA.COLUMNS
WHERE
    TABLE_NAME = 'Entidades'
ORDER BY
    ORDINAL_POSITION;
PRINT '';

-- ActXEntidad
PRINT '>>> ACT X ENTIDAD';
SELECT
    COLUMN_NAME AS 'Columna',
    DATA_TYPE AS 'Tipo',
    CHARACTER_MAXIMUM_LENGTH AS 'Longitud',
    IS_NULLABLE AS 'Nullable'
FROM
    INFORMATION_SCHEMA.COLUMNS
WHERE
    TABLE_NAME = 'ActXEntidad'
ORDER BY
    ORDINAL_POSITION;
PRINT '';

-- CadenaDeValor
PRINT '>>> CADENA DE VALOR';
SELECT
    COLUMN_NAME AS 'Columna',
    DATA_TYPE AS 'Tipo',
    CHARACTER_MAXIMUM_LENGTH AS 'Longitud',
    IS_NULLABLE AS 'Nullable'
FROM
    INFORMATION_SCHEMA.COLUMNS
WHERE
    TABLE_NAME = 'CadenaDeValor'
ORDER BY
    ORDINAL_POSITION;
PRINT '';

-- 3. DATOS DE CADA TABLA
PRINT '3. DATOS DE LAS TABLAS:';
PRINT '----------------------------------------';

-- Users
PRINT '>>> USERS (Datos):';
SELECT * FROM Users;
PRINT '';

-- Roles
PRINT '>>> ROLES (Datos):';
SELECT * FROM Roles;
PRINT '';

-- Proyectos
PRINT '>>> PROYECTOS (Datos):';
SELECT * FROM Proyectos;
PRINT '';

-- Actividades
PRINT '>>> ACTIVIDADES (Datos):';
SELECT * FROM Actividades;
PRINT '';

-- Tareas
PRINT '>>> TAREAS (Datos):';
SELECT * FROM Tareas;
PRINT '';

-- Rubros
PRINT '>>> RUBROS (Datos):';
SELECT * FROM Rubros;
PRINT '';

-- TalentoHumano
PRINT '>>> TALENTO HUMANO (Datos):';
SELECT * FROM TalentoHumano;
PRINT '';

-- EquiposSoftware
PRINT '>>> EQUIPOS Y SOFTWARE (Datos):';
SELECT * FROM EquiposSoftware;
PRINT '';

-- MaterialesInsumos
PRINT '>>> MATERIALES E INSUMOS (Datos):';
SELECT * FROM MaterialesInsumos;
PRINT '';

-- ServiciosTecnologicos
PRINT '>>> SERVICIOS TECNOLOGICOS (Datos):';
SELECT * FROM ServiciosTecnologicos;
PRINT '';

-- CapacitacionEventos
PRINT '>>> CAPACITACION Y EVENTOS (Datos):';
SELECT * FROM CapacitacionEventos;
PRINT '';

-- GastosViaje
PRINT '>>> GASTOS DE VIAJE (Datos):';
SELECT * FROM GastosViaje;
PRINT '';

-- Entidades
PRINT '>>> ENTIDADES (Datos):';
SELECT * FROM Entidades;
PRINT '';

-- ActXEntidad
PRINT '>>> ACT X ENTIDAD (Datos):';
SELECT * FROM ActXEntidad;
PRINT '';

-- CadenaDeValor
PRINT '>>> CADENA DE VALOR (Datos):';
SELECT * FROM CadenaDeValor;
PRINT '';

PRINT '========================================';
PRINT 'FIN DEL REPORTE';
PRINT '========================================';
