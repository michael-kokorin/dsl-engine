namespace DbMigrations.Migrations.Migration_54
{
	using System.Collections.Generic;
	using System.Data;

	using JetBrains.Annotations;

	using DbUpdateCommon.Properties;

	// ReSharper disable once InconsistentNaming
	[UsedImplicitly]
	internal sealed class Migration_54: DbMigration
	{
		private static void CreateTelemetryTable(
			IDbTransformationProvider database,
			string tableName,
			params Column[] privateColumns)
		{
			var columns = new List<Column>
			{
				new Column("DateTimeUtc", DbType.DateTime2, ColumnProperty.NotNull),
				new Column("DateTimeLocal", DbType.DateTime2, ColumnProperty.NotNull),
				new Column("EntityId", DbType.Int64, ColumnProperty.Null),
				new Column("RelatedEntityId", DbType.Int64, ColumnProperty.Null),
				new Column("OperationName", DbType.String, 255, ColumnProperty.NotNull),
				new Column("OperationSource", DbType.String, 255, ColumnProperty.Null),
				new Column("OperationDuration", DbType.Int64, ColumnProperty.Null),
				new Column("UserSid", DbType.String, 255, ColumnProperty.NotNull),
				new Column("UserLogin", DbType.String, 255, ColumnProperty.NotNull),
				new Column("OperationStatus", DbType.Int32, ColumnProperty.NotNull),
				new Column("OperationHResult", DbType.Int32, ColumnProperty.Null)
			};

			columns.AddRange(privateColumns);

			database.AddTableWithIdPrimaryKeyColumn(
				Settings.Default.TelemetrySchemaName,
				tableName,
				columns.ToArray());
		}

		/// <summary>
		///     Performs migration.
		/// </summary>
		public override void Up(IDbTransformationProvider database)
		{
			database.CreateSchema(Settings.Default.TelemetrySchemaName);

			CreateTelemetryTable(
				database,
				"QueryTelemetry",
				new Column("DisplayName", DbType.String, 255, ColumnProperty.Null),
				new Column("Visibility", DbType.Int32, ColumnProperty.Null),
				new Column("Privacy", DbType.Int32, ColumnProperty.Null),
				new Column("IsSystem", DbType.Boolean, ColumnProperty.Null),
				new Column("Comment", DbType.String, 255, ColumnProperty.Null));

			CreateTelemetryTable(
				database,
				"ReportTelemetry",
				new Column("DisplayName", DbType.String, 255, ColumnProperty.Null),
				new Column("IsSystem", DbType.Boolean, ColumnProperty.Null));

			CreateTelemetryTable(
				database,
				"TaskTelemetry",
				new Column("Branch", DbType.String, 255, ColumnProperty.Null),
				new Column("TaskStatus", DbType.Int32, ColumnProperty.Null),
				new Column("TaskResolution", DbType.Int32, ColumnProperty.Null),
				new Column("TaskSdlStatus", DbType.Int32, ColumnProperty.Null),
				new Column("VcsPluginName", DbType.String, 255, ColumnProperty.Null),
				new Column("ItPluginName", DbType.String, 255, ColumnProperty.Null),
				new Column("FolderSize", DbType.Int64, ColumnProperty.Null),
				new Column("ScanCoreWorkTime", DbType.Int64, ColumnProperty.Null),
				new Column("AnalyzedSize", DbType.Int64, ColumnProperty.Null));

			CreateTelemetryTable(
				database,
				"ProjectTelemetry",
				new Column("ProjectName", DbType.String, 255, ColumnProperty.Null),
				new Column("SyncWithVcs", DbType.Boolean, ColumnProperty.Null),
				new Column("CommitToVcs", DbType.Boolean, ColumnProperty.Null),
				new Column("CommitToIt", DbType.Boolean, ColumnProperty.Null),
				new Column("EnablePooling", DbType.Boolean, ColumnProperty.Null),
				new Column("PoolingTimeout", DbType.Int32, ColumnProperty.Null));

			CreateTelemetryTable(
				database,
				"VcsPluginTelemetry",
				new Column("DisplayName", DbType.String, 255, ColumnProperty.Null),
				new Column("TypeFullName", DbType.String, 255, ColumnProperty.Null),
				new Column("AssemblyName", DbType.String, 255, ColumnProperty.Null),
				new Column("DownloadedSourcesSize", DbType.Int64, ColumnProperty.Null),
				new Column("CommittedSize", DbType.Int64, ColumnProperty.Null),
				new Column("CreatedBranchName", DbType.String, 255, ColumnProperty.Null));

			CreateTelemetryTable(
				database,
				"ItPluginTelemetry",
				new Column("DisplayName", DbType.String, 255, ColumnProperty.Null),
				new Column("TypeFullName", DbType.String, 255, ColumnProperty.Null),
				new Column("AssemblyName", DbType.String, 255, ColumnProperty.Null));
		}
	}
}