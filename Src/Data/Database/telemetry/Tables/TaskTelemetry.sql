﻿CREATE TABLE [telemetry].[TaskTelemetry] (
    [Id]                BIGINT         IDENTITY (1, 1) NOT NULL,
    [DateTimeUtc]       DATETIME2 (7)  NOT NULL,
    [DateTimeLocal]     DATETIME2 (7)  NOT NULL,
    [EntityId]          BIGINT         NULL,
    [RelatedEntityId]   BIGINT         NULL,
    [OperationName]     NVARCHAR (255) NOT NULL,
    [OperationSource]   NVARCHAR (255) NULL,
    [OperationDuration] BIGINT         NULL,
    [UserSid]           NVARCHAR (255) NOT NULL,
    [UserLogin]         NVARCHAR (255) NOT NULL,
    [OperationStatus]   INT            NOT NULL,
    [OperationHResult]  INT            NULL,
    [Branch]            NVARCHAR (255) NULL,
    [TaskStatus]        INT            NULL,
    [TaskResolution]    INT            NULL,
    [TaskSdlStatus]     INT            NULL,
    [VcsPluginName]     NVARCHAR (255) NULL,
    [ItPluginName]      NVARCHAR (255) NULL,
    [FolderSize]        BIGINT         NULL,
    [ScanCoreWorkTime]  BIGINT         NULL,
    [AnalyzedSize]      BIGINT         NULL,
    CONSTRAINT [PK_TaskTelemetry] PRIMARY KEY CLUSTERED ([Id] ASC)
);

