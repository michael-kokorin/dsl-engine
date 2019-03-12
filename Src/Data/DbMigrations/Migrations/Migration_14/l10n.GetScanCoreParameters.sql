CREATE FUNCTION l10n.GetScanCoreParameters
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
		END as 'CultureId',
		CASE
			WHEN [tl].[DefaultValue] IS NULL THEN [t].[DefaultValue]
			ELSE [tl].[DefaultValue]
		END as 'DefaultValue',
		[t].[ParameterType]
	  FROM [data].[ScanCoreParameters] AS [t]
	  LEFT JOIN [data].[ScanCoreParameters_l10n] AS [tl] ON [tl].[SourceId] = [t].Id AND [tl].[CultureId] = @CultureId
);