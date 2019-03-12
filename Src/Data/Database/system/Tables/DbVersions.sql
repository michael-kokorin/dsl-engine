CREATE TABLE [system].[DbVersions] (
    [Id]      BIGINT         IDENTITY (1, 1) NOT NULL,
    [Version] INT            NOT NULL,
    [Module]  NVARCHAR (400) NOT NULL,
    CONSTRAINT [PK_DbVersions] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UK_DbVersions_Module] UNIQUE NONCLUSTERED ([Module] ASC)
);

