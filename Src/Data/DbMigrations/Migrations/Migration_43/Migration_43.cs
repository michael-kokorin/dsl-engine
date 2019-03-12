namespace DbMigrations.Migrations.Migration_43
{
	using System.Data;

	using JetBrains.Annotations;

	using DbUpdateCommon.Properties;

	[UsedImplicitly]
	// ReSharper disable once InconsistentNaming
	internal sealed class Migration_43: DbMigration
	{
		/// <summary>
		///     Performs migration.
		/// </summary>
		public override void Up(IDbTransformationProvider database)
		{
			database.AddColumn(
				Settings.Default.DataSchemaName,
				"Projects",
				"EnablePoll",
				DbType.Boolean,
				ColumnProperty.NotNull, 0);

			database.AddColumn(
				Settings.Default.DataSchemaName,
				"Projects",
				"PollTimeout",
				DbType.Int32,
				ColumnProperty.Null);
		}
	}
}