CREATE TABLE [security].[Roles] (
    [Id]                 BIGINT         IDENTITY (1, 1) NOT NULL,
    [Sid]                NVARCHAR (64)  NULL,
    [Alias]              NVARCHAR (64)  NOT NULL,
    [DisplayName]        NVARCHAR (255) NOT NULL,
    [ProjectId]          BIGINT         NULL,
    [TasksQueryId]       BIGINT         NOT NULL,
    [TaskResultsQueryId] BIGINT         NOT NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Roles_Projects_ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [data].[Projects] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Roles_Queries_TaskResultsQueryId] FOREIGN KEY ([TaskResultsQueryId]) REFERENCES [data].[Queries] ([Id]),
    CONSTRAINT [FK_Roles_Queries_TasksQueryId] FOREIGN KEY ([TasksQueryId]) REFERENCES [data].[Queries] ([Id]),
    CONSTRAINT [UK_Roles_ProjectId_Alias] UNIQUE NONCLUSTERED ([ProjectId] ASC, [Alias] ASC)
);









