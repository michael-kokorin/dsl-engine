namespace DbMigrations.Migrations.Migration_7
{
	using System.Data;

	using JetBrains.Annotations;

	using DbUpdateCommon.Properties;

	[UsedImplicitly]

	// ReSharper disable once InconsistentNaming
	internal sealed class Migration_7: DbMigration
	{
		/// <summary>
		///   Performs migration.
		/// </summary>
		public override void Up(IDbTransformationProvider database)
		{
			database.RemoveConstraint(Settings.Default.SystemSchemaName, "Tables", "UK_Tables_Name");
			database.RemoveConstraint(Settings.Default.SystemSchemaName, "Tables", "UK_Tables_DataSourceName");

			database.ChangeColumn(
				Settings.Default.SystemSchemaName,
				"Tables",
				new Column("Name", DbType.String, 64, ColumnProperty.NotNull));
			database.ChangeColumn(
				Settings.Default.SystemSchemaName,
				"Tables",
				new Column("DataSourceName", DbType.String, 64, ColumnProperty.NotNull));

			database.AddUniqueConstraint(Settings.Default.SystemSchemaName, "Tables", "Name");
			database.AddUniqueConstraint(Settings.Default.SystemSchemaName, "Tables", "DataSourceName");
		}
	}
}