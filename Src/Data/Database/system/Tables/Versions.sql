CREATE TABLE [system].[Versions] (
    [Id]          BIGINT         IDENTITY (1, 1) NOT NULL,
    [Version]     NVARCHAR (20)  NOT NULL,
    [Module]      NVARCHAR (400) NOT NULL,
    [Description] NVARCHAR (MAX) NOT NULL,
    [Installed]   DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_Versions] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UK_Versions_Module] UNIQUE NONCLUSTERED ([Module] ASC)
);

