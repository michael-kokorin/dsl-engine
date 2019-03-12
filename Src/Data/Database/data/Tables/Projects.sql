CREATE TABLE [data].[Projects] (
    [Id]                BIGINT         IDENTITY (1, 1) NOT NULL,
    [Alias]             NVARCHAR (255) NOT NULL,
    [DefaultBranchName] NVARCHAR (255) NOT NULL,
    [Created]           DATETIME2 (7)  NOT NULL,
    [CreatedById]       BIGINT         NOT NULL,
    [DisplayName]       NVARCHAR (255) NOT NULL,
    [Modified]          DATETIME2 (7)  NOT NULL,
    [ModifiedById]      BIGINT         NOT NULL,
    [ItPluginId]        BIGINT         NULL,
    [VcsPluginId]       BIGINT         NULL,
    [SdlPolicyStatus]   INT            NOT NULL,
    [Description]       NVARCHAR (MAX) NULL,
    [VcsLastSyncUtc]    DATETIME2 (7)  NULL,
    [ItLastSyncUtc]     DATETIME2 (7)  NULL,
    [VcsSyncEnabled]    BIT            DEFAULT ((0)) NOT NULL,
    [EnablePoll]        BIT            DEFAULT ((0)) NOT NULL,
    [PollTimeout]       INT            NULL,
    [CommitToVcs]       BIT            DEFAULT ((1)) NOT NULL,
    [CommitToIt]        BIT            DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Projects] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Projects_Plugins_ItPluginId] FOREIGN KEY ([ItPluginId]) REFERENCES [data].[Plugins] ([Id]),
    CONSTRAINT [FK_Projects_Plugins_VcsPluginId] FOREIGN KEY ([VcsPluginId]) REFERENCES [data].[Plugins] ([Id]),
    CONSTRAINT [FK_Projects_SdlStatuses_SdlPolicyStatus] FOREIGN KEY ([SdlPolicyStatus]) REFERENCES [data].[SdlStatuses] ([Id]),
    CONSTRAINT [FK_Projects_Users_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [security].[Users] ([Id]),
    CONSTRAINT [FK_Projects_Users_ModifiedById] FOREIGN KEY ([ModifiedById]) REFERENCES [security].[Users] ([Id]),
    CONSTRAINT [UK_Projects_Alias] UNIQUE NONCLUSTERED ([Alias] ASC)
);



