CREATE FUNCTION l10n.GetTables
(	
	@CultureId BIGINT
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT
		[t].[Id],
		[t].[Name],
		[t].[Type],
		CASE
			WHEN [tl].[CultureId] IS NULL THEN [t].[DataSourceName]
			ELSE [tl].[DataSourceName]
		END as 'DataSourceName',
		CASE
			WHEN [tl].[CultureId] IS NULL THEN [t].[DataSourceDescription]
			ELSE [tl].[DataSourceDescription]
		END as 'DataSourceDescription',
		CASE
			WHEN [tl].[CultureId] IS NULL THEN [t].[CultureId]
			ELSE [tl].[CultureId]
		END as 'CultureId'
	  FROM [system].[Tables] AS [t]
	  LEFT JOIN [system].[Tables_l10n] AS [tl] ON [tl].[SourceId] = [t].Id AND [tl].[CultureId] = @CultureId
);