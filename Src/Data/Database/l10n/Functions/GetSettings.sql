CREATE FUNCTION l10n.GetSettings
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
		[t].[SettingGroupId],
		[t].[SettingType],
		[t].[SettingOwner],
		[t].[ParentSettingId],
		[t].[ParentSettingItemKey],
		[t].[Conditions],
		CASE
			WHEN [tl].[CultureId] IS NULL THEN [t].[DisplayName]
			ELSE [tl].[DisplayName]
		END as 'DisplayName',
		CASE
			WHEN [tl].[CultureId] IS NULL THEN [t].[DefaultValue]
			ELSE [tl].[DefaultValue]
		END as 'DefaultValue'
	  FROM [data].[Settings] AS [t]
	  LEFT JOIN [data].[Settings_l10n] AS [tl] ON [tl].[SourceId] = [t].Id AND [tl].[CultureId] = @CultureId
);