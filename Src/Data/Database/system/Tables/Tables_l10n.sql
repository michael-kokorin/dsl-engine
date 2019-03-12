CREATE TABLE [system].[Tables_l10n] (
    [Id]                    BIGINT         IDENTITY (1, 1) NOT NULL,
    [SourceId]              BIGINT         NOT NULL,
    [DataSourceName]        NVARCHAR (64)  NOT NULL,
    [DataSourceDescription] NVARCHAR (MAX) NULL,
    [CultureId]             BIGINT         NOT NULL,
    CONSTRAINT [PK_Tables_l10n] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Tables_l10n_Cultures_CultureId] FOREIGN KEY ([CultureId]) REFERENCES [l10n].[Cultures] ([Id]),
    CONSTRAINT [FK_Tables_l10n_Tables_SourceId] FOREIGN KEY ([SourceId]) REFERENCES [system].[Tables] ([Id]),
    CONSTRAINT [UK_Tables_l10n_SourceId_CultureId] UNIQUE NONCLUSTERED ([SourceId] ASC, [CultureId] ASC)
);



