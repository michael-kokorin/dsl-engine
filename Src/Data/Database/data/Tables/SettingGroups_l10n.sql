CREATE TABLE [data].[SettingGroups_l10n] (
    [Id]          BIGINT         IDENTITY (1, 1) NOT NULL,
    [SourceId]    BIGINT         NOT NULL,
    [DisplayName] NVARCHAR (400) NOT NULL,
    [CultureId]   BIGINT         NOT NULL,
    CONSTRAINT [PK_SettingGroups_l10n] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SettingGroups_l10n_Cultures_CultureId] FOREIGN KEY ([CultureId]) REFERENCES [l10n].[Cultures] ([Id]),
    CONSTRAINT [FK_SettingGroups_l10n_SettingGroups_SourceId] FOREIGN KEY ([SourceId]) REFERENCES [data].[SettingGroups] ([Id])
);

