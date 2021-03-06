﻿CREATE TABLE [data].[Tasks] (
    [Id]                  BIGINT         IDENTITY (1, 1) NOT NULL,
    [ProjectId]           BIGINT         NOT NULL,
    [Created]             DATETIME2 (7)  NOT NULL,
    [CreatedById]         BIGINT         NOT NULL,
    [Finished]            DATETIME2 (7)  NULL,
    [Modified]            DATETIME2 (7)  NOT NULL,
    [ModifiedById]        BIGINT         NOT NULL,
    [Repository]          NVARCHAR (255) NOT NULL,
    [Status]              INT            NOT NULL,
    [SdlStatus]           INT            NOT NULL,
    [FolderPath]          NVARCHAR (400) NULL,
    [LogPath]             NVARCHAR (400) NULL,
    [ResultPath]          NVARCHAR (400) NULL,
    [FolderSize]          BIGINT         NULL,
    [ScanCoreWorkingTime] BIGINT         NULL,
    [AnalyzedFiles]       NVARCHAR (MAX) NULL,
    [AnalyzedSize]        BIGINT         NULL,
    [AnalyzedLinesCount]  BIGINT         NULL,
    [LowSeverityVulns]    INT            NULL,
    [MediumSeverityVulns] INT            NULL,
    [HighSeverityVulns]   INT            NULL,
    [FP]                  INT            NULL,
    [Todo]                INT            NULL,
    [Reopen]              INT            NULL,
    [Fixed]               INT            NULL,
    [IncrementFP]         INT            NULL,
    [IncrementTodo]       INT            NULL,
    [IncrementReopen]     INT            NULL,
    [IncrementFixed]      INT            NULL,
    [Resolution]          INT            DEFAULT ((0)) NOT NULL,
    [ResolutionMessage]   NVARCHAR (MAX) NULL,
    [AgentId]             BIGINT         NULL,
    CONSTRAINT [PK_Tasks] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Tasks_Projects_ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [data].[Projects] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Tasks_ScanAgents_AgentId] FOREIGN KEY ([AgentId]) REFERENCES [data].[ScanAgents] ([Id]),
    CONSTRAINT [FK_Tasks_SdlStatuses_SdlStatus] FOREIGN KEY ([SdlStatus]) REFERENCES [data].[SdlStatuses] ([Id]),
    CONSTRAINT [FK_Tasks_TaskResolutions_Resolution] FOREIGN KEY ([Resolution]) REFERENCES [data].[TaskResolutions] ([Id]),
    CONSTRAINT [FK_Tasks_TaskStatuses_Status] FOREIGN KEY ([Status]) REFERENCES [data].[TaskStatuses] ([Id]),
    CONSTRAINT [FK_Tasks_Users_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [security].[Users] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Tasks_Users_ModifiedById] FOREIGN KEY ([ModifiedById]) REFERENCES [security].[Users] ([Id])
);



