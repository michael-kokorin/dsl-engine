namespace DbMigrations.Migrations.Migration_12
{
	using System.Data;

	using JetBrains.Annotations;

	using DbUpdateCommon.Properties;

	[UsedImplicitly]

	// ReSharper disable once InconsistentNaming
	internal sealed class Migration_12: DbMigration
	{
		/// <summary>
		///   Performs migration.
		/// </summary>
		public override void Up(IDbTransformationProvider database)
		{
			database.RemoveColumn(Settings.Default.DataSchemaName, "Queries", "XmlQuery");

			database.AddColumn(
				Settings.Default.DataSchemaName,
				"Queries",
				"JsonQuery",
				DbType.String,
				DbConstants.NVarCharMax,
				ColumnProperty.Null);
		}
	}
}