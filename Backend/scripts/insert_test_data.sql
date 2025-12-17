-- Script para insertar datos de prueba en la base de datos
-- Este script es útil cuando la base de datos está vacía y necesitas probar los endpoints
-- Incluye toda la cadena de dependencias: Usuario → Proyecto → Objetivo → CadenaDeValor → Actividad → Entidad → Rubro → Recurso → RecursoEspecifico

USE ProyectoIntegradorDb;
GO

-- 1. Verificar si existe algún usuario
IF NOT EXISTS (SELECT 1 FROM Users WHERE UserId = 1)
BEGIN
    -- Insertar un usuario de prueba (necesario para Proyectos)
    SET IDENTITY_INSERT Users ON;
    INSERT INTO Users (UserId, FirstName, LastName, Email, PasswordHash, IsActive, CreatedAt)
    VALUES (1, 'Usuario', 'Prueba', 'test@test.com', 'hash_temp', 1, GETDATE());
    SET IDENTITY_INSERT Users OFF;
    PRINT 'Usuario de prueba insertado (UserId = 1)';
END
ELSE
BEGIN
    PRINT 'Ya existe un usuario con UserId = 1';
END
GO

-- 2. Insertar un Proyecto de prueba
IF NOT EXISTS (SELECT 1 FROM Proyectos WHERE ProyectoId = 1)
BEGIN
    SET IDENTITY_INSERT Proyectos ON;
    INSERT INTO Proyectos (ProyectoId, Nombre, FechaCreacion, Estado, UsuarioId)
    VALUES (1, 'Proyecto de Prueba', GETDATE(), 'Activo', 1);
    SET IDENTITY_INSERT Proyectos OFF;
    PRINT 'Proyecto de prueba insertado (ProyectoId = 1)';
END
ELSE
BEGIN
    PRINT 'Ya existe un proyecto con ProyectoId = 1';
END
GO

-- 3. Insertar un Objetivo de prueba
IF NOT EXISTS (SELECT 1 FROM Objetivos WHERE ObjetivoId = 1)
BEGIN
    SET IDENTITY_INSERT Objetivos ON;
    INSERT INTO Objetivos (ObjetivoId, ProyectoId, Nombre, Descripcion, ResultadoEsperado)
    VALUES (1, 1, 'Objetivo General', 'Descripción del objetivo', 'Resultado esperado del objetivo');
    SET IDENTITY_INSERT Objetivos OFF;
    PRINT 'Objetivo de prueba insertado (ObjetivoId = 1)';
END
ELSE
BEGIN
    PRINT 'Ya existe un objetivo con ObjetivoId = 1';
END
GO

-- 4. Insertar una CadenaDeValor de prueba
IF NOT EXISTS (SELECT 1 FROM CadenasDeValor WHERE CadenaDeValorId = 1)
BEGIN
    SET IDENTITY_INSERT CadenasDeValor ON;
    INSERT INTO CadenasDeValor (CadenaDeValorId, ObjetivoId, Nombre, Descripcion)
    VALUES (1, 1, 'Cadena de Valor Principal', 'Descripción de la cadena de valor');
    SET IDENTITY_INSERT CadenasDeValor OFF;
    PRINT 'CadenaDeValor de prueba insertada (CadenaDeValorId = 1)';
END
ELSE
BEGIN
    PRINT 'Ya existe una cadena de valor con CadenaDeValorId = 1';
END
GO

-- 5. Insertar una Actividad de prueba
IF NOT EXISTS (SELECT 1 FROM Actividades WHERE ActividadId = 1)
BEGIN
    SET IDENTITY_INSERT Actividades ON;
    INSERT INTO Actividades (ActividadId, CadenaDeValorId, Nombre, Descripcion, Justificacion, 
                             EspecificacionesTecnicas, CantidadAnios, ValorUnitario)
    VALUES (1, 1, 'Actividad Principal', 'Descripción de la actividad', 'Justificación de la actividad',
            'Especificaciones técnicas', 2, 1000.00);
    SET IDENTITY_INSERT Actividades OFF;
    PRINT 'Actividad de prueba insertada (ActividadId = 1)';
END
ELSE
BEGIN
    PRINT 'Ya existe una actividad con ActividadId = 1';
END
GO

-- 6. Insertar una Entidad de prueba
IF NOT EXISTS (SELECT 1 FROM Entidades WHERE EntidadId = 1)
BEGIN
    SET IDENTITY_INSERT Entidades ON;
    INSERT INTO Entidades (EntidadId, Nombre, Tipo, Contacto)
    VALUES (1, 'Entidad Colaboradora', 'Externa', 'contacto@entidad.com');
    SET IDENTITY_INSERT Entidades OFF;
    PRINT 'Entidad de prueba insertada (EntidadId = 1)';
END
ELSE
BEGIN
    PRINT 'Ya existe una entidad con EntidadId = 1';
END
GO

-- 7. Insertar un Rubro de prueba
IF NOT EXISTS (SELECT 1 FROM Rubros WHERE RubroId = 1)
BEGIN
    SET IDENTITY_INSERT Rubros ON;
    INSERT INTO Rubros (RubroId, Descripcion)
    VALUES (1, 'Talento Humano');
    SET IDENTITY_INSERT Rubros OFF;
    PRINT 'Rubro de prueba insertado (RubroId = 1)';
END
ELSE
BEGIN
    PRINT 'Ya existe un rubro con RubroId = 1';
END
GO

-- 8. Insertar un Recurso de prueba
IF NOT EXISTS (SELECT 1 FROM Recursos WHERE RecursoId = 1)
BEGIN
    SET IDENTITY_INSERT Recursos ON;
    INSERT INTO Recursos (RecursoId, ActividadId, EntidadId, RubroId, TipoRecurso, 
                          MontoEfectivo, MontoEspecie, Descripcion)
    VALUES (1, 1, 1, 1, 'Humano', 50000.00, 10000.00, 'Recurso humano especializado');
    SET IDENTITY_INSERT Recursos OFF;
    PRINT 'Recurso de prueba insertado (RecursoId = 1)';
END
ELSE
BEGIN
    PRINT 'Ya existe un recurso con RecursoId = 1';
END
GO

-- 9. Verificar datos insertados
SELECT 'Usuarios' as Tabla, COUNT(*) as Total FROM Users
UNION ALL
SELECT 'Proyectos', COUNT(*) FROM Proyectos
UNION ALL
SELECT 'Objetivos', COUNT(*) FROM Objetivos
UNION ALL
SELECT 'CadenasDeValor', COUNT(*) FROM CadenasDeValor
UNION ALL
SELECT 'Actividades', COUNT(*) FROM Actividades
UNION ALL
SELECT 'Entidades', COUNT(*) FROM Entidades
UNION ALL
SELECT 'Rubros', COUNT(*) FROM Rubros
UNION ALL
SELECT 'Recursos', COUNT(*) FROM Recursos
UNION ALL
SELECT 'RecursosEspecificos', COUNT(*) FROM RecursosEspecificos;
GO

PRINT '========================================';
PRINT 'Datos de prueba listos!';
PRINT 'Ahora puedes:';
PRINT '1. Crear RecursoEspecifico usando RecursoId = 1';
PRINT '2. Crear Objetivos usando ProyectoId = 1';
PRINT '3. Crear CadenasDeValor usando ObjetivoId = 1';
PRINT '4. Crear Actividades usando CadenaDeValorId = 1';
PRINT '5. Crear Recursos usando ActividadId = 1, EntidadId = 1, RubroId = 1';
PRINT '========================================';
