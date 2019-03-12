namespace DbMigrations.Migrations.Migration_1
{
	using System.Data;

	using JetBrains.Annotations;

	using DbUpdateCommon.Properties;

	[UsedImplicitly]

	// ReSharper disable once InconsistentNaming
	internal sealed class Migration_1: DbMigration
	{
		public override void Up(IDbTransformationProvider database)
		{
			database.AddTableWithIdPrimaryKeyColumn(
				Settings.Default.SystemSchemaName,
				Settings.Default.PackageVersionTableName,
				new Column(Settings.Default.VersionColumnName, DbType.String, 20, ColumnProperty.NotNull),
				new Column(Settings.Default.ModuleColumnName, DbType.String, 400, ColumnProperty.NotNull),
				new Column(Settings.Default.InstalledColumnName, DbType.DateTime2, ColumnProperty.NotNull));

			database.AddUniqueConstraint(
				Settings.Default.SystemSchemaName,
				Settings.Default.PackageVersionTableName,
				Settings.Default.ModuleColumnName);

			database.AddTableWithIdPrimaryKeyColumn(
				Settings.Default.SystemSchemaName,
				Settings.Default.DbVersionTableName,
				new Column(Settings.Default.VersionColumnName, DbType.Int32, ColumnProperty.NotNull),
				new Column(Settings.Default.ModuleColumnName, DbType.String, 400, ColumnProperty.NotNull));

			database.AddUniqueConstraint(
				Settings.Default.SystemSchemaName,
				Settings.Default.DbVersionTableName,
				Settings.Default.ModuleColumnName);
		}
	}
}