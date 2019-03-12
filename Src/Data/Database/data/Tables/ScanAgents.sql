CREATE TABLE [data].[ScanAgents] (
    [Id]              BIGINT         IDENTITY (1, 1) NOT NULL,
    [Version]         NVARCHAR (100) NOT NULL,
    [Machine]         NVARCHAR (400) NOT NULL,
    [Uid]             NVARCHAR (64)  NOT NULL,
    [AssemblyVersion] NVARCHAR (10)  NOT NULL,
    CONSTRAINT [PK_ScanAgents] PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([Uid] ASC)
);