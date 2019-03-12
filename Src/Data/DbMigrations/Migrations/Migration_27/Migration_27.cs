namespace DbMigrations.Migrations.Migration_27
{
	using System.Data;

	using JetBrains.Annotations;

	using DbUpdateCommon.Properties;

	[UsedImplicitly]

	// ReSharper disable once InconsistentNaming
	internal sealed class Migration_27: DbMigration
	{
		/// <summary>
		///   Performs migration.
		/// </summary>
		public override void Up(IDbTransformationProvider database)
		{
			database.ExecuteNonQuery("DELETE FROM report.Reports");

			database.RemoveColumn(Settings.Default.ReportSchemaName, "Reports", "Request");

			database.AddColumn(
				Settings.Default.ReportSchemaName,
				"Reports",
				"Rule",
				DbType.String,
				DbConstants.NVarCharMax,
				ColumnProperty.Null,
				null);

			database.AddColumn(
				Settings.Default.ReportSchemaName,
				"Reports",
				"IsSystem",
				DbType.Boolean,
				ColumnProperty.NotNull,
				0);
		}
	}
}