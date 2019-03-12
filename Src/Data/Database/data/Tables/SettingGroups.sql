CREATE TABLE [data].[SettingGroups] (
    [Id]            BIGINT         NOT NULL IDENTITY,
    [Code]          NVARCHAR (400) NOT NULL,
    [DisplayName]   NVARCHAR (400) NOT NULL,
    [ParentGroupId] BIGINT         NULL,
    [OwnerPluginId] BIGINT         NULL,
    CONSTRAINT [PK_SettingGroups] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SettingGroups_Plugins] FOREIGN KEY ([OwnerPluginId]) REFERENCES [data].[Plugins] ([Id]),
    CONSTRAINT [FK_SettingGroups_SettingGroups_ParentGroupId] FOREIGN KEY ([ParentGroupId]) REFERENCES [data].[SettingGroups] ([Id])
);



