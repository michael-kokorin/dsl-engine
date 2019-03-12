CREATE TABLE [data].[PolicyRules] (
    [Id]        BIGINT         IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (64)  NOT NULL,
    [Query]     NVARCHAR (MAX) NOT NULL,
    [ProjectId] BIGINT         NOT NULL,
    [Added]     DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_PolicyRules] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_PolicyRules_Projects_ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [data].[Projects] ([Id]) ON DELETE CASCADE
);

