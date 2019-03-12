CREATE TABLE [data].[Settings] (
    [Id]                   BIGINT          IDENTITY (1, 1) NOT NULL,
    [Code]                 NVARCHAR (400)  NOT NULL,
    [DisplayName]          NVARCHAR (400)  NOT NULL,
    [SettingGroupId]       BIGINT          NULL,
    [SettingType]          INT             NOT NULL,
    [SettingOwner]         INT             NOT NULL,
    [ParentSettingId]      BIGINT          NULL,
    [DefaultValue]         NVARCHAR (MAX)  NULL,
    [ParentSettingItemKey] NVARCHAR (400)  NULL,
    [Conditions]           NVARCHAR (4000) NULL,
    [OwnerPluginId]        BIGINT          NULL,
    [IsAuth]               BIT             CONSTRAINT [DF_Settings_IsAuth] DEFAULT ((0)) NOT NULL,
    [IsArchived]           BIT             DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Settings] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Settings_Plugins] FOREIGN KEY ([OwnerPluginId]) REFERENCES [data].[Plugins] ([Id]),
    CONSTRAINT [FK_Settings_SettingGroups_SettingGroupId] FOREIGN KEY ([SettingGroupId]) REFERENCES [data].[SettingGroups] ([Id]),
    CONSTRAINT [FK_Settings_Settings_ParentSettingId] FOREIGN KEY ([ParentSettingId]) REFERENCES [data].[Settings] ([Id])
);







