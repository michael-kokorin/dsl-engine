﻿CREATE TABLE [l10n].[Cultures] (
    [Id]   BIGINT        IDENTITY (1, 1) NOT NULL,
    [Code] NVARCHAR (5)  NOT NULL,
    [Name] NVARCHAR (32) NOT NULL,
    CONSTRAINT [PK_Cultures] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UK_Cultures_Code] UNIQUE NONCLUSTERED ([Code] ASC)
);

