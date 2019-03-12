namespace DbMigrations.Migrations.Migration_15
{
	using System.Data;

	using JetBrains.Annotations;

	using DbUpdateCommon.Properties;

	[UsedImplicitly]

	// ReSharper disable once InconsistentNaming
	internal sealed class Migration_15: DbMigration
	{
		/// <summary>
		///   Performs migration.
		/// </summary>
		public override void Up(IDbTransformationProvider database) =>
			database.AddColumn(Settings.Default.DataSchemaName, "Queries", "IsSystem", DbType.Boolean, ColumnProperty.NotNull, 0);
	}
}