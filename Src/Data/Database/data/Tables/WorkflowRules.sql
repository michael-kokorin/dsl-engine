CREATE TABLE [data].[WorkflowRules] (
    [Id]               BIGINT         IDENTITY (1, 1) NOT NULL,
    [ProjectId]        BIGINT         NOT NULL,
    [DisplayName]      NVARCHAR (400) NOT NULL,
    [Description]      NVARCHAR (MAX) NULL,
    [Query]            NVARCHAR (MAX) NOT NULL,
    [IsEventTriggered] BIT            NOT NULL,
    [IsTimeTriggered]  BIT            NOT NULL,
    [Start]            DATETIME2 (7)  NULL,
    [IsRepeatable]     BIT            NULL,
    [Repeat]           NVARCHAR (50)  NULL,
    [Added]            DATETIME2 (7)  NOT NULL,
    [ActionKey]        NVARCHAR (400) NOT NULL,
    [IsForAllEvents]   BIT            NOT NULL,
    CONSTRAINT [PK_WorkflowRules] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_WorkflowRules_Projects_ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [data].[Projects] ([Id]) ON DELETE CASCADE
);

