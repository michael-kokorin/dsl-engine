﻿CREATE TABLE [telemetry].[QueryTelemetry] (
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
    [DisplayName]       NVARCHAR (255) NULL,
    [Visibility]        INT            NULL,
    [Privacy]           INT            NULL,
    [IsSystem]          BIT            NULL,
    [Comment]           NVARCHAR (255) NULL,
    CONSTRAINT [PK_QueryTelemetry] PRIMARY KEY CLUSTERED ([Id] ASC)
);

