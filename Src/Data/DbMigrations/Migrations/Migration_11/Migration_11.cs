namespace DbMigrations.Migrations.Migration_11
{
	using JetBrains.Annotations;

	using DbUpdateCommon.Properties;

	[UsedImplicitly]

	// ReSharper disable once InconsistentNaming
	internal sealed class Migration_11: DbMigration
	{
		public override void Up(IDbTransformationProvider database) =>
			database.AddForeignKey(
				Settings.Default.ReportSchemaName,
				"Reports",
				new[] {"ProjectId"},
				Settings.Default.DataSchemaName,
				"Projects",
				new[] {"Id"},
				onUpdateConstraint: ForeignKeyConstraint.Cascade,
				onDeleteConstraint: ForeignKeyConstraint.Cascade);
	}
}