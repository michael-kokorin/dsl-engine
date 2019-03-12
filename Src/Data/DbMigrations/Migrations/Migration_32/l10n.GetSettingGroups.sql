CREATE FUNCTION l10n.GetSettingGroups
(	
	@CultureId BIGINT
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT
		[t].[Id],
		[t].[Code],
		[t].[ParentGroupId],
		CASE
			WHEN [tl].[CultureId] IS NULL THEN [t].[DisplayName]
			ELSE [tl].[DisplayName]
		END as 'DisplayName'
	  FROM [data].[SettingGroups] AS [t]
	  LEFT JOIN [data].[SettingGroups_l10n] AS [tl] ON [tl].[SourceId] = [t].Id AND [tl].[CultureId] = @CultureId
);