namespace DbMigrations.Migrations.Migration_28
{
	using System.Data;

	using JetBrains.Annotations;

	using DbUpdateCommon.Properties;

	[UsedImplicitly]

	// ReSharper disable once InconsistentNaming
	internal sealed class Migration_28: DbMigration
	{
		/// <summary>
		///   Performs migration.
		/// </summary>
		public override void Up(IDbTransformationProvider database)
		{
			database.CreateSchema(Settings.Default.TagSchemaName);

			database.AddTable(
				Settings.Default.TagSchemaName,
				"Tags",
				new Column("Id", DbType.Int64, ColumnProperty.NotNull | ColumnProperty.PrimaryKeyWithIdentity),
				new Column("Name", DbType.String, ColumnProperty.Unsigned | ColumnProperty.NotNull));

			database.AddUniqueConstraint(Settings.Default.TagSchemaName, "Tags", "Name");

			database.AddTable(
				Settings.Default.TagSchemaName,
				"TagEntities",
				new Column("Id", DbType.Int64, ColumnProperty.NotNull | ColumnProperty.PrimaryKeyWithIdentity),
				new Column("TagId", DbType.Int64, ColumnProperty.NotNull),
				new Column("TableId", DbType.Int64, ColumnProperty.NotNull),
				new Column("EntityId", DbType.Int64, ColumnProperty.NotNull)
				);

			database.AddForeignKey(
				Settings.Default.TagSchemaName,
				"TagEntities",
				"TagId",
				Settings.Default.TagSchemaName,
				"Tags");
			database.AddForeignKey(
				Settings.Default.TagSchemaName,
				"TagEntities",
				"TableId",
				Settings.Default.SystemSchemaName,
				"Tables");

			database.AddUniqueConstraint(Settings.Default.TagSchemaName, "TagEntities", "TagId", "TableId", "EntityId");
		}
	}
}