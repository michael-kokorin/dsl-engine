namespace DbMigrations.Migrations.Migration_6
{
	using System.Data;

	using JetBrains.Annotations;

	using DbUpdateCommon.Properties;

	[UsedImplicitly]

	// ReSharper disable once InconsistentNaming
	internal sealed class Migration_6: DbMigration
	{
		/// <summary>
		///   Performs migration.
		/// </summary>
		public override void Up(IDbTransformationProvider database)
		{
			database.AddTableWithIdPrimaryKeyColumn(
				Settings.Default.DataSchemaName,
				"Queries",
				new Column("ProjectId", DbType.Int64, ColumnProperty.Null),
				new Column("Visibility", DbType.Int32, ColumnProperty.NotNull),
				new Column("Privacy", DbType.Int32, ColumnProperty.NotNull),
				new Column("TargetCultureId", DbType.Int64, ColumnProperty.Null),
				new Column("Name", DbType.String, 128, ColumnProperty.NotNull),
				new Column("Comment", DbType.String, DbConstants.NVarCharMax, ColumnProperty.Null),
				new Column("CreatedById", DbType.Int64, ColumnProperty.NotNull),
				new Column("CreatedUtc", DbType.DateTime2, ColumnProperty.NotNull),
				new Column("ModifiedById", DbType.Int64, ColumnProperty.NotNull),
				new Column("ModifiedUtc", DbType.DateTime2, ColumnProperty.NotNull),
				new Column("Query", DbType.String, DbConstants.NVarCharMax, ColumnProperty.Null),
				new Column("XmlQuery", DbType.Xml, ColumnProperty.Null));

			database.AddUniqueConstraint(Settings.Default.DataSchemaName, "Queries", "ProjectId", "Name", "CreatedById");

			database.AddForeignKey(
				Settings.Default.DataSchemaName,
				"Queries",
				"ProjectId",
				Settings.Default.DataSchemaName,
				"Projects");
			database.AddForeignKey(
				Settings.Default.DataSchemaName,
				"Queries",
				"TargetCultureId",
				Settings.Default.L10NSchemaName,
				"Cultures");
			database.AddForeignKey(
				Settings.Default.DataSchemaName,
				"Queries",
				"CreatedById",
				Settings.Default.SecuritySchemaName,
				"Users");
			database.AddForeignKey(
				Settings.Default.DataSchemaName,
				"Queries",
				"ModifiedById",
				Settings.Default.SecuritySchemaName,
				"Users");
		}
	}
}