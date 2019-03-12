CREATE TABLE [common].[Configuration] (
    [Id]    BIGINT         IDENTITY (1, 1) NOT NULL,
    [Name]  NVARCHAR (255) NOT NULL,
    [Value] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Configuration] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UK_Configuration_Name] UNIQUE NONCLUSTERED ([Name] ASC)
);

