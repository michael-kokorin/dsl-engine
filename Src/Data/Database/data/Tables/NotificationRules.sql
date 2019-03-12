CREATE TABLE [data].[NotificationRules] (
    [Id]          BIGINT         IDENTITY (1, 1) NOT NULL,
    [ProjectId]   BIGINT         NOT NULL,
    [DisplayName] NVARCHAR (400) NOT NULL,
    [Description] NVARCHAR (MAX) NULL,
    [Query]       NVARCHAR (MAX) NOT NULL,
    [Added]       DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_NotificationRules] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_NotificationRules_Projects_ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [data].[Projects] ([Id]) ON DELETE CASCADE
);



