CREATE TABLE [security].[UserProjectSettings] (
    [Id]             BIGINT IDENTITY (1, 1) NOT NULL,
    [ProjectId]      BIGINT NOT NULL,
    [UserId]         BIGINT NOT NULL,
    [PreferedRoleId] BIGINT NOT NULL,
    CONSTRAINT [PK_UserProjectSettings] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_UserProjectSettings_Projects_ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [data].[Projects] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserProjectSettings_Roles_PreferedRoleId] FOREIGN KEY ([PreferedRoleId]) REFERENCES [security].[Roles] ([Id]),
    CONSTRAINT [FK_UserProjectSettings_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [security].[Users] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [UK_UserProjectSettings_ProjectId_UserId] UNIQUE NONCLUSTERED ([ProjectId] ASC, [UserId] ASC)
);

