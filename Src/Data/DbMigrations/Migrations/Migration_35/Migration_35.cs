namespace DbMigrations.Migrations.Migration_35
{
	using System.Data;

	using JetBrains.Annotations;

	using DbUpdateCommon.Properties;

	[UsedImplicitly]

	// ReSharper disable once InconsistentNaming
	internal sealed class Migration_35: DbMigration
	{
		/// <summary>
		///   Performs migration.
		/// </summary>
		public override void Up(IDbTransformationProvider database) =>
			database.AddColumn(
				Settings.Default.DataSchemaName,
				"Settings",
				"Conditions",
				DbType.String,
				4000,
				ColumnProperty.Null);
	}
}