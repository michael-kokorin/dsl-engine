namespace DbActions.ActionsStore
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Common.Extensions;
	using DbUpdateCommon;
	using DbUpdateCommon.Properties;

	internal sealed class AddRoleAction: DbAction
	{
		/// <summary>
		///   Gets the key.
		/// </summary>
		/// <value>
		///   The key.
		/// </value>
		public override string Key => "AddRole";

		/// <summary>
		///   Gets the version.
		/// </summary>
		/// <value>
		///   The version.
		/// </value>
		public override Version Version { get; } = new Version(1, 0, 0, 0);

		/// <summary>
		///   Executes action on the specified database.
		/// </summary>
		/// <param name="database">The database.</param>
		/// <param name="parameters">The parameters.</param>
		public override void Execute(IDbInformationProvider database, Dictionary<string, object> parameters)
		{
			var alias = parameters.Get("alias") as string;
			var name = parameters.Get("name") as string;
			var projectAlias = parameters.Get("projectAlias") as string;
			var tasksQuery = parameters.Get("tasksQuery") as string;
			var taskResultsQuery = parameters.Get("taskResultsQuery") as string;
			var authorities = parameters.Get("authorities") as string[] ?? new string[0];

			database.Insert(
				database.GetFullTableName(Settings.Default.SecuritySchemaName, "Roles"),
				new[] {"Alias", "DisplayName", "ProjectId", "TasksQuery", "TaskResultsQuery"},
				new[]
				{
					alias,
					name,
					$"(select Id from {database.GetFullTableName(Settings.Default.DataSchemaName, "Projects")} where Alias='{projectAlias}')",
					tasksQuery,
					taskResultsQuery
				});

			database.Insert(
				database.GetFullTableName(Settings.Default.SecuritySchemaName, "RoleAuthorities"),
				new[]
				{
					"RoleId",
					"AuthorityId"
				},
				authorities.Select(
					_ => new[]
							{
								$"(select Id from {database.GetFullTableName(Settings.Default.SecuritySchemaName, "Roles")} where [Alias]='{alias}')",
								$"(select Id from {database.GetFullTableName(Settings.Default.SecuritySchemaName, "Authorities")} where [Key]='{_}')"
							}));
		}
	}
}