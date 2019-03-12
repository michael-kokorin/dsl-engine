CREATE FUNCTION l10n.GetTableColumns
(	
	@CultureId BIGINT
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT
		[tc].[Id],
		[tc].[TableId],
		[tc].[ReferenceTableId],
		[tc].[Name],
		[tc].[Type],
		CASE
			WHEN [tcl].[CultureId] IS NULL THEN [tc].[FieldName]
			ELSE [tcl].[FieldName]
		END AS 'FieldName',
		CASE
			WHEN [tcl].[CultureId] IS NULL THEN [tc].[FieldDescription]
			ELSE [tcl].[FieldDescription]
		END AS 'FieldDescription',
		[tc].[FieldType],
		[tc].[FieldDataType],
		CASE
			WHEN [tcl].[CultureId] IS NULL THEN [tc].[CultureId]
			ELSE [tcl].[CultureId]
		END AS 'CultureId'
	FROM [system].[TableColumns] [tc]
	LEFT JOIN [system].[TableColumns_l10n] [tcl] ON [tcl].[SourceId] = [tc].[Id] AND [tcl].[CultureId] = @CultureId
);