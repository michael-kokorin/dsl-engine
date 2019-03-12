namespace DbMigrations.Migrations.Migration_31
{
	using JetBrains.Annotations;

	using DbUpdateCommon.Properties;

	[UsedImplicitly]
	// ReSharper disable once InconsistentNaming
	internal sealed class Migration_31 : DbMigration
	{
		/// <summary>
		///   Performs migration.
		/// </summary>
		public override void Up(IDbTransformationProvider database)
		{
			database.RemoveConstraint(Settings.Default.DataSchemaName, "Projects", "FK_Projects_ScanCores_ScanCoreId");
			database.RemoveConstraint(Settings.Default.DataSchemaName, "Tasks", "FK_Tasks_ScanCores_ScanCoreId");

			database.RemoveColumn(Settings.Default.DataSchemaName, "Projects", "ScanCoreId");
			database.RemoveColumn(Settings.Default.DataSchemaName, "Tasks", "ScanCoreId");

			database.RemoveTable(Settings.Default.DataSchemaName, "ProjectToCoreToScanCoreParameters");
			database.RemoveTable(Settings.Default.DataSchemaName, "TaskToCoreToScanCoreParameters");
			database.RemoveTable(Settings.Default.DataSchemaName, "CoreToScanCoreParameters");
			database.RemoveTable(Settings.Default.DataSchemaName, "ScanCoreParameters_l10n");
			database.RemoveTable(Settings.Default.DataSchemaName, "ScanCoreParameters");
			database.RemoveTable(Settings.Default.DataSchemaName, "ScanCoreParameterGroups_l10n");
			database.RemoveTable(Settings.Default.DataSchemaName, "ScanCoreParameterGroups");
			database.RemoveTable(Settings.Default.DataSchemaName, "ScanCoreParameterTypes");

			database.RemoveTable(Settings.Default.DataSchemaName, "ScanCores");

			database.ExecuteNonQuery("DROP FUNCTION [l10n].[GetScanCoreParameterGroups]");
			database.ExecuteNonQuery("DROP FUNCTION [l10n].[GetScanCoreParameters]");
		}
	}
}