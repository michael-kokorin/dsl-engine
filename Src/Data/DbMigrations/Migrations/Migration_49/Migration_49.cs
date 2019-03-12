namespace DbMigrations.Migrations.Migration_49
{
	using System.Data;

	using DbUpdateCommon.Properties;

	// ReSharper disable once InconsistentNaming
	internal sealed class Migration_49 : DbMigration
	{
		/// <summary>
		///     Performs migration.
		/// </summary>
		public override void Up(IDbTransformationProvider database)
		{
			database.ChangeColumn(
				Settings.Default.DataSchemaName,
				"Projects",
				new Column("VcsPluginId", DbType.Int64, ColumnProperty.Null, null));

			database.ChangeColumn(
				Settings.Default.DataSchemaName,
				"Projects",
				new Column("ItPluginId", DbType.Int64, ColumnProperty.Null, null));
		}
	}
}