CREATE TABLE [system].[Tables] (
    [Id]                    BIGINT         IDENTITY (1, 1) NOT NULL,
    [Name]                  NVARCHAR (64)  NOT NULL,
    [Type]                  INT            NOT NULL,
    [DataSourceName]        NVARCHAR (64)  NOT NULL,
    [DataSourceDescription] NVARCHAR (MAX) NULL,
    [CultureId]             BIGINT         NOT NULL,
    CONSTRAINT [PK_Tables] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Tables_Cultures_CultureId] FOREIGN KEY ([CultureId]) REFERENCES [l10n].[Cultures] ([Id]),
    CONSTRAINT [UK_Tables_DataSourceName] UNIQUE NONCLUSTERED ([DataSourceName] ASC),
    CONSTRAINT [UK_Tables_Name] UNIQUE NONCLUSTERED ([Name] ASC)
);

