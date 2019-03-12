namespace DbMigrations.Migrations.Migration_13
{
	using JetBrains.Annotations;

	using DbUpdateCommon.Properties;

	[UsedImplicitly]

	// ReSharper disable once InconsistentNaming
	internal sealed class Migration_13: DbMigration
	{
		/// <summary>
		///   Performs migration.
		/// </summary>
		public override void Up(IDbTransformationProvider database)
		{
			database.RemoveConstraint(Settings.Default.SystemSchemaName, "PackageVersions", "UK_PackageVersions_Module");

			database.AddUniqueConstraint(Settings.Default.SystemSchemaName, "PackageVersions", "Module", "Version");
		}
	}
}