CREATE TABLE [data].[UserInterfaces] (
    [Id]             BIGINT         IDENTITY (1, 1) NOT NULL,
    [Host]           NVARCHAR (255) NOT NULL,
    [RemoteIp]       NVARCHAR (15)  NOT NULL,
    [RemotePort]     INT            NOT NULL,
    [Version]        NVARCHAR (12)  NOT NULL,
    [RegisteredUtc]  DATETIME2 (7)  NOT NULL,
    [LastCheckedUtc] DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_UserInterfaces] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UK_UserInterfaces_Host] UNIQUE NONCLUSTERED ([Host] ASC)
);

