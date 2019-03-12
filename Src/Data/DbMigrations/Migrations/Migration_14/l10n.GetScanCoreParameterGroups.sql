CREATE FUNCTION l10n.GetScanCoreParameterGroups
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
		CASE
			WHEN [tl].[CultureId] IS NULL THEN [t].[DisplayName]
			ELSE [tl].[DisplayName]
		END as 'DisplayName',
		CASE
			WHEN [tl].[CultureId] IS NULL THEN [t].[CultureId]
			ELSE [tl].[CultureId]
		END as 'CultureId'
	  FROM [data].[ScanCoreParameterGroups] AS [t]
	  LEFT JOIN [data].[ScanCoreParameterGroups_l10n] AS [tl] ON [tl].[SourceId] = [t].Id AND [tl].[CultureId] = @CultureId
);