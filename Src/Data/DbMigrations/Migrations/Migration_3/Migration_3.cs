namespace DbMigrations.Migrations.Migration_3
{
	using System.Data;

	using JetBrains.Annotations;

	using Common.FileSystem;
	using DbUpdateCommon.Properties;

	[UsedImplicitly]

	// ReSharper disable once InconsistentNaming
	internal sealed class Migration_3: DbMigration
	{
		/// <summary>
		///   Performs migration.
		/// </summary>
		public override void Up(IDbTransformationProvider database)
		{
			database.AddUniqueConstraint(Settings.Default.L10NSchemaName, "Cultures", "Code");

			database.AddTableWithIdPrimaryKeyColumn(
				Settings.Default.SystemSchemaName,
				"Tables",
				new Column("Name", DbType.String, 20, ColumnProperty.NotNull),
				new Column("Type", DbType.Int32, ColumnProperty.NotNull),
				new Column("DataSourceName", DbType.String, 20, ColumnProperty.NotNull),
				new Column("DataSourceDescription", DbType.String, DbConstants.NVarCharMax),
				new Column("CultureId", DbType.Int64, ColumnProperty.NotNull));

			database.AddUniqueConstraint(Settings.Default.SystemSchemaName, "Tables", "Name");
			database.AddUniqueConstraint(Settings.Default.SystemSchemaName, "Tables", "DataSourceName");
			database.AddForeignKey(
				Settings.Default.SystemSchemaName,
				"Tables",
				"CultureId",
				Settings.Default.L10NSchemaName,
				"Cultures");

			database.AddTableWithIdPrimaryKeyColumn(
				Settings.Default.SystemSchemaName,
				"Tables_l10n",
				new Column("SourceId", DbType.Int64, ColumnProperty.NotNull),
				new Column("DataSourceName", DbType.String, 20, ColumnProperty.NotNull),
				new Column("DataSourceDescription", DbType.String, DbConstants.NVarCharMax),
				new Column("CultureId", DbType.Int64, ColumnProperty.NotNull));

			database.AddUniqueConstraint(Settings.Default.SystemSchemaName, "Tables_l10n", "SourceId", "CultureId");
			database.AddForeignKey(
				Settings.Default.SystemSchemaName,
				"Tables_l10n",
				"SourceId",
				Settings.Default.SystemSchemaName,
				"Tables");
			database.AddForeignKey(
				Settings.Default.SystemSchemaName,
				"Tables_l10n",
				"CultureId",
				Settings.Default.L10NSchemaName,
				"Cultures");

			database.AddTableWithIdPrimaryKeyColumn(
				Settings.Default.SystemSchemaName,
				"TableColumns",
				new Column("TableId", DbType.Int64, ColumnProperty.NotNull),
				new Column("ReferenceTableId", DbType.Int64, ColumnProperty.Null),
				new Column("Name", DbType.String, 20, ColumnProperty.NotNull),
				new Column("Type", DbType.Int32, ColumnProperty.NotNull),
				new Column("FieldName", DbType.String, 20, ColumnProperty.NotNull),
				new Column("FieldDescription", DbType.String, DbConstants.NVarCharMax, ColumnProperty.Null),
				new Column("FieldType", DbType.Int32, ColumnProperty.NotNull),
				new Column("FieldDataType", DbType.Int32, ColumnProperty.NotNull),
				new Column("CultureId", DbType.Int64, ColumnProperty.NotNull));

			database.AddUniqueConstraint(Settings.Default.SystemSchemaName, "TableColumns", "TableId", "Name");
			database.AddUniqueConstraint(Settings.Default.SystemSchemaName, "TableColumns", "TableId", "FieldName");

			database.AddForeignKey(
				Settings.Default.SystemSchemaName,
				"TableColumns",
				"TableId",
				Settings.Default.SystemSchemaName,
				"Tables");
			database.AddForeignKey(
				Settings.Default.SystemSchemaName,
				"TableColumns",
				"ReferenceTableId",
				Settings.Default.SystemSchemaName,
				"Tables");
			database.AddForeignKey(
				Settings.Default.SystemSchemaName,
				"TableColumns",
				"CultureId",
				Settings.Default.L10NSchemaName,
				"Cultures");

			database.AddTableWithIdPrimaryKeyColumn(
				Settings.Default.SystemSchemaName,
				"TableColumns_l10n",
				new Column("SourceId", DbType.Int64, ColumnProperty.NotNull),
				new Column("FieldName", DbType.String, 20, ColumnProperty.NotNull),
				new Column("FieldDescription", DbType.String, DbConstants.NVarCharMax),
				new Column("CultureId", DbType.Int64, ColumnProperty.NotNull));

			database.AddUniqueConstraint(Settings.Default.SystemSchemaName, "TableColumns_l10n", "SourceId", "CultureId");
			database.AddForeignKey(
				Settings.Default.SystemSchemaName,
				"TableColumns_l10n",
				"SourceId",
				Settings.Default.SystemSchemaName,
				"TableColumns");
			database.AddForeignKey(
				Settings.Default.SystemSchemaName,
				"TableColumns_l10n",
				"CultureId",
				Settings.Default.L10NSchemaName,
				"Cultures");

			database.ExecuteNonQuery(FileLoader.FromResource($"{GetType().Namespace}.l10n.GetTables.sql"));
			database.ExecuteNonQuery(FileLoader.FromResource($"{GetType().Namespace}.l10n.GetTableColumns.sql"));
		}
	}
}