namespace DbMigrations.Migrations.Migration_16
{
	using System.Data;

	using JetBrains.Annotations;

	using DbUpdateCommon.Properties;

	[UsedImplicitly]

	// ReSharper disable once InconsistentNaming
	internal sealed class Migration_16: DbMigration
	{
		private readonly RoleQueriesBundle[] _bundles;

		public Migration_16()
		{
			_bundles = new[]
								{
									new RoleQueriesBundle("sdl_admin", "Administrator tasks query", "Administrator task results query"),
									new RoleQueriesBundle("developer", "Developer tasks query", "Developer task results query"),
									new RoleQueriesBundle("securitymanager", "Security manager tasks query", "Security manager task results query")
								};
		}

		/// <summary>
		///   Performs migration.
		/// </summary>
		public override void Up(IDbTransformationProvider database)
		{
			const string tasksQueryIdColumnName = "TasksQueryId";
			const string taskResultsQueryIdColumnName = "TaskResultsQueryId";

			database.RemoveColumn(Settings.Default.SecuritySchemaName, "Roles", "TasksQuery");
			database.RemoveColumn(Settings.Default.SecuritySchemaName, "Roles", "TaskResultsQuery");

			database.AddColumn(
				Settings.Default.SecuritySchemaName,
				"Roles",
				tasksQueryIdColumnName,
				DbType.Int64,
				ColumnProperty.Null,
				null);
			database.AddColumn(
				Settings.Default.SecuritySchemaName,
				"Roles",
				taskResultsQueryIdColumnName,
				DbType.Int64,
				ColumnProperty.Null,
				null);

			database.AddForeignKey(
				Settings.Default.SecuritySchemaName,
				"Roles",
				tasksQueryIdColumnName,
				Settings.Default.DataSchemaName,
				"Queries");
			database.AddForeignKey(
				Settings.Default.SecuritySchemaName,
				"Roles",
				taskResultsQueryIdColumnName,
				Settings.Default.DataSchemaName,
				"Queries");

			// ReSharper disable once LoopCanBePartlyConvertedToQuery
			foreach(var bundle in _bundles)
			{
				var tasksQueryId = database.ExecuteScalar(
					$"SELECT [q].[Id] FROM [data].[Queries] [q] WHERE [q].[Name] = N'{bundle.TasksQueryName}'");

				var taskResultsQueryId = database.ExecuteScalar(
					$"SELECT [q].[Id] FROM [data].[Queries] [q] WHERE [q].[Name] = N'{bundle.TaskResultsQueryName}'");

				var update = "UPDATE [security].[Roles] " +
										"SET " +
										"[TasksQueryId] = " + tasksQueryId + ", " +
										"[TaskResultsQueryId] = " + taskResultsQueryId +
										" WHERE [Alias] = N'" + bundle.RoleAlias + "'";

				database.ExecuteNonQuery(update);
			}

			database.ChangeColumn(
				Settings.Default.SecuritySchemaName,
				"Roles",
				new Column(tasksQueryIdColumnName, DbType.Int64, ColumnProperty.NotNull));
			database.ChangeColumn(
				Settings.Default.SecuritySchemaName,
				"Roles",
				new Column(taskResultsQueryIdColumnName, DbType.Int64, ColumnProperty.NotNull));
		}

		private sealed class RoleQueriesBundle
		{
			public readonly string RoleAlias;

			public readonly string TaskResultsQueryName;

			public readonly string TasksQueryName;

			public RoleQueriesBundle(string roleAlias, string tasksQueryName, string taskResultsQueryName)
			{
				RoleAlias = roleAlias;
				TasksQueryName = tasksQueryName;
				TaskResultsQueryName = taskResultsQueryName;
			}
		}
	}
}