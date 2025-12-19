IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250925054045_InitialCreate'
)
BEGIN
    CREATE TABLE [Users] (
        [Id] int NOT NULL IDENTITY,
        [FirstName] nvarchar(100) NOT NULL,
        [LastName] nvarchar(100) NOT NULL,
        [Email] nvarchar(255) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [IsActive] bit NOT NULL DEFAULT CAST(1 AS bit),
        CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250925054045_InitialCreate'
)
BEGIN
    CREATE TABLE [WeatherForecasts] (
        [Id] int NOT NULL IDENTITY,
        [Date] date NOT NULL,
        [TemperatureC] int NOT NULL,
        [Summary] nvarchar(100) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_WeatherForecasts] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250925054045_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'Email', N'FirstName', N'IsActive', N'LastName', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Users]'))
        SET IDENTITY_INSERT [Users] ON;
    EXEC(N'INSERT INTO [Users] ([Id], [CreatedAt], [Email], [FirstName], [IsActive], [LastName], [UpdatedAt])
    VALUES (1, ''2025-09-25T05:40:43.8051420Z'', N''juan.perez@email.com'', N''Juan'', CAST(1 AS bit), N''Pérez'', NULL),
    (2, ''2025-09-25T05:40:43.8051426Z'', N''maria.gonzalez@email.com'', N''María'', CAST(1 AS bit), N''González'', NULL)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'Email', N'FirstName', N'IsActive', N'LastName', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Users]'))
        SET IDENTITY_INSERT [Users] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250925054045_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'Email', N'FirstName', N'LastName', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Users]'))
        SET IDENTITY_INSERT [Users] ON;
    EXEC(N'INSERT INTO [Users] ([Id], [CreatedAt], [Email], [FirstName], [LastName], [UpdatedAt])
    VALUES (3, ''2025-09-25T05:40:43.8051429Z'', N''carlos.lopez@email.com'', N''Carlos'', N''López'', NULL)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'Email', N'FirstName', N'LastName', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Users]'))
        SET IDENTITY_INSERT [Users] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250925054045_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'Date', N'Summary', N'TemperatureC', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[WeatherForecasts]'))
        SET IDENTITY_INSERT [WeatherForecasts] ON;
    EXEC(N'INSERT INTO [WeatherForecasts] ([Id], [CreatedAt], [Date], [Summary], [TemperatureC], [UpdatedAt])
    VALUES (1, ''2025-09-25T05:40:43.8052054Z'', ''2025-09-26'', N''Bracing'', 30, NULL),
    (2, ''2025-09-25T05:40:43.8052066Z'', ''2025-09-27'', N''Warm'', -11, NULL),
    (3, ''2025-09-25T05:40:43.8052069Z'', ''2025-09-28'', N''Chilly'', -8, NULL),
    (4, ''2025-09-25T05:40:43.8052072Z'', ''2025-09-29'', N''Warm'', 34, NULL),
    (5, ''2025-09-25T05:40:43.8052075Z'', ''2025-09-30'', N''Hot'', -7, NULL),
    (6, ''2025-09-25T05:40:43.8052084Z'', ''2025-10-01'', N''Chilly'', -3, NULL),
    (7, ''2025-09-25T05:40:43.8052087Z'', ''2025-10-02'', N''Cool'', 17, NULL),
    (8, ''2025-09-25T05:40:43.8052090Z'', ''2025-10-03'', N''Chilly'', 8, NULL),
    (9, ''2025-09-25T05:40:43.8052093Z'', ''2025-10-04'', N''Freezing'', 18, NULL),
    (10, ''2025-09-25T05:40:43.8052098Z'', ''2025-10-05'', N''Warm'', 41, NULL)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'Date', N'Summary', N'TemperatureC', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[WeatherForecasts]'))
        SET IDENTITY_INSERT [WeatherForecasts] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250925054045_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Users_Email] ON [Users] ([Email]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250925054045_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250925054045_InitialCreate', N'8.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251002214006_AddRolesAndPasswordHash'
)
BEGIN
    ALTER TABLE [Users] ADD [PasswordHash] nvarchar(255) NOT NULL DEFAULT N'';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251002214006_AddRolesAndPasswordHash'
)
BEGIN
    ALTER TABLE [Users] ADD [RoleId] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251002214006_AddRolesAndPasswordHash'
)
BEGIN
    CREATE TABLE [Roles] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(50) NOT NULL,
        [Description] nvarchar(255) NOT NULL,
        [Permissions] nvarchar(1000) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [IsActive] bit NOT NULL DEFAULT CAST(1 AS bit),
        CONSTRAINT [PK_Roles] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251002214006_AddRolesAndPasswordHash'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'Description', N'IsActive', N'Name', N'Permissions', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Roles]'))
        SET IDENTITY_INSERT [Roles] ON;
    EXEC(N'INSERT INTO [Roles] ([Id], [CreatedAt], [Description], [IsActive], [Name], [Permissions], [UpdatedAt])
    VALUES (1, ''2025-10-02T21:40:04.5129767Z'', N''Rol con permisos completos del sistema'', CAST(1 AS bit), N''Administrador'', N''["users.create", "users.read", "users.update", "users.delete", "weather.create", "weather.read", "weather.update", "weather.delete"]'', NULL),
    (2, ''2025-10-02T21:40:04.5129782Z'', N''Rol con permisos básicos del sistema'', CAST(1 AS bit), N''Usuario'', N''["users.read", "weather.read"]'', NULL)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'Description', N'IsActive', N'Name', N'Permissions', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Roles]'))
        SET IDENTITY_INSERT [Roles] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251002214006_AddRolesAndPasswordHash'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [CreatedAt] = ''2025-10-02T21:40:04.8319716Z'', [PasswordHash] = N''$2a$11$z.mS7rxE2lXTZa/FLBRZQ.Mjb0ApUDKiZvGyRQzMwvjQQO/zZf4CC'', [RoleId] = 1
    WHERE [Id] = 1;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251002214006_AddRolesAndPasswordHash'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [CreatedAt] = ''2025-10-02T21:40:05.1857445Z'', [PasswordHash] = N''$2a$11$xL.4FeqN2HV.2tvvAcERY./i/GL3XDUQ7.LNAWJzRkFaWuMfrPLSK'', [RoleId] = 2
    WHERE [Id] = 2;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251002214006_AddRolesAndPasswordHash'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [CreatedAt] = ''2025-10-02T21:40:05.5456474Z'', [PasswordHash] = N''$2a$11$oAqO/zHXCIpmBrf.BmT3HuqNpfIPWfI03DmReZhYCBuS4HZG5HF4i'', [RoleId] = 2
    WHERE [Id] = 3;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251002214006_AddRolesAndPasswordHash'
)
BEGIN
    EXEC(N'UPDATE [WeatherForecasts] SET [CreatedAt] = ''2025-10-02T21:40:05.5457349Z'', [Date] = ''2025-10-03''
    WHERE [Id] = 1;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251002214006_AddRolesAndPasswordHash'
)
BEGIN
    EXEC(N'UPDATE [WeatherForecasts] SET [CreatedAt] = ''2025-10-02T21:40:05.5457377Z'', [Date] = ''2025-10-04''
    WHERE [Id] = 2;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251002214006_AddRolesAndPasswordHash'
)
BEGIN
    EXEC(N'UPDATE [WeatherForecasts] SET [CreatedAt] = ''2025-10-02T21:40:05.5457382Z'', [Date] = ''2025-10-05''
    WHERE [Id] = 3;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251002214006_AddRolesAndPasswordHash'
)
BEGIN
    EXEC(N'UPDATE [WeatherForecasts] SET [CreatedAt] = ''2025-10-02T21:40:05.5457387Z'', [Date] = ''2025-10-06''
    WHERE [Id] = 4;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251002214006_AddRolesAndPasswordHash'
)
BEGIN
    EXEC(N'UPDATE [WeatherForecasts] SET [CreatedAt] = ''2025-10-02T21:40:05.5457391Z'', [Date] = ''2025-10-07''
    WHERE [Id] = 5;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251002214006_AddRolesAndPasswordHash'
)
BEGIN
    EXEC(N'UPDATE [WeatherForecasts] SET [CreatedAt] = ''2025-10-02T21:40:05.5457412Z'', [Date] = ''2025-10-08''
    WHERE [Id] = 6;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251002214006_AddRolesAndPasswordHash'
)
BEGIN
    EXEC(N'UPDATE [WeatherForecasts] SET [CreatedAt] = ''2025-10-02T21:40:05.5457417Z'', [Date] = ''2025-10-09''
    WHERE [Id] = 7;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251002214006_AddRolesAndPasswordHash'
)
BEGIN
    EXEC(N'UPDATE [WeatherForecasts] SET [CreatedAt] = ''2025-10-02T21:40:05.5457489Z'', [Date] = ''2025-10-10''
    WHERE [Id] = 8;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251002214006_AddRolesAndPasswordHash'
)
BEGIN
    EXEC(N'UPDATE [WeatherForecasts] SET [CreatedAt] = ''2025-10-02T21:40:05.5457494Z'', [Date] = ''2025-10-11''
    WHERE [Id] = 9;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251002214006_AddRolesAndPasswordHash'
)
BEGIN
    EXEC(N'UPDATE [WeatherForecasts] SET [CreatedAt] = ''2025-10-02T21:40:05.5457503Z'', [Date] = ''2025-10-12''
    WHERE [Id] = 10;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251002214006_AddRolesAndPasswordHash'
)
BEGIN
    CREATE INDEX [IX_Users_RoleId] ON [Users] ([RoleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251002214006_AddRolesAndPasswordHash'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Roles_Name] ON [Roles] ([Name]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251002214006_AddRolesAndPasswordHash'
)
BEGIN
    ALTER TABLE [Users] ADD CONSTRAINT [FK_Users_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([Id]) ON DELETE NO ACTION;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251002214006_AddRolesAndPasswordHash'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251002214006_AddRolesAndPasswordHash', N'8.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251016012522_AddEntidadAndActXEntidad'
)
BEGIN
    CREATE TABLE [Entidad] (
        [EntidadId] int NOT NULL IDENTITY,
        [Nombre] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Entidad] PRIMARY KEY ([EntidadId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251016012522_AddEntidadAndActXEntidad'
)
BEGIN
    CREATE TABLE [ActXEntidades] (
        [ActXEntidadId] int NOT NULL IDENTITY,
        [ActividadId] int NOT NULL,
        [EntidadId] int NOT NULL,
        [Efectivo] decimal(18,2) NOT NULL,
        [Especie] decimal(18,2) NOT NULL,
        CONSTRAINT [PK_ActXEntidades] PRIMARY KEY ([ActXEntidadId]),
        CONSTRAINT [FK_ActXEntidades_Actividades_ActividadId] FOREIGN KEY ([ActividadId]) REFERENCES [Actividades] ([ActividadId]) ON DELETE CASCADE,
        CONSTRAINT [FK_ActXEntidades_Entidad_EntidadId] FOREIGN KEY ([EntidadId]) REFERENCES [Entidad] ([EntidadId]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251016012522_AddEntidadAndActXEntidad'
)
BEGIN
    EXEC(N'UPDATE [Roles] SET [CreatedAt] = ''2025-10-16T01:25:21.5904579Z''
    WHERE [Id] = 1;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251016012522_AddEntidadAndActXEntidad'
)
BEGIN
    EXEC(N'UPDATE [Roles] SET [CreatedAt] = ''2025-10-16T01:25:21.5904585Z''
    WHERE [Id] = 2;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251016012522_AddEntidadAndActXEntidad'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [CreatedAt] = ''2025-10-16T01:25:21.7152569Z'', [PasswordHash] = N''$2a$11$CpDuebgC7CL7gqNqc.N0XO3qEOvC3GWH7Q/7w9K65Yk42l9TT/5LC''
    WHERE [Id] = 1;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251016012522_AddEntidadAndActXEntidad'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [CreatedAt] = ''2025-10-16T01:25:21.8406675Z'', [PasswordHash] = N''$2a$11$YzCmqmPuq4hmsofPZaV0o.iIiGXFv8.Q.O56Cg1n80UsCx.TxjtyC''
    WHERE [Id] = 2;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251016012522_AddEntidadAndActXEntidad'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [CreatedAt] = ''2025-10-16T01:25:21.9642975Z'', [PasswordHash] = N''$2a$11$W/ZEyoh9LKd3MF1lcj1JWOMLwKcZEsyigWr4ieeuQAJPIY34NUAkW''
    WHERE [Id] = 3;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251016012522_AddEntidadAndActXEntidad'
)
BEGIN
    EXEC(N'UPDATE [WeatherForecasts] SET [CreatedAt] = ''2025-10-16T01:25:21.9643399Z'', [Date] = ''2025-10-16''
    WHERE [Id] = 1;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251016012522_AddEntidadAndActXEntidad'
)
BEGIN
    EXEC(N'UPDATE [WeatherForecasts] SET [CreatedAt] = ''2025-10-16T01:25:21.9643413Z'', [Date] = ''2025-10-17''
    WHERE [Id] = 2;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251016012522_AddEntidadAndActXEntidad'
)
BEGIN
    EXEC(N'UPDATE [WeatherForecasts] SET [CreatedAt] = ''2025-10-16T01:25:21.9643415Z'', [Date] = ''2025-10-18''
    WHERE [Id] = 3;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251016012522_AddEntidadAndActXEntidad'
)
BEGIN
    EXEC(N'UPDATE [WeatherForecasts] SET [CreatedAt] = ''2025-10-16T01:25:21.9643416Z'', [Date] = ''2025-10-19''
    WHERE [Id] = 4;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251016012522_AddEntidadAndActXEntidad'
)
BEGIN
    EXEC(N'UPDATE [WeatherForecasts] SET [CreatedAt] = ''2025-10-16T01:25:21.9643417Z'', [Date] = ''2025-10-20''
    WHERE [Id] = 5;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251016012522_AddEntidadAndActXEntidad'
)
BEGIN
    EXEC(N'UPDATE [WeatherForecasts] SET [CreatedAt] = ''2025-10-16T01:25:21.9643427Z'', [Date] = ''2025-10-21''
    WHERE [Id] = 6;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251016012522_AddEntidadAndActXEntidad'
)
BEGIN
    EXEC(N'UPDATE [WeatherForecasts] SET [CreatedAt] = ''2025-10-16T01:25:21.9643428Z'', [Date] = ''2025-10-22''
    WHERE [Id] = 7;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251016012522_AddEntidadAndActXEntidad'
)
BEGIN
    EXEC(N'UPDATE [WeatherForecasts] SET [CreatedAt] = ''2025-10-16T01:25:21.9643429Z'', [Date] = ''2025-10-23''
    WHERE [Id] = 8;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251016012522_AddEntidadAndActXEntidad'
)
BEGIN
    EXEC(N'UPDATE [WeatherForecasts] SET [CreatedAt] = ''2025-10-16T01:25:21.9643430Z'', [Date] = ''2025-10-24''
    WHERE [Id] = 9;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251016012522_AddEntidadAndActXEntidad'
)
BEGIN
    EXEC(N'UPDATE [WeatherForecasts] SET [CreatedAt] = ''2025-10-16T01:25:21.9643432Z'', [Date] = ''2025-10-25''
    WHERE [Id] = 10;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251016012522_AddEntidadAndActXEntidad'
)
BEGIN
    CREATE INDEX [IX_ActXEntidades_ActividadId] ON [ActXEntidades] ([ActividadId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251016012522_AddEntidadAndActXEntidad'
)
BEGIN
    CREATE INDEX [IX_ActXEntidades_EntidadId] ON [ActXEntidades] ([EntidadId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251016012522_AddEntidadAndActXEntidad'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251016012522_AddEntidadAndActXEntidad', N'8.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251016034621_AddTareasTable'
)
BEGIN
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
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251016034621_AddTareasTable'
)
BEGIN
                    IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Entidades') AND name = 'EntidadId')
                        EXEC sp_rename N'dbo.Entidades.EntidadId', N'Id_Entidad', N'COLUMN';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251016034621_AddTareasTable'
)
BEGIN
    ALTER TABLE [Actividades] ADD [CadenaDeValorId] int NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251016034621_AddTareasTable'
)
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Entidades]') AND [c].[name] = N'Nombre');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Entidades] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [Entidades] ALTER COLUMN [Nombre] nvarchar(255) NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251016034621_AddTareasTable'
)
BEGIN
                    IF NOT EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = 'PK_ActxEntidades' AND parent_object_id = OBJECT_ID('ActxEntidades'))
                    BEGIN
                        ALTER TABLE [ActxEntidades] ADD CONSTRAINT [PK_ActxEntidades] PRIMARY KEY ([Id]);
                    END
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251016034621_AddTareasTable'
)
BEGIN
                    IF NOT EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = 'PK_Entidades' AND parent_object_id = OBJECT_ID('Entidades'))
                    BEGIN
                        ALTER TABLE [Entidades] ADD CONSTRAINT [PK_Entidades] PRIMARY KEY ([Id_Entidad]);
                    END
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251016034621_AddTareasTable'
)
BEGIN
    CREATE TABLE [CadenasDeValor] (
        [CadenaDeValorId] int NOT NULL IDENTITY,
        [Nombre] nvarchar(255) NOT NULL,
        [ObjetivoEspecifico] nvarchar(1000) NOT NULL,
        [Producto] nvarchar(255) NOT NULL,
        CONSTRAINT [PK_CadenasDeValor] PRIMARY KEY ([CadenaDeValorId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251016034621_AddTareasTable'
)
BEGIN
    CREATE TABLE [Tareas] (
        [TareaId] int NOT NULL IDENTITY,
        [Nombre] nvarchar(255) NOT NULL,
        [Descripcion] nvarchar(1000) NOT NULL,
        [Periodo] nvarchar(100) NOT NULL,
        [Monto] decimal(18,2) NOT NULL,
        [ActividadId] int NOT NULL,
        CONSTRAINT [PK_Tareas] PRIMARY KEY ([TareaId]),
        CONSTRAINT [FK_Tareas_Actividades_ActividadId] FOREIGN KEY ([ActividadId]) REFERENCES [Actividades] ([ActividadId]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251016034621_AddTareasTable'
)
BEGIN
    EXEC(N'UPDATE [Roles] SET [CreatedAt] = ''2025-10-16T03:46:20.6996154Z''
    WHERE [Id] = 1;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251016034621_AddTareasTable'
)
BEGIN
    EXEC(N'UPDATE [Roles] SET [CreatedAt] = ''2025-10-16T03:46:20.6996158Z''
    WHERE [Id] = 2;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251016034621_AddTareasTable'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [CreatedAt] = ''2025-10-16T03:46:20.8193993Z'', [PasswordHash] = N''$2a$11$xY0/3VJnZriSqb4bwYwtVerjBplO1vhBN1yuw8v5ZKjSF0/JUhJja''
    WHERE [Id] = 1;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251016034621_AddTareasTable'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [CreatedAt] = ''2025-10-16T03:46:20.9419608Z'', [PasswordHash] = N''$2a$11$doH/D3dtT4fiEF35PHxS2u3ZJPIP5QoB0OStAYdXd5b.hhe1tUXcC''
    WHERE [Id] = 2;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251016034621_AddTareasTable'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [CreatedAt] = ''2025-10-16T03:46:21.0638992Z'', [PasswordHash] = N''$2a$11$M0O.vH8dmyeTLVLTHU.HBugXnNQzlkeOg9T8Cd.N.mQ8qlWOQ0Hvi''
    WHERE [Id] = 3;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251016034621_AddTareasTable'
)
BEGIN
    EXEC(N'UPDATE [WeatherForecasts] SET [CreatedAt] = ''2025-10-16T03:46:21.0639378Z''
    WHERE [Id] = 1;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251016034621_AddTareasTable'
)
BEGIN
    EXEC(N'UPDATE [WeatherForecasts] SET [CreatedAt] = ''2025-10-16T03:46:21.0639390Z''
    WHERE [Id] = 2;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251016034621_AddTareasTable'
)
BEGIN
    EXEC(N'UPDATE [WeatherForecasts] SET [CreatedAt] = ''2025-10-16T03:46:21.0639391Z''
    WHERE [Id] = 3;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251016034621_AddTareasTable'
)
BEGIN
    EXEC(N'UPDATE [WeatherForecasts] SET [CreatedAt] = ''2025-10-16T03:46:21.0639392Z''
    WHERE [Id] = 4;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251016034621_AddTareasTable'
)
BEGIN
    EXEC(N'UPDATE [WeatherForecasts] SET [CreatedAt] = ''2025-10-16T03:46:21.0639393Z''
    WHERE [Id] = 5;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251016034621_AddTareasTable'
)
BEGIN
    EXEC(N'UPDATE [WeatherForecasts] SET [CreatedAt] = ''2025-10-16T03:46:21.0639403Z''
    WHERE [Id] = 6;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251016034621_AddTareasTable'
)
BEGIN
    EXEC(N'UPDATE [WeatherForecasts] SET [CreatedAt] = ''2025-10-16T03:46:21.0639404Z''
    WHERE [Id] = 7;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251016034621_AddTareasTable'
)
BEGIN
    EXEC(N'UPDATE [WeatherForecasts] SET [CreatedAt] = ''2025-10-16T03:46:21.0639405Z''
    WHERE [Id] = 8;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251016034621_AddTareasTable'
)
BEGIN
    EXEC(N'UPDATE [WeatherForecasts] SET [CreatedAt] = ''2025-10-16T03:46:21.0639406Z''
    WHERE [Id] = 9;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251016034621_AddTareasTable'
)
BEGIN
    EXEC(N'UPDATE [WeatherForecasts] SET [CreatedAt] = ''2025-10-16T03:46:21.0639408Z''
    WHERE [Id] = 10;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251016034621_AddTareasTable'
)
BEGIN
    CREATE INDEX [IX_Actividades_CadenaDeValorId] ON [Actividades] ([CadenaDeValorId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251016034621_AddTareasTable'
)
BEGIN
    CREATE INDEX [IX_Tareas_ActividadId] ON [Tareas] ([ActividadId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251016034621_AddTareasTable'
)
BEGIN
    ALTER TABLE [Actividades] ADD CONSTRAINT [FK_Actividades_CadenasDeValor_CadenaDeValorId] FOREIGN KEY ([CadenaDeValorId]) REFERENCES [CadenasDeValor] ([CadenaDeValorId]) ON DELETE SET NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251016034621_AddTareasTable'
)
BEGIN
                    IF NOT EXISTS (
                        SELECT 1 FROM ActxEntidades a
                        LEFT JOIN Actividades act ON a.act_id = act.ActividadId
                        WHERE act.ActividadId IS NULL
                    )
                    BEGIN
                        ALTER TABLE [ActxEntidades] ADD CONSTRAINT [FK_ActxEntidades_Actividades_act_id]
                        FOREIGN KEY ([act_id]) REFERENCES [Actividades] ([ActividadId]) ON DELETE CASCADE;
                    END
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251016034621_AddTareasTable'
)
BEGIN
                    IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_ActxEntidades_Entidades_entidad_id')
                    BEGIN
                        ALTER TABLE [ActxEntidades] ADD CONSTRAINT [FK_ActxEntidades_Entidades_entidad_id]
                        FOREIGN KEY ([entidad_id]) REFERENCES [Entidades] ([Id_Entidad]) ON DELETE CASCADE;
                    END
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251016034621_AddTareasTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251016034621_AddTareasTable', N'8.0.0');
END;
GO

COMMIT;
GO

