CREATE TABLE [data].[TaskStatuses] (
    [Id]   INT            NOT NULL,
    [Name] NVARCHAR (400) NOT NULL,
    CONSTRAINT [PK_TaskStatuses] PRIMARY KEY CLUSTERED ([Id] ASC)
);