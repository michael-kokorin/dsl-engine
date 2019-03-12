CREATE TABLE [data].[TaskResults] (
    [Id]                          BIGINT         IDENTITY (1, 1) NOT NULL,
    [TaskId]                      BIGINT         NOT NULL,
    [Message]                     NVARCHAR (MAX) NULL,
    [ExploitGraph]                NVARCHAR (MAX) NULL,
    [AdditionalExploitConditions] NVARCHAR (MAX) NULL,
    [File]                        NVARCHAR (400) NOT NULL,
    [Function]                    NVARCHAR (400) NULL,
    [LineNumber]                  INT            NOT NULL,
    [Place]                       NVARCHAR (400) NULL,
    [SourceFile]                  NVARCHAR (400) NULL,
    [RawLine]                     NVARCHAR (MAX) NOT NULL,
    [Type]                        NVARCHAR (400) NOT NULL,
    [TypeShort]                   NVARCHAR (10)  NULL,
    [Description]                 NVARCHAR (MAX) NULL,
    [SeverityType]                INT            DEFAULT ((0)) NOT NULL,
    [LinePosition]                INT            NOT NULL,
    [IssueNumber]                 NVARCHAR (MAX) NULL,
    [IssueUrl]                    NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_TaskResults] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_TaskResults_Tasks_TaskId] FOREIGN KEY ([TaskId]) REFERENCES [data].[Tasks] ([Id]) ON DELETE CASCADE
);



