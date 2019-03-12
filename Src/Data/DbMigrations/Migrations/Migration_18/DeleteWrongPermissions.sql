DELETE [ra]
FROM [security].[RoleAuthorities] [ra]
INNER JOIN [security].[Roles] [r] ON [r].[Id] = [ra].[RoleId]
INNER JOIN [security].[Authorities] [a] ON [a].[Id] = [ra].[AuthorityId]
WHERE
	[r].[Alias] <> N'sdl_admin' AND (
	[a].[Key] = N'edit_query_all' OR
	[a].[Key] = N'view_queries_all')