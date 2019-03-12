namespace DbMigrations.Migrations.Migration_41
{
	using System.Collections.Generic;

	using JetBrains.Annotations;

	using DbUpdateCommon.Properties;

	// ReSharper disable once InconsistentNaming
	[UsedImplicitly]
	internal sealed class Migration_41 : DbMigration
	{
		private readonly IEnumerable<string> _columnsToDelete;

		public Migration_41()
		{
			_columnsToDelete = new[]
			{
				"IsForAllSubjects",
				"IsForAllEvents",
				"TemplateKey",
				"DeliveryProtocol",
				"Repeat",
				"IsRepeatable",
				"Start",
				"IsTimeTriggered",
				"IsEventTriggered"
			};
		}

		/// <summary>
		///   Performs migration.
		/// </summary>
		public override void Up(IDbTransformationProvider database)
		{
			if(database.TableExists(Settings.Default.DataSchemaName, "NotificationRuleToEvents"))
				database.RemoveTable(Settings.Default.DataSchemaName, "NotificationRuleToEvents");

			foreach (var column in _columnsToDelete)
			{
				RemoveColumn(database, column);
			}
		}

		private static void RemoveColumn(IDbTransformationProvider database, string columnName)
		{
			if (database.ColumnExists(Settings.Default.DataSchemaName, "NotificationRules", columnName))
				database.RemoveColumn(Settings.Default.DataSchemaName, "NotificationRules", columnName);
		}
	}
}