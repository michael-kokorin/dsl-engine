namespace DbMigrations.Migrations.Migration_9
{
	using System.Data;

	using JetBrains.Annotations;

	using DbUpdateCommon.Properties;

	[UsedImplicitly]

	// ReSharper disable once InconsistentNaming
	internal sealed class Migration_9: DbMigration
	{
		/// <summary>
		///   Performs migration.
		/// </summary>
		public override void Up(IDbTransformationProvider database) =>
			database.AddColumn(
				Settings.Default.DataSchemaName,
				"Projects",
				"VcsSyncEnabled",
				DbType.Boolean,
				ColumnProperty.NotNull,
				0);
	}
}