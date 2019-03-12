namespace DbMigrations.Migrations.Migration_4
{
	using System.Data;

	using JetBrains.Annotations;

	using DbUpdateCommon.Properties;

	[UsedImplicitly]

	// ReSharper disable once InconsistentNaming
	internal sealed class Migration_4: DbMigration
	{
		/// <summary>
		///   Performs migration.
		/// </summary>
		public override void Up(IDbTransformationProvider database) =>
			database.ChangeColumn(
				Settings.Default.SecuritySchemaName,
				"Roles",
				new Column("Sid", DbType.String, 64, ColumnProperty.Null));
	}
}