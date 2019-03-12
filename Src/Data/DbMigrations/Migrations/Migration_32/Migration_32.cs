namespace DbMigrations.Migrations.Migration_32
{
	using System.Data;

	using JetBrains.Annotations;

	using Common.FileSystem;
	using DbUpdateCommon.Properties;

	[UsedImplicitly]
	// ReSharper disable once InconsistentNaming
	internal sealed class Migration_32 : DbMigration
	{
		/// <summary>
		///   Performs migration.
		/// </summary>
		public override void Up(IDbTransformationProvider database)
		{
			database.AddTableWithIdPrimaryKeyColumn(
				Settings.Default.DataSchemaName,
				"Settings_l10n",
				new Column("SourceId", DbType.Int64, ColumnProperty.NotNull),
				new Column("DisplayName", DbType.String, 400, ColumnProperty.NotNull),
				new Column("DefaultValue", DbType.String, DbConstants.NVarCharMax, ColumnProperty.Null),
				new Column("CultureId", DbType.Int64, ColumnProperty.NotNull));

			database.AddForeignKey(Settings.Default.DataSchemaName, "Settings_l10n", "SourceId", Settings.Default.DataSchemaName, "Settings");
			database.AddForeignKey(Settings.Default.DataSchemaName, "Settings_l10n", "CultureId", Settings.Default.L10NSchemaName, "Cultures");

			database.AddTableWithIdPrimaryKeyColumn(
				Settings.Default.DataSchemaName,
				"SettingGroups_l10n",
				new Column("SourceId", DbType.Int64, ColumnProperty.NotNull),
				new Column("DisplayName", DbType.String, 400, ColumnProperty.NotNull),
				new Column("CultureId", DbType.Int64, ColumnProperty.NotNull));

			database.AddForeignKey(Settings.Default.DataSchemaName, "SettingGroups_l10n", "SourceId", Settings.Default.DataSchemaName, "SettingGroups");
			database.AddForeignKey(Settings.Default.DataSchemaName, "SettingGroups_l10n", "CultureId", Settings.Default.L10NSchemaName, "Cultures");

			database.ExecuteNonQuery(FileLoader.FromResource($"{GetType().Namespace}.l10n.GetSettings.sql"));
			database.ExecuteNonQuery(FileLoader.FromResource($"{GetType().Namespace}.l10n.GetSettingGroups.sql"));
		}
	}
}