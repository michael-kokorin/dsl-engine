namespace DbMigrations.Migrations.Migration_63
{
	using System.Data;

	using DbUpdateCommon.Properties;

	// ReSharper disable once InconsistentNaming
	internal sealed class Migration_63: DbMigration
	{
		public override void Up(IDbTransformationProvider database)
		{
			database.ExecuteNonQuery("UPDATE [t] SET [t].[AgentId] = NULL FROM [data].[Tasks] [t]");

			database.ExecuteNonQuery("DELETE FROM [data].[ScanAgents]");

			const string tableName = "ScanAgents";

			database.AddColumn(
				Settings.Default.DataSchemaName,
				tableName,
				new Column("Uid", DbType.String, 64, ColumnProperty.NotNull | ColumnProperty.Unique));

			database.AddColumn(
				Settings.Default.DataSchemaName,
				tableName,
				new Column("AssemblyVersion", DbType.String, 10, ColumnProperty.NotNull));
		}
	}
}