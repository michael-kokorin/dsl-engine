CREATE TABLE [system].[TableColumns] (
    [Id]               BIGINT         IDENTITY (1, 1) NOT NULL,
    [TableId]          BIGINT         NOT NULL,
    [ReferenceTableId] BIGINT         NULL,
    [Name]             NVARCHAR (64)  NOT NULL,
    [Type]             INT            NOT NULL,
    [FieldName]        NVARCHAR (64)  NOT NULL,
    [FieldDescription] NVARCHAR (MAX) NULL,
    [FieldType]        INT            NOT NULL,
    [FieldDataType]    INT            NOT NULL,
    [CultureId]        BIGINT         NOT NULL,
    CONSTRAINT [PK_TableColumns] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_TableColumns_Cultures_CultureId] FOREIGN KEY ([CultureId]) REFERENCES [l10n].[Cultures] ([Id]),
    CONSTRAINT [FK_TableColumns_Tables_ReferenceTableId] FOREIGN KEY ([ReferenceTableId]) REFERENCES [system].[Tables] ([Id]),
    CONSTRAINT [FK_TableColumns_Tables_TableId] FOREIGN KEY ([TableId]) REFERENCES [system].[Tables] ([Id]),
    CONSTRAINT [UK_TableColumns_TableId_FieldName] UNIQUE NONCLUSTERED ([TableId] ASC, [FieldName] ASC),
    CONSTRAINT [UK_TableColumns_TableId_Name] UNIQUE NONCLUSTERED ([TableId] ASC, [Name] ASC)
);

