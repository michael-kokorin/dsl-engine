CREATE TABLE [data].[SettingValues] (
    [Id]        BIGINT         IDENTITY (1, 1) NOT NULL,
    [SettingId] BIGINT         NOT NULL,
    [EntityId]  BIGINT         NOT NULL,
    [Value]     NVARCHAR (MAX) NULL,
    [ProjectId] BIGINT         NOT NULL,
    CONSTRAINT [PK_SettingValues] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SettingValues_Projects_ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [data].[Projects] ([Id]),
    CONSTRAINT [FK_SettingValues_Settings_SettingId] FOREIGN KEY ([SettingId]) REFERENCES [data].[Settings] ([Id])
);



