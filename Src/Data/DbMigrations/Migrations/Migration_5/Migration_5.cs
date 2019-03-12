namespace DbMigrations.Migrations.Migration_5
{
	using System.Data;

	using JetBrains.Annotations;

	using DbUpdateCommon.Properties;

	[UsedImplicitly]

	// ReSharper disable once InconsistentNaming
	internal sealed class Migration_5: DbMigration
	{
		/// <summary>
		///   Performs migration.
		/// </summary>
		public override void Up(IDbTransformationProvider database)
		{
			database.AddColumn(
				Settings.Default.DataSchemaName,
				"Projects",
				"VcsLastSyncUtc",
				DbType.DateTime2,
				ColumnProperty.Null);

			database.AddColumn(
				Settings.Default.DataSchemaName,
				"Projects",
				"ItLastSyncUtc",
				DbType.DateTime2,
				ColumnProperty.Null);
		}
	}
}