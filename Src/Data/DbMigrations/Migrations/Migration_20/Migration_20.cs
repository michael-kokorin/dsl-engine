namespace DbMigrations.Migrations.Migration_20
{
	using System.Data;

	using JetBrains.Annotations;

	using DbUpdateCommon.Properties;

	[UsedImplicitly]

	// ReSharper disable once InconsistentNaming
	internal sealed class Migration_20: DbMigration
	{
		/// <summary>
		///   Performs migration.
		/// </summary>
		public override void Up(IDbTransformationProvider database)
		{
			database.AddColumn(Settings.Default.DataSchemaName, "Tasks", "Resolution", DbType.Int32, ColumnProperty.NotNull, 0);
			database.AddColumn(
				Settings.Default.DataSchemaName,
				"Tasks",
				"ResolutionMessage",
				DbType.String,
				DbConstants.NVarCharMax,
				ColumnProperty.Null);

			database.AddTable(
				Settings.Default.DataSchemaName,
				"TaskStatuses",
				new Column("Id", DbType.Int32, ColumnProperty.PrimaryKey),
				new Column("Name", DbType.String, 400, ColumnProperty.NotNull));

			database.AddTable(
				Settings.Default.DataSchemaName,
				"TaskResolutions",
				new Column("Id", DbType.Int32, ColumnProperty.PrimaryKey),
				new Column("Name", DbType.String, 400, ColumnProperty.NotNull));

			database.AddTable(
				Settings.Default.DataSchemaName,
				"SdlStatuses",
				new Column("Id", DbType.Int32, ColumnProperty.PrimaryKey),
				new Column("Name", DbType.String, 400, ColumnProperty.NotNull));
		}
	}
}