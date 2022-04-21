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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220408210301_Initial')
BEGIN
    CREATE TABLE [AspNetRoles] (
        [Id] nvarchar(450) NOT NULL,
        [Name] nvarchar(256) NULL,
        [NormalizedName] nvarchar(256) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220408210301_Initial')
BEGIN
    CREATE TABLE [Companies] (
        [IdCompany] int NOT NULL IDENTITY,
        [NameCompany] nvarchar(max) NOT NULL,
        [NitCompany] nvarchar(450) NOT NULL,
        [DocumentCompany] nvarchar(max) NULL,
        CONSTRAINT [PK_Companies] PRIMARY KEY ([IdCompany])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220408210301_Initial')
BEGIN
    CREATE TABLE [TypeConveyors] (
        [IdTypeConveyor] int NOT NULL IDENTITY,
        [NameTypeConveyor] nvarchar(max) NOT NULL,
        [DescriptionTypeConveyor] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_TypeConveyors] PRIMARY KEY ([IdTypeConveyor])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220408210301_Initial')
BEGIN
    CREATE TABLE [users] (
        [Id] nvarchar(450) NOT NULL,
        [FirstName] nvarchar(100) NOT NULL,
        [LastName] nvarchar(100) NOT NULL,
        [Document] nvarchar(15) NOT NULL,
        [Status] bit NOT NULL,
        [Slug] nvarchar(15) NOT NULL,
        [UserName] nvarchar(256) NULL,
        [NormalizedUserName] nvarchar(256) NULL,
        [Email] nvarchar(256) NULL,
        [NormalizedEmail] nvarchar(256) NULL,
        [EmailConfirmed] bit NOT NULL,
        [PasswordHash] nvarchar(max) NULL,
        [SecurityStamp] nvarchar(max) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        [PhoneNumber] nvarchar(max) NULL,
        [PhoneNumberConfirmed] bit NOT NULL,
        [TwoFactorEnabled] bit NOT NULL,
        [LockoutEnd] datetimeoffset NULL,
        [LockoutEnabled] bit NOT NULL,
        [AccessFailedCount] int NOT NULL,
        CONSTRAINT [PK_users] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220408210301_Initial')
BEGIN
    CREATE TABLE [AspNetRoleClaims] (
        [Id] int NOT NULL IDENTITY,
        [RoleId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220408210301_Initial')
BEGIN
    CREATE TABLE [AspNetUserClaims] (
        [Id] int NOT NULL IDENTITY,
        [UserId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetUserClaims_users_UserId] FOREIGN KEY ([UserId]) REFERENCES [users] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220408210301_Initial')
BEGIN
    CREATE TABLE [AspNetUserLogins] (
        [LoginProvider] nvarchar(450) NOT NULL,
        [ProviderKey] nvarchar(450) NOT NULL,
        [ProviderDisplayName] nvarchar(max) NULL,
        [UserId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
        CONSTRAINT [FK_AspNetUserLogins_users_UserId] FOREIGN KEY ([UserId]) REFERENCES [users] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220408210301_Initial')
BEGIN
    CREATE TABLE [AspNetUserRoles] (
        [UserId] nvarchar(450) NOT NULL,
        [RoleId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AspNetUserRoles_users_UserId] FOREIGN KEY ([UserId]) REFERENCES [users] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220408210301_Initial')
BEGIN
    CREATE TABLE [AspNetUserTokens] (
        [UserId] nvarchar(450) NOT NULL,
        [LoginProvider] nvarchar(450) NOT NULL,
        [Name] nvarchar(450) NOT NULL,
        [Value] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
        CONSTRAINT [FK_AspNetUserTokens_users_UserId] FOREIGN KEY ([UserId]) REFERENCES [users] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220408210301_Initial')
BEGIN
    CREATE TABLE [Conveyors] (
        [IdConveyor] int NOT NULL IDENTITY,
        [CodeSap] nvarchar(max) NOT NULL,
        [TypeConveyorId] int NOT NULL,
        [CompanyId] int NULL,
        [UserId] nvarchar(450) NULL,
        CONSTRAINT [PK_Conveyors] PRIMARY KEY ([IdConveyor]),
        CONSTRAINT [FK_Conveyors_Companies_CompanyId] FOREIGN KEY ([CompanyId]) REFERENCES [Companies] ([IdCompany]),
        CONSTRAINT [FK_Conveyors_TypeConveyors_TypeConveyorId] FOREIGN KEY ([TypeConveyorId]) REFERENCES [TypeConveyors] ([IdTypeConveyor]) ON DELETE CASCADE,
        CONSTRAINT [FK_Conveyors_users_UserId] FOREIGN KEY ([UserId]) REFERENCES [users] ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220408210301_Initial')
BEGIN
    CREATE TABLE [Drivers] (
        [IdDriver] int NOT NULL IDENTITY,
        [UserId] nvarchar(450) NULL,
        [TypeConveyorId] int NULL,
        [ConveyorId] int NULL,
        [CodeSap] nvarchar(max) NOT NULL,
        [Status] bit NOT NULL,
        [DocumentDrivinglicenseFrontal] nvarchar(max) NULL,
        [DocumentDrivinglicenseBack] nvarchar(max) NULL,
        [DocumentIdentityCardFrontal] nvarchar(max) NULL,
        [DocumentIdentityCardBack] nvarchar(max) NULL,
        CONSTRAINT [PK_Drivers] PRIMARY KEY ([IdDriver]),
        CONSTRAINT [FK_Drivers_Conveyors_ConveyorId] FOREIGN KEY ([ConveyorId]) REFERENCES [Conveyors] ([IdConveyor]),
        CONSTRAINT [FK_Drivers_TypeConveyors_TypeConveyorId] FOREIGN KEY ([TypeConveyorId]) REFERENCES [TypeConveyors] ([IdTypeConveyor]),
        CONSTRAINT [FK_Drivers_users_UserId] FOREIGN KEY ([UserId]) REFERENCES [users] ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220408210301_Initial')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
        SET IDENTITY_INSERT [AspNetRoles] ON;
    EXEC(N'INSERT INTO [AspNetRoles] ([Id], [ConcurrencyStamp], [Name], [NormalizedName])
    VALUES (N''17362988-fbd2-448a-a1d7-fa4a87047dd3'', N''b5d89cb3-ff38-4161-aa89-3e6d764be9d6'', N''PoweUser'', N''POWERUSER''),
    (N''70773e6b-5767-4362-81dd-f5ed1187495d'', N''508ea9a0-b139-433d-bd2c-24125f594199'', N''ManTruck'', N''MANTRUCK''),
    (N''7a600ee4-8744-4eb9-8ec9-b5abb23013ca'', N''8451e44c-318f-40b7-99f5-11a965e19ebb'', N''CemexAdminLogis'', N''CEMEXADMINLOGIS''),
    (N''abe634a4-7d0b-4bb7-8af5-26426080a107'', N''9009b339-6091-4227-acf4-65c02435dd39'', N''AdminLogis'', N''ADMINLOGIS''),
    (N''f4fc722a-a40d-4e0e-899c-b8b8514c929d'', N''d1cdf8fc-9599-44c8-a0b3-8cab082a222a'', N''Driver'', N''DRIVER'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
        SET IDENTITY_INSERT [AspNetRoles] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220408210301_Initial')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'IdCompany', N'DocumentCompany', N'NameCompany', N'NitCompany') AND [object_id] = OBJECT_ID(N'[Companies]'))
        SET IDENTITY_INSERT [Companies] ON;
    EXEC(N'INSERT INTO [Companies] ([IdCompany], [DocumentCompany], [NameCompany], [NitCompany])
    VALUES (1, NULL, N''Sevi Transporte'', N''123121-212''),
    (2, NULL, N''Entregas SAS'', N''34341-982''),
    (3, NULL, N''Carga Segura'', N''431231-12'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'IdCompany', N'DocumentCompany', N'NameCompany', N'NitCompany') AND [object_id] = OBJECT_ID(N'[Companies]'))
        SET IDENTITY_INSERT [Companies] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220408210301_Initial')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'IdTypeConveyor', N'DescriptionTypeConveyor', N'NameTypeConveyor') AND [object_id] = OBJECT_ID(N'[TypeConveyors]'))
        SET IDENTITY_INSERT [TypeConveyors] ON;
    EXEC(N'INSERT INTO [TypeConveyors] ([IdTypeConveyor], [DescriptionTypeConveyor], [NameTypeConveyor])
    VALUES (1, N''Son empresas de transporte de carga que cuentan con una flota que supera los 5 camiones.'', N''AdminLogis''),
    (2, N''Son microempresas de transporte que tienen entre 1 a 5 camiones en su flota.'', N''ManTruck''),
    (3, N''Encargado de conducir el camión y su responsabilidad es cumplir con el itinerario de viajes asignados.'', N''Driver'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'IdTypeConveyor', N'DescriptionTypeConveyor', N'NameTypeConveyor') AND [object_id] = OBJECT_ID(N'[TypeConveyors]'))
        SET IDENTITY_INSERT [TypeConveyors] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220408210301_Initial')
BEGIN
    CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220408210301_Initial')
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220408210301_Initial')
BEGIN
    CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220408210301_Initial')
BEGIN
    CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220408210301_Initial')
BEGIN
    CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220408210301_Initial')
BEGIN
    CREATE UNIQUE INDEX [IX_Companies_NitCompany] ON [Companies] ([NitCompany]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220408210301_Initial')
BEGIN
    CREATE INDEX [IX_Conveyors_CompanyId] ON [Conveyors] ([CompanyId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220408210301_Initial')
BEGIN
    CREATE INDEX [IX_Conveyors_TypeConveyorId] ON [Conveyors] ([TypeConveyorId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220408210301_Initial')
BEGIN
    CREATE INDEX [IX_Conveyors_UserId] ON [Conveyors] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220408210301_Initial')
BEGIN
    CREATE INDEX [IX_Drivers_ConveyorId] ON [Drivers] ([ConveyorId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220408210301_Initial')
BEGIN
    CREATE INDEX [IX_Drivers_TypeConveyorId] ON [Drivers] ([TypeConveyorId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220408210301_Initial')
BEGIN
    CREATE INDEX [IX_Drivers_UserId] ON [Drivers] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220408210301_Initial')
BEGIN
    CREATE INDEX [EmailIndex] ON [users] ([NormalizedEmail]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220408210301_Initial')
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [UserNameIndex] ON [users] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220408210301_Initial')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220408210301_Initial', N'6.0.3');
END;
GO

COMMIT;
GO

