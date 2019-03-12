CREATE TABLE [system].[TableColumns_l10n] (
    [Id]               BIGINT         IDENTITY (1, 1) NOT NULL,
    [SourceId]         BIGINT         NOT NULL,
    [FieldName]        NVARCHAR (64)  NOT NULL,
    [FieldDescription] NVARCHAR (MAX) NULL,
    [CultureId]        BIGINT         NOT NULL,
    CONSTRAINT [PK_TableColumns_l10n] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_TableColumns_l10n_Cultures_CultureId] FOREIGN KEY ([CultureId]) REFERENCES [l10n].[Cultures] ([Id]),
    CONSTRAINT [FK_TableColumns_l10n_TableColumns_SourceId] FOREIGN KEY ([SourceId]) REFERENCES [system].[TableColumns] ([Id]),
    CONSTRAINT [UK_TableColumns_l10n_SourceId_CultureId] UNIQUE NONCLUSTERED ([SourceId] ASC, [CultureId] ASC)
);



