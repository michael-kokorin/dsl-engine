CREATE TABLE [report].[Reports] (
    [Id]           BIGINT         IDENTITY (1, 1) NOT NULL,
    [ProjectId]    BIGINT         NULL,
    [DisplayName]  NVARCHAR (255) NOT NULL,
    [Created]      DATETIME2 (7)  NOT NULL,
    [CreatedById]  BIGINT         NOT NULL,
    [Modified]     DATETIME2 (7)  NOT NULL,
    [ModifiedById] BIGINT         NOT NULL,
    [Description]  NVARCHAR (MAX) NULL,
    [Rule]         NVARCHAR (MAX) NULL,
    [IsSystem]     BIT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Reports] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Reports_Projects_ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [data].[Projects] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_Reports_Users_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [security].[Users] ([Id]),
    CONSTRAINT [FK_Reports_Users_ModifiedById] FOREIGN KEY ([ModifiedById]) REFERENCES [security].[Users] ([Id])
);