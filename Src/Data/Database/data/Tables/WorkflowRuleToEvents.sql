CREATE TABLE [data].[WorkflowRuleToEvents] (
    [Id]             BIGINT IDENTITY (1, 1) NOT NULL,
    [WorkflowRuleId] BIGINT NOT NULL,
    [EventId]        BIGINT NOT NULL,
    CONSTRAINT [PK_WorkflowRuleToEvents] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_WorkflowRuleToEvents_Events_EventId] FOREIGN KEY ([EventId]) REFERENCES [data].[Events] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_WorkflowRuleToEvents_WorkflowRules_WorkflowRuleId] FOREIGN KEY ([WorkflowRuleId]) REFERENCES [data].[WorkflowRules] ([Id]) ON DELETE CASCADE
);

