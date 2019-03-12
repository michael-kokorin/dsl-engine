namespace DbMigrations.Migrations.Migration_48
{
	using System.Data;

	using JetBrains.Annotations;

	using DbUpdateCommon.Properties;

	[UsedImplicitly]
	// ReSharper disable once InconsistentNaming
	internal sealed class Migration_48: DbMigration
	{
		/// <summary>
		///     Performs migration.
		/// </summary>
		public override void Up(IDbTransformationProvider database)
		{
			if(!database.ColumnExists(Settings.Default.DataSchemaName, "Projects", "EnablePoll"))
			{
				database.AddColumn(
				Settings.Default.DataSchemaName,
				"Projects",
				"EnablePoll",
				DbType.Boolean,
				ColumnProperty.NotNull, 0);
			}

			if(!database.ColumnExists(Settings.Default.DataSchemaName, "Projects", "PollTimeout"))
			{
				database.AddColumn(
				Settings.Default.DataSchemaName,
				"Projects",
				"PollTimeout",
				DbType.Int32,
				ColumnProperty.Null);
			}

			if(!database.ColumnExists(Settings.Default.DataSchemaName, "Settings", "Conditions"))
			{
				database.AddColumn(
					Settings.Default.DataSchemaName,
					"Settings",
					"Conditions",
					DbType.String,
					4000,
					ColumnProperty.Null);
			}
		}
	}
}