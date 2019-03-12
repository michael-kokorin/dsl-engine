DELETE [rt]
FROM [security].[RoleAuthorities] [rt]
JOIN [security].[Roles] [r] ON [rt].[RoleId] = [r].[Id]
JOIN [security].[Authorities] [a] ON [a].[Id] = [rt].[AuthorityId]
WHERE
	[a].[Key] = N'create_project' AND
	[r].[ProjectId] IS NOT NULL