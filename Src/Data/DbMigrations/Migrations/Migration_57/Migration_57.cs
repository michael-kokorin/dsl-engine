namespace DbMigrations.Migrations.Migration_57
{
	using System.Data;

	using JetBrains.Annotations;

	using DbUpdateCommon.Properties;

	// ReSharper disable once InconsistentNaming
	[UsedImplicitly]
	internal sealed class Migration_57 : DbMigration
	{
		public override void Up(IDbTransformationProvider database)
		{
			database.RemoveTable(Settings.Default.DataSchemaName, "UserPluginSettings");
			database.RemoveTable(Settings.Default.DataSchemaName, "ProjectPluginSettings");
			database.RemoveTable(Settings.Default.DataSchemaName, "PluginSettings");

			database.AddColumn(Settings.Default.DataSchemaName, "Settings", "IsArchived", DbType.Boolean, ColumnProperty.NotNull, 0);
			database.ExecuteNonQuery("delete from [data].[SettingValues]");
			database.AddColumn(Settings.Default.DataSchemaName, "SettingValues", "ProjectId", DbType.Int64, ColumnProperty.NotNull);
			database.AddForeignKey(Settings.Default.DataSchemaName, "SettingValues", "ProjectId", Settings.Default.DataSchemaName, "Projects");
		}
	}
}