namespace DbMigrations.Migrations.Migration_44
{
	using System.Data;

	using JetBrains.Annotations;

	using DbUpdateCommon.Properties;

	[UsedImplicitly]

	// ReSharper disable once InconsistentNaming
	internal sealed class Migration_44 : DbMigration
	{
		/// <summary>
		///   Performs migration.
		/// </summary>
		public override void Up(IDbTransformationProvider database)
		{
			if (database.TableExists(Settings.Default.DataSchemaName, "NotificationRuleToEvents"))
				database.RemoveTable(Settings.Default.DataSchemaName, "NotificationRuleToEvents");

			const int commitByDefault = 1;

			if (!database.ColumnExists(Settings.Default.DataSchemaName, "Projects", "CommitToVcs"))
			{
				database.AddColumn(
					Settings.Default.DataSchemaName,
					"Projects",
					new Column("CommitToVcs", DbType.Boolean, ColumnProperty.NotNull, commitByDefault));
			}

			if (!database.ColumnExists(Settings.Default.DataSchemaName, "Projects", "CommitToIt"))
			{
				database.AddColumn(
					Settings.Default.DataSchemaName,
					"Projects",
					new Column("CommitToIt", DbType.Boolean, ColumnProperty.NotNull, commitByDefault));
			}

			database.ExecuteNonQuery("ALTER TABLE [data].[TaskResults] ALTER COLUMN [IssueNumber] NVARCHAR(MAX) NULL;");

			database.ExecuteNonQuery("ALTER TABLE [data].[TaskResults] ALTER COLUMN [IssueUrl] NVARCHAR(MAX) NULL;");
		}
	}
}