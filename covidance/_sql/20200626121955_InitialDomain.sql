IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [Person] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(128) NULL,
    [Email] nvarchar(128) NULL,
    [Deleted] bit NOT NULL,
    [UserId] nvarchar(450) NULL,
    CONSTRAINT [PK_Person] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Record] (
    [Id] uniqueidentifier NOT NULL,
    [PersonId] uniqueidentifier NOT NULL,
    [When] datetime2 NOT NULL,
    [DateCreated] datetime2 NOT NULL,
    [Temperature] float NOT NULL,
    [Symptoms] int NOT NULL,
    [RecentContact] bit NOT NULL,
    [Sanitised] bit NOT NULL,
    [Bagged] bit NOT NULL,
    [Reason] nvarchar(128) NULL,
    CONSTRAINT [PK_Record] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Record_Person_PersonId] FOREIGN KEY ([PersonId]) REFERENCES [Person] ([Id]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_Record_PersonId] ON [Record] ([PersonId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200626121955_InitialDomain', N'3.1.5');

GO

