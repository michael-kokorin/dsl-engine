namespace DbMigrations.Migrations.Migration_10
{
	using JetBrains.Annotations;

	using DbUpdateCommon.Properties;

	[UsedImplicitly]

	// ReSharper disable once InconsistentNaming
	internal sealed class Migration_10: DbMigration
	{
		public override void Up(IDbTransformationProvider database) =>
			database.RemoveConstraint(Settings.Default.ReportSchemaName, "Reports", "FK_Reports_Projects_ProjectId");
	}
}