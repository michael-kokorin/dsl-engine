CREATE TABLE [telemetry].[ProjectTelemetry] (
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
    [ProjectName]       NVARCHAR (255) NULL,
    [SyncWithVcs]       BIT            NULL,
    [CommitToVcs]       BIT            NULL,
    [CommitToIt]        BIT            NULL,
    [EnablePooling]     BIT            NULL,
    [PoolingTimeout]    INT            NULL,
    CONSTRAINT [PK_ProjectTelemetry] PRIMARY KEY CLUSTERED ([Id] ASC)
);

