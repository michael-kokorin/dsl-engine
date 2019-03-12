namespace DbMigrations.Migrations.Migration_14
{
	using System.Data;

	using JetBrains.Annotations;

	using Common.FileSystem;
	using DbUpdateCommon.Properties;

	[UsedImplicitly]

	// ReSharper disable once InconsistentNaming
	internal sealed class Migration_14: DbMigration
	{
		/// <summary>
		///   Performs migration.
		/// </summary>
		public override void Up(IDbTransformationProvider database)
		{
			database.AddTable(
				Settings.Default.DataSchemaName,
				"ScanCoreParameterTypes",
				new Column("Id", DbType.Int16, ColumnProperty.PrimaryKey),
				new Column("Name", DbType.String, 400, ColumnProperty.NotNull));

			database.AddTable(
				Settings.Default.DataSchemaName,
				"ScanCoreParameterGroups",
				new Column("Id", DbType.Int64, ColumnProperty.PrimaryKey),
				new Column("Name", DbType.String, 400, ColumnProperty.NotNull),
				new Column("DisplayName", DbType.String, 400, ColumnProperty.NotNull),
				new Column("CultureId", DbType.Int64, ColumnProperty.NotNull));

			database.AddForeignKey(
				Settings.Default.DataSchemaName,
				"ScanCoreParameterGroups",
				"CultureId",
				Settings.Default.L10NSchemaName,
				"Cultures");

			database.AddTableWithIdPrimaryKeyColumn(
				Settings.Default.DataSchemaName,
				"ScanCoreParameterGroups_l10n",
				new Column("SourceId", DbType.Int64, ColumnProperty.NotNull),
				new Column("DisplayName", DbType.String, 400, ColumnProperty.NotNull),
				new Column("CultureId", DbType.Int64, ColumnProperty.NotNull));

			database.AddForeignKey(
				Settings.Default.DataSchemaName,
				"ScanCoreParameterGroups_l10n",
				"SourceId",
				Settings.Default.DataSchemaName,
				"ScanCoreParameterGroups");
			database.AddForeignKey(
				Settings.Default.DataSchemaName,
				"ScanCoreParameterGroups_l10n",
				"CultureId",
				Settings.Default.L10NSchemaName,
				"Cultures");

			database.AddTable(
				Settings.Default.DataSchemaName,
				"ScanCoreParameters",
				new Column("Id", DbType.Int64, ColumnProperty.PrimaryKey),
				new Column("Name", DbType.String, 400, ColumnProperty.NotNull),
				new Column("DisplayName", DbType.String, 400, ColumnProperty.NotNull),
				new Column("DefaultValue", DbType.String, DbConstants.NVarCharMax, ColumnProperty.Null),
				new Column("CultureId", DbType.Int64, ColumnProperty.NotNull),
				new Column("ParameterType", DbType.Int16, ColumnProperty.NotNull));

			database.AddForeignKey(
				Settings.Default.DataSchemaName,
				"ScanCoreParameters",
				"CultureId",
				Settings.Default.L10NSchemaName,
				"Cultures");
			database.AddForeignKey(
				Settings.Default.DataSchemaName,
				"ScanCoreParameters",
				"ParameterType",
				Settings.Default.DataSchemaName,
				"ScanCoreParameterTypes");

			database.AddTableWithIdPrimaryKeyColumn(
				Settings.Default.DataSchemaName,
				"ScanCoreParameters_l10n",
				new Column("SourceId", DbType.Int64, ColumnProperty.NotNull),
				new Column("DisplayName", DbType.String, 400, ColumnProperty.NotNull),
				new Column("DefaultValue", DbType.String, DbConstants.NVarCharMax, ColumnProperty.Null),
				new Column("CultureId", DbType.Int64, ColumnProperty.NotNull));

			database.AddForeignKey(
				Settings.Default.DataSchemaName,
				"ScanCoreParameters_l10n",
				"SourceId",
				Settings.Default.DataSchemaName,
				"ScanCoreParameters");
			database.AddForeignKey(
				Settings.Default.DataSchemaName,
				"ScanCoreParameters_l10n",
				"CultureId",
				Settings.Default.L10NSchemaName,
				"Cultures");

			database.AddTableWithIdPrimaryKeyColumn(
				Settings.Default.DataSchemaName,
				"CoreToScanCoreParameters",
				new Column("CoreId", DbType.Int64, ColumnProperty.NotNull),
				new Column("ParameterId", DbType.Int64, ColumnProperty.NotNull),
				new Column("GroupId", DbType.Int64, ColumnProperty.Null));

			database.AddForeignKey(
				Settings.Default.DataSchemaName,
				"CoreToScanCoreParameters",
				"CoreId",
				Settings.Default.DataSchemaName,
				"ScanCores");
			database.AddForeignKey(
				Settings.Default.DataSchemaName,
				"CoreToScanCoreParameters",
				"ParameterId",
				Settings.Default.DataSchemaName,
				"ScanCoreParameters");
			database.AddForeignKey(
				Settings.Default.DataSchemaName,
				"CoreToScanCoreParameters",
				"GroupId",
				Settings.Default.DataSchemaName,
				"ScanCoreParameterGroups");

			database.AddTableWithIdPrimaryKeyColumn(
				Settings.Default.DataSchemaName,
				"ProjectToCoreToScanCoreParameters",
				new Column("ProjectId", DbType.Int64, ColumnProperty.NotNull),
				new Column("ParameterId", DbType.Int64, ColumnProperty.NotNull),
				new Column("Value", DbType.String, DbConstants.NVarCharMax, ColumnProperty.Null));

			database.AddForeignKey(
				Settings.Default.DataSchemaName,
				"ProjectToCoreToScanCoreParameters",
				"ProjectId",
				Settings.Default.DataSchemaName,
				"Projects");
			database.AddForeignKey(
				Settings.Default.DataSchemaName,
				"ProjectToCoreToScanCoreParameters",
				"ParameterId",
				Settings.Default.DataSchemaName,
				"CoreToScanCoreParameters");

			database.AddTableWithIdPrimaryKeyColumn(
				Settings.Default.DataSchemaName,
				"TaskToCoreToScanCoreParameters",
				new Column("TaskId", DbType.Int64, ColumnProperty.NotNull),
				new Column("ParameterId", DbType.Int64, ColumnProperty.NotNull),
				new Column("Value", DbType.String, DbConstants.NVarCharMax, ColumnProperty.NotNull));

			database.AddForeignKey(
				Settings.Default.DataSchemaName,
				"TaskToCoreToScanCoreParameters",
				"TaskId",
				Settings.Default.DataSchemaName,
				"Tasks");
			database.AddForeignKey(
				Settings.Default.DataSchemaName,
				"TaskToCoreToScanCoreParameters",
				"ParameterId",
				Settings.Default.DataSchemaName,
				"CoreToScanCoreParameters");

			database.ExecuteNonQuery(FileLoader.FromResource($"{GetType().Namespace}.l10n.GetScanCoreParameters.sql"));
			database.ExecuteNonQuery(FileLoader.FromResource($"{GetType().Namespace}.l10n.GetScanCoreParameterGroups.sql"));
		}
	}
}