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

CREATE TABLE [Book] (
    [Id] int NOT NULL IDENTITY,
    [Isbn] nvarchar(17) NOT NULL,
    [Name] nvarchar(128) NOT NULL,
    [Genre] nvarchar(64) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [Author] nvarchar(64) NOT NULL,
    [DateBorrowed] date NULL,
    [ReturningDate] date NULL,
    CONSTRAINT [PK_Book] PRIMARY KEY ([Id])
    );
GO

CREATE INDEX [IX_Book_Isbn] ON [Book] ([Isbn]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20231113120025_initial_appcontext', N'7.0.13');
GO

COMMIT;
GO
