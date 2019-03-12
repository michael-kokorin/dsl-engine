CREATE TABLE [security].[RoleAuthorities] (
    [Id]          BIGINT IDENTITY (1, 1) NOT NULL,
    [RoleId]      BIGINT NOT NULL,
    [AuthorityId] BIGINT NOT NULL,
    CONSTRAINT [PK_RoleAuthorities] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_RoleAuthorities_Authorities_AuthorityId] FOREIGN KEY ([AuthorityId]) REFERENCES [security].[Authorities] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_RoleAuthorities_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [security].[Roles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [UK_RoleAuthorities_RoleId_AuthorityId] UNIQUE NONCLUSTERED ([RoleId] ASC, [AuthorityId] ASC)
);

