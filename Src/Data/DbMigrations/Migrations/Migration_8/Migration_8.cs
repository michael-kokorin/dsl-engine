namespace DbMigrations.Migrations.Migration_8
{
	using System.Data;

	using JetBrains.Annotations;

	using DbUpdateCommon.Properties;

	// ReSharper disable once InconsistentNaming
	[UsedImplicitly]
	internal sealed class Migration_8: DbMigration
	{
		/// <summary>
		///   Performs migration.
		/// </summary>
		public override void Up(IDbTransformationProvider database)
		{
			database.ChangeColumn(
				Settings.Default.SystemSchemaName,
				"TableColumns",
				new Column("Name", DbType.String, 64, ColumnProperty.NotNull));
			database.ChangeColumn(
				Settings.Default.SystemSchemaName,
				"TableColumns",
				new Column("FieldName", DbType.String, 64, ColumnProperty.NotNull));

			database.ChangeColumn(
				Settings.Default.SystemSchemaName,
				"Tables_l10n",
				new Column("DataSourceName", DbType.String, 64, ColumnProperty.NotNull));

			database.ChangeColumn(
				Settings.Default.SystemSchemaName,
				"TableColumns_l10n",
				new Column("FieldName", DbType.String, 64, ColumnProperty.NotNull));
		}
	}
}