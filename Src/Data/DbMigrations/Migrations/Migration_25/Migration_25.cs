namespace DbMigrations.Migrations.Migration_25
{
	using System.Data;

	using JetBrains.Annotations;

	using DbUpdateCommon.Properties;

	[UsedImplicitly]

	// ReSharper disable once InconsistentNaming
	internal sealed class Migration_25: DbMigration
	{
		/// <summary>
		///   Performs migration.
		/// </summary>
		public override void Up(IDbTransformationProvider database)
		{
			database.AddTable(
				Settings.Default.DataSchemaName,
				"SettingGroups",
				new Column("Id", DbType.Int64, ColumnProperty.PrimaryKey),
				new Column("Code", DbType.String, 400, ColumnProperty.NotNull),
				new Column("DisplayName", DbType.String, 400, ColumnProperty.NotNull),
				new Column("ParentGroupId", DbType.Int64, ColumnProperty.Null));

			database.AddForeignKey(
				Settings.Default.DataSchemaName,
				"SettingGroups",
				"ParentGroupId",
				Settings.Default.DataSchemaName,
				"SettingGroups");

			database.AddTable(
				Settings.Default.DataSchemaName,
				"Settings",
				new Column("Id", DbType.Int64, ColumnProperty.PrimaryKey),
				new Column("Code", DbType.String, 400, ColumnProperty.NotNull),
				new Column("DisplayName", DbType.String, 400, ColumnProperty.NotNull),
				new Column("SettingGroupId", DbType.Int64, ColumnProperty.Null),
				new Column("SettingType", DbType.Int32, ColumnProperty.NotNull),
				new Column("SettingOwner", DbType.Int32, ColumnProperty.NotNull),
				new Column("ParentSettingId", DbType.Int64, ColumnProperty.Null),
				new Column("DefaultValue", DbType.String, DbConstants.NVarCharMax, ColumnProperty.Null),
				new Column("ParentSettingItemKey", DbType.String, 400, ColumnProperty.Null));

			database.AddForeignKey(
				Settings.Default.DataSchemaName,
				"Settings",
				"SettingGroupId",
				Settings.Default.DataSchemaName,
				"SettingGroups");
			database.AddForeignKey(
				Settings.Default.DataSchemaName,
				"Settings",
				"ParentSettingId",
				Settings.Default.DataSchemaName,
				"Settings");

			database.AddTableWithIdPrimaryKeyColumn(
				Settings.Default.DataSchemaName,
				"SettingValues",
				new Column("SettingId", DbType.Int64, ColumnProperty.NotNull),
				new Column("EntityId", DbType.Int64, ColumnProperty.NotNull),
				new Column("Value", DbType.String, DbConstants.NVarCharMax, ColumnProperty.Null));

			database.AddForeignKey(
				Settings.Default.DataSchemaName,
				"SettingValues",
				"SettingId",
				Settings.Default.DataSchemaName,
				"Settings");
		}
	}
}