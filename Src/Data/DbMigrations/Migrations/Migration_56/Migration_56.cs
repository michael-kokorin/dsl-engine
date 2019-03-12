namespace DbMigrations.Migrations.Migration_56
{
	using System.Data;

	using JetBrains.Annotations;

	using DbUpdateCommon.Properties;

	// ReSharper disable once InconsistentNaming
	[UsedImplicitly]
	internal sealed class Migration_56: DbMigration
	{
		public override void Up(IDbTransformationProvider database)
		{
			database.AddColumn(
				Settings.Default.DataSchemaName,
				"Tasks",
				new Column("AgentId", DbType.Int64, ColumnProperty.Null));

			database.AddForeignKey(
				Settings.Default.DataSchemaName,
				"Tasks",
				"AgentId",
				Settings.Default.DataSchemaName,
				"ScanAgents");
		}
	}
}