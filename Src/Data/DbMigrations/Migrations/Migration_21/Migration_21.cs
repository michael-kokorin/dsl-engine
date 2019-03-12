namespace DbMigrations.Migrations.Migration_21
{
	using JetBrains.Annotations;

	using DbUpdateCommon.Properties;

	[UsedImplicitly]

	// ReSharper disable once InconsistentNaming
	internal sealed class Migration_21: DbMigration
	{
		/// <summary>
		///   Performs migration.
		/// </summary>
		public override void Up(IDbTransformationProvider database)
		{
			database.AddForeignKey(
				Settings.Default.DataSchemaName,
				"Tasks",
				"Status",
				Settings.Default.DataSchemaName,
				"TaskStatuses");

			database.AddForeignKey(
				Settings.Default.DataSchemaName,
				"Tasks",
				"Resolution",
				Settings.Default.DataSchemaName,
				"TaskResolutions");

			database.AddForeignKey(
				Settings.Default.DataSchemaName,
				"Tasks",
				"SdlStatus",
				Settings.Default.DataSchemaName,
				"SdlStatuses");

			database.AddForeignKey(
				Settings.Default.DataSchemaName,
				"Projects",
				"SdlPolicyStatus",
				Settings.Default.DataSchemaName,
				"SdlStatuses");
		}
	}
}