namespace DbMigrations.Migrations.Migration_2
{
	using System.Data;

	using JetBrains.Annotations;

	using DbUpdateCommon.Properties;

	using ForeignKeyConstraint = DbMigrations.ForeignKeyConstraint;

	[UsedImplicitly]

	// ReSharper disable once InconsistentNaming
	internal sealed class Migration_2: DbMigration
	{
		private static void CreateCommonTables([NotNull] IDbTransformationProvider database)
		{
			database.AddTableWithIdPrimaryKeyColumn(
				Settings.Default.CommonSchemaName,
				"Configuration",
				new Column("Name", DbType.String, 255, ColumnProperty.NotNull),
				new Column("Value", DbType.String, DbConstants.NVarCharMax));

			database.AddUniqueConstraint(Settings.Default.CommonSchemaName, "Configuration", "Name");
		}

		private static void CreateIndependentDataTables([NotNull] IDbTransformationProvider database)
		{
			database.AddTableWithIdPrimaryKeyColumn(
				Settings.Default.DataSchemaName,
				"Events",
				new Column("Key", DbType.String, 100, ColumnProperty.NotNull),
				new Column("Name", DbType.String, 400, ColumnProperty.NotNull));

			database.AddUniqueConstraint(Settings.Default.DataSchemaName, "Events", "Key");

			database.AddTableWithIdPrimaryKeyColumn(
				Settings.Default.DataSchemaName,
				"QueryEntityNames",
				new Column("Key", DbType.String, 100, ColumnProperty.NotNull),
				new Column("TypeName", DbType.String, 400, ColumnProperty.NotNull),
				new Column("AssemblyName", DbType.String, 400, ColumnProperty.NotNull));

			database.AddUniqueConstraint(Settings.Default.DataSchemaName, "QueryEntityNames", "Key");

			database.AddTableWithIdPrimaryKeyColumn(
				Settings.Default.DataSchemaName,
				"ScanAgents",
				new Column("Version", DbType.String, 100, ColumnProperty.NotNull),
				new Column("Machine", DbType.String, 400, ColumnProperty.NotNull));

			database.AddTableWithIdPrimaryKeyColumn(
				Settings.Default.DataSchemaName,
				"ScanCores",
				new Column("Key", DbType.String, 64, ColumnProperty.NotNull),
				new Column("DisplayName", DbType.String, DbConstants.NVarCharMax, ColumnProperty.NotNull));

			database.AddUniqueConstraint(Settings.Default.DataSchemaName, "ScanCores", "Key");

			database.AddTableWithIdPrimaryKeyColumn(
				Settings.Default.DataSchemaName,
				"UserInterfaces",
				new Column("Host", DbType.String, 255, ColumnProperty.NotNull),
				new Column("RemoteIp", DbType.String, 15, ColumnProperty.NotNull),
				new Column("RemotePort", DbType.Int32, ColumnProperty.NotNull),
				new Column("Version", DbType.String, 12, ColumnProperty.NotNull),
				new Column("RegisteredUtc", DbType.DateTime2, ColumnProperty.NotNull),
				new Column("LastCheckedUtc", DbType.DateTime2, ColumnProperty.NotNull));

			database.AddUniqueConstraint(Settings.Default.DataSchemaName, "UserInterfaces", "Host");

			database.AddTableWithIdPrimaryKeyColumn(
				Settings.Default.DataSchemaName,
				"WorkflowActions",
				new Column("Key", DbType.String, 100, ColumnProperty.NotNull),
				new Column("DisplayName", DbType.String, 400, ColumnProperty.NotNull));

			database.AddUniqueConstraint(Settings.Default.DataSchemaName, "WorkflowActions", "Key");

			database.AddTableWithIdPrimaryKeyColumn(
				Settings.Default.DataSchemaName,
				"Plugins",
				new Column("Type", DbType.Int32, ColumnProperty.NotNull),
				new Column("DisplayName", DbType.String, 255, ColumnProperty.NotNull),
				new Column("AssemblyName", DbType.String, 255, ColumnProperty.NotNull),
				new Column("TypeFullName", DbType.String, 255, ColumnProperty.NotNull));

			database.AddUniqueConstraint(Settings.Default.DataSchemaName, "Plugins", "AssemblyName", "TypeFullName", "Type");

			database.AddTableWithIdPrimaryKeyColumn(
				Settings.Default.DataSchemaName,
				"Templates",
				new Column("Key", DbType.String, 100, ColumnProperty.NotNull),
				new Column("Title", DbType.String, DbConstants.NVarCharMax, ColumnProperty.NotNull),
				new Column("Body", DbType.String, DbConstants.NVarCharMax, ColumnProperty.NotNull));

			database.AddUniqueConstraint(Settings.Default.DataSchemaName, "Templates", "Key");
		}

		private static void CreateL10NTables([NotNull] IDbTransformationProvider database)
		{
			// ReSharper disable once ConvertMethodToExpressionBody
			database.AddTableWithIdPrimaryKeyColumn(
				Settings.Default.L10NSchemaName,
				"Cultures",
				new Column("Code", DbType.String, 5, ColumnProperty.NotNull),
				new Column("Name", DbType.String, 32, ColumnProperty.NotNull));
		}

		private static void CreateNotificationRulesTable([NotNull] IDbTransformationProvider database)
		{
			database.AddTableWithIdPrimaryKeyColumn(
				Settings.Default.DataSchemaName,
				"NotificationRules",
				new Column("ProjectId", DbType.Int64, ColumnProperty.NotNull),
				new Column("DisplayName", DbType.String, 400, ColumnProperty.NotNull),
				new Column("Description", DbType.String, DbConstants.NVarCharMax),
				new Column("Query", DbType.String, DbConstants.NVarCharMax, ColumnProperty.NotNull),
				new Column("IsEventTriggered", DbType.Boolean, ColumnProperty.NotNull),
				new Column("IsTimeTriggered", DbType.Boolean, ColumnProperty.NotNull),
				new Column("Start", DbType.DateTime2),
				new Column("IsRepeatable", DbType.Boolean),
				new Column("Repeat", DbType.String, 50),
				new Column("Added", DbType.DateTime2, ColumnProperty.NotNull),
				new Column("DeliveryProtocol", DbType.Int32, ColumnProperty.NotNull),
				new Column("TemplateKey", DbType.String, 400, ColumnProperty.NotNull),
				new Column("IsForAllEvents", DbType.Boolean, ColumnProperty.NotNull),
				new Column("IsForAllSubjects", DbType.Boolean, ColumnProperty.NotNull));

			database.AddForeignKey(
				Settings.Default.DataSchemaName,
				"NotificationRules",
				"ProjectId",
				Settings.Default.DataSchemaName,
				"Projects",
				constraint: ForeignKeyConstraint.Cascade);

			database.AddTableWithIdPrimaryKeyColumn(
				Settings.Default.DataSchemaName,
				"NotificationRuleToEvents",
				new Column("NotificationRuleId", DbType.Int64, ColumnProperty.NotNull),
				new Column("EventId", DbType.Int64, ColumnProperty.NotNull));

			database.AddForeignKey(
				Settings.Default.DataSchemaName,
				"NotificationRuleToEvents",
				"NotificationRuleId",
				Settings.Default.DataSchemaName,
				"NotificationRules",
				"Id",
				ForeignKeyConstraint.Cascade);

			database.AddForeignKey(
				Settings.Default.DataSchemaName,
				"NotificationRuleToEvents",
				"EventId",
				Settings.Default.DataSchemaName,
				"Events",
				constraint: ForeignKeyConstraint.Cascade);
		}

		private static void CreatePolicyRulesTable([NotNull] IDbTransformationProvider database)
		{
			database.AddTableWithIdPrimaryKeyColumn(
				Settings.Default.DataSchemaName,
				"PolicyRules",
				new Column("Name", DbType.String, 64, ColumnProperty.NotNull),
				new Column("Query", DbType.String, DbConstants.NVarCharMax, ColumnProperty.NotNull),
				new Column("ProjectId", DbType.Int64, ColumnProperty.NotNull),
				new Column("Added", DbType.DateTime2, ColumnProperty.NotNull));

			database.AddForeignKey(
				Settings.Default.DataSchemaName,
				"PolicyRules",
				"ProjectId",
				Settings.Default.DataSchemaName,
				"Projects",
				constraint: ForeignKeyConstraint.Cascade);
		}

		private static void CreateProjectTable([NotNull] IDbTransformationProvider database)
		{
			database.AddTableWithIdPrimaryKeyColumn(
				Settings.Default.DataSchemaName,
				"Projects",
				new Column("Alias", DbType.String, 255, ColumnProperty.NotNull),
				new Column("DefaultBranchName", DbType.String, 255, ColumnProperty.NotNull),
				new Column("Created", DbType.DateTime2, ColumnProperty.NotNull),
				new Column("CreatedById", DbType.Int64, ColumnProperty.NotNull),
				new Column("DisplayName", DbType.String, 255, ColumnProperty.NotNull),
				new Column("Modified", DbType.DateTime2, ColumnProperty.NotNull),
				new Column("ModifiedById", DbType.Int64, ColumnProperty.NotNull),
				new Column("ScanCoreId", DbType.Int64, ColumnProperty.NotNull),
				new Column("ItPluginId", DbType.Int64, ColumnProperty.NotNull),
				new Column("VcsPluginId", DbType.Int64, ColumnProperty.NotNull),
				new Column("SdlPolicyStatus", DbType.Int32, ColumnProperty.NotNull),
				new Column("Description", DbType.String, DbConstants.NVarCharMax));

			database.AddUniqueConstraint(Settings.Default.DataSchemaName, "Projects", "Alias");
			database.AddForeignKey(
				Settings.Default.DataSchemaName,
				"Projects",
				"CreatedById",
				Settings.Default.SecuritySchemaName,
				"Users");

			database.AddForeignKey(
				Settings.Default.DataSchemaName,
				"Projects",
				"ModifiedById",
				Settings.Default.SecuritySchemaName,
				"Users");

			database.AddForeignKey(
				Settings.Default.DataSchemaName,
				"Projects",
				"ScanCoreId",
				Settings.Default.DataSchemaName,
				"ScanCores");

			database.AddForeignKey(
				Settings.Default.DataSchemaName,
				"Projects",
				"ItPluginId",
				Settings.Default.DataSchemaName,
				"Plugins");

			database.AddForeignKey(
				Settings.Default.DataSchemaName,
				"Projects",
				"VcsPluginId",
				Settings.Default.DataSchemaName,
				"Plugins");
		}

		private static void CreateQueueTables([NotNull] IDbTransformationProvider database)
		{
			// ReSharper disable once ConvertMethodToExpressionBody
			database.AddTableWithIdPrimaryKeyColumn(
				Settings.Default.QueueSchemaName,
				"Queue",
				new Column("Type", DbType.String, 32, ColumnProperty.NotNull),
				new Column("Created", DbType.DateTime2, ColumnProperty.NotNull),
				new Column("IsProcessed", DbType.Boolean, ColumnProperty.NotNull, 0),
				new Column("Processed", DbType.DateTime2),
				new Column("Body", DbType.Xml, ColumnProperty.NotNull));
		}

		private static void CreateReportsTable([NotNull] IDbTransformationProvider database)
		{
			database.AddTableWithIdPrimaryKeyColumn(
				Settings.Default.ReportSchemaName,
				"Reports",
				new Column("ProjectId", DbType.Int64),
				new Column("DisplayName", DbType.String, 255, ColumnProperty.NotNull),
				new Column("Created", DbType.DateTime2, ColumnProperty.NotNull),
				new Column("CreatedById", DbType.Int64, ColumnProperty.NotNull),
				new Column("Modified", DbType.DateTime2, ColumnProperty.NotNull),
				new Column("ModifiedById", DbType.Int64, ColumnProperty.NotNull),
				new Column("Request", DbType.String, DbConstants.NVarCharMax, ColumnProperty.NotNull),
				new Column("Description", DbType.String, DbConstants.NVarCharMax));

			database.AddForeignKey(
				Settings.Default.ReportSchemaName,
				"Reports",
				"CreatedById",
				Settings.Default.SecuritySchemaName,
				"Users");

			database.AddForeignKey(
				Settings.Default.ReportSchemaName,
				"Reports",
				"ModifiedById",
				Settings.Default.SecuritySchemaName,
				"Users");

			database.AddForeignKey(
				Settings.Default.ReportSchemaName,
				"Reports",
				"ProjectId",
				Settings.Default.DataSchemaName,
				"Projects");
		}

		private static void CreateRolesTable([NotNull] IDbTransformationProvider database)
		{
			database.AddTableWithIdPrimaryKeyColumn(
				Settings.Default.SecuritySchemaName,
				"Roles",
				new Column("Sid", DbType.String, 64, ColumnProperty.NotNull),
				new Column("Alias", DbType.String, 64, ColumnProperty.NotNull),
				new Column("DisplayName", DbType.String, 255, ColumnProperty.NotNull),
				new Column("ProjectId", DbType.Int64),
				new Column("TasksQuery", DbType.String, DbConstants.NVarCharMax),
				new Column("TaskResultsQuery", DbType.String, DbConstants.NVarCharMax));

			database.AddForeignKey(
				Settings.Default.SecuritySchemaName,
				"Roles",
				"ProjectId",
				Settings.Default.DataSchemaName,
				"Projects",
				constraint: ForeignKeyConstraint.Cascade);

			database.AddUniqueConstraint(Settings.Default.SecuritySchemaName, "Roles", "ProjectId", "Alias");

			database.AddTableWithIdPrimaryKeyColumn(
				Settings.Default.SecuritySchemaName,
				"RoleAuthorities",
				new Column("RoleId", DbType.Int64, ColumnProperty.NotNull),
				new Column("AuthorityId", DbType.Int64, ColumnProperty.NotNull));

			database.AddForeignKey(
				Settings.Default.SecuritySchemaName,
				"RoleAuthorities",
				"RoleId",
				Settings.Default.SecuritySchemaName,
				"Roles",
				constraint: ForeignKeyConstraint.Cascade);

			database.AddForeignKey(
				Settings.Default.SecuritySchemaName,
				"RoleAuthorities",
				"AuthorityId",
				Settings.Default.SecuritySchemaName,
				"Authorities",
				constraint: ForeignKeyConstraint.Cascade);

			database.AddUniqueConstraint(Settings.Default.SecuritySchemaName, "RoleAuthorities", "RoleId", "AuthorityId");
		}

		private static void CreateSchemas([NotNull] IDbTransformationProvider database)
		{
			database.CreateSchema(Settings.Default.CommonSchemaName);
			database.CreateSchema(Settings.Default.DataSchemaName);
			database.CreateSchema(Settings.Default.QueueSchemaName);
			database.CreateSchema(Settings.Default.ReportSchemaName);
			database.CreateSchema(Settings.Default.SecuritySchemaName);
			database.CreateSchema(Settings.Default.L10NSchemaName);
		}

		private static void CreateSecurityTables([NotNull] IDbTransformationProvider database)
		{
			database.AddTableWithIdPrimaryKeyColumn(
				Settings.Default.SecuritySchemaName,
				"Users",
				new Column("Sid", DbType.String, 185, ColumnProperty.NotNull),
				new Column("Login", DbType.String, 104, ColumnProperty.NotNull),
				new Column("DisplayName", DbType.String, DbConstants.NVarCharMax, ColumnProperty.NotNull),
				new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, 1),
				new Column("Email", DbType.String, 100));

			database.AddUniqueConstraint(Settings.Default.SecuritySchemaName, "Users", "Sid");

			database.AddTableWithIdPrimaryKeyColumn(
				Settings.Default.SecuritySchemaName,
				"Authorities",
				new Column("Key", DbType.String, 255, ColumnProperty.NotNull),
				new Column("DisplayName", DbType.String, 255, ColumnProperty.NotNull));

			database.AddUniqueConstraint(Settings.Default.SecuritySchemaName, "Authorities", "Key");
		}

		private static void CreateSettingsTables([NotNull] IDbTransformationProvider database)
		{
			database.AddTableWithIdPrimaryKeyColumn(
				Settings.Default.SecuritySchemaName,
				"UserProjectSettings",
				new Column("ProjectId", DbType.Int64, ColumnProperty.NotNull),
				new Column("UserId", DbType.Int64, ColumnProperty.NotNull),
				new Column("PreferedRoleId", DbType.Int64, ColumnProperty.NotNull));

			database.AddUniqueConstraint(Settings.Default.SecuritySchemaName, "UserProjectSettings", "ProjectId", "UserId");
			database.AddForeignKey(
				Settings.Default.SecuritySchemaName,
				"UserProjectSettings",
				"ProjectId",
				Settings.Default.DataSchemaName,
				"Projects",
				constraint: ForeignKeyConstraint.Cascade);

			database.AddForeignKey(
				Settings.Default.SecuritySchemaName,
				"UserProjectSettings",
				"UserId",
				Settings.Default.SecuritySchemaName,
				"Users",
				constraint: ForeignKeyConstraint.Cascade);

			database.AddForeignKey(
				Settings.Default.SecuritySchemaName,
				"UserProjectSettings",
				"PreferedRoleId",
				Settings.Default.SecuritySchemaName,
				"Roles");

			database.AddTableWithIdPrimaryKeyColumn(
				Settings.Default.DataSchemaName,
				"PluginSettings",
				new Column("PluginId", DbType.Int64, ColumnProperty.NotNull),
				new Column("Key", DbType.String, 32, ColumnProperty.NotNull),
				new Column("DisplayName", DbType.String, 255, ColumnProperty.NotNull),
				new Column("Level", DbType.Int32, ColumnProperty.NotNull),
				new Column("ValueType", DbType.Int32, ColumnProperty.NotNull));

			database.AddUniqueConstraint(Settings.Default.DataSchemaName, "PluginSettings", "PluginId", "Key");
			database.AddForeignKey(
				Settings.Default.DataSchemaName,
				"PluginSettings",
				"PluginId",
				Settings.Default.DataSchemaName,
				"Plugins",
				constraint: ForeignKeyConstraint.Cascade);

			database.AddTableWithIdPrimaryKeyColumn(
				Settings.Default.DataSchemaName,
				"ProjectPluginSettings",
				new Column("ProjectId", DbType.Int64, ColumnProperty.NotNull),
				new Column("SettingId", DbType.Int64, ColumnProperty.NotNull),
				new Column("Value", DbType.String, 255));

			database.AddUniqueConstraint(Settings.Default.DataSchemaName, "ProjectPluginSettings", "ProjectId", "SettingId");
			database.AddForeignKey(
				Settings.Default.DataSchemaName,
				"ProjectPluginSettings",
				"ProjectId",
				Settings.Default.DataSchemaName,
				"Projects",
				constraint: ForeignKeyConstraint.Cascade);

			database.AddForeignKey(
				Settings.Default.DataSchemaName,
				"ProjectPluginSettings",
				"SettingId",
				Settings.Default.DataSchemaName,
				"PluginSettings",
				constraint: ForeignKeyConstraint.Cascade);

			database.AddTableWithIdPrimaryKeyColumn(
				Settings.Default.DataSchemaName,
				"UserPluginSettings",
				new Column("ProjectId", DbType.Int64, ColumnProperty.NotNull),
				new Column("UserId", DbType.Int64, ColumnProperty.NotNull),
				new Column("SettingId", DbType.Int64, ColumnProperty.NotNull),
				new Column("Value", DbType.String, 255));

			database.AddForeignKey(
				Settings.Default.DataSchemaName,
				"UserPluginSettings",
				"ProjectId",
				Settings.Default.DataSchemaName,
				"Projects",
				constraint: ForeignKeyConstraint.Cascade);

			database.AddForeignKey(
				Settings.Default.DataSchemaName,
				"UserPluginSettings",
				"UserId",
				Settings.Default.SecuritySchemaName,
				"Users",
				constraint: ForeignKeyConstraint.Cascade);

			database.AddForeignKey(
				Settings.Default.DataSchemaName,
				"UserPluginSettings",
				"SettingId",
				Settings.Default.DataSchemaName,
				"PluginSettings",
				constraint: ForeignKeyConstraint.Cascade);
		}

		private static void CreateTables([NotNull] IDbTransformationProvider database)
		{
			CreateCommonTables(database);
			CreateQueueTables(database);
			CreateL10NTables(database);
			CreateSecurityTables(database);
			CreateIndependentDataTables(database);
			CreateProjectTable(database);
			CreateRolesTable(database);
			CreateNotificationRulesTable(database);
			CreateWorkflowRulesTable(database);
			CreatePolicyRulesTable(database);
			CreateSettingsTables(database);
			CreateTasksTable(database);
			CreateTaskResultsTable(database);
			CreateReportsTable(database);
		}

		private static void CreateTaskResultsTable([NotNull] IDbTransformationProvider database)
		{
			database.AddTableWithIdPrimaryKeyColumn(
				Settings.Default.DataSchemaName,
				"TaskResults",
				new Column("TaskId", DbType.Int64, ColumnProperty.NotNull),
				new Column("Message", DbType.String, DbConstants.NVarCharMax),
				new Column("ExploitGraph", DbType.String, DbConstants.NVarCharMax),
				new Column("AdditionalExploitConditions", DbType.String, DbConstants.NVarCharMax),
				new Column("File", DbType.String, 400, ColumnProperty.NotNull),
				new Column("Function", DbType.String, 400),
				new Column("LineNumber", DbType.Int32, ColumnProperty.NotNull),
				new Column("Place", DbType.String, 400),
				new Column("SourceFile", DbType.String, 400),
				new Column("RawLine", DbType.String, DbConstants.NVarCharMax, ColumnProperty.NotNull),
				new Column("Type", DbType.String, 400, ColumnProperty.NotNull),
				new Column("TypeShort", DbType.String, 10),
				new Column("Description", DbType.String, DbConstants.NVarCharMax),
				new Column("SeverityType", DbType.Int32, ColumnProperty.NotNull, 0),
				new Column("LinePosition", DbType.Int32, ColumnProperty.NotNull),
				new Column("IssueNumber", DbType.Int32, ColumnProperty.NotNull),
				new Column("IssueUrl", DbType.String, 400, ColumnProperty.NotNull));

			database.AddForeignKey(
				Settings.Default.DataSchemaName,
				"TaskResults",
				"TaskId",
				Settings.Default.DataSchemaName,
				"Tasks",
				constraint: ForeignKeyConstraint.Cascade);
		}

		private static void CreateTasksTable([NotNull] IDbTransformationProvider database)
		{
			database.AddTableWithIdPrimaryKeyColumn(
				Settings.Default.DataSchemaName,
				"Tasks",
				new Column("ProjectId", DbType.Int64, ColumnProperty.NotNull),
				new Column("Created", DbType.DateTime2, ColumnProperty.NotNull),
				new Column("CreatedById", DbType.Int64, ColumnProperty.NotNull),
				new Column("Finished", DbType.DateTime2),
				new Column("Modified", DbType.DateTime2, ColumnProperty.NotNull),
				new Column("ModifiedById", DbType.Int64, ColumnProperty.NotNull),
				new Column("Repository", DbType.String, 255, ColumnProperty.NotNull),
				new Column("ScanCoreId", DbType.Int64, ColumnProperty.NotNull),
				new Column("Status", DbType.Int32, ColumnProperty.NotNull),
				new Column("SdlStatus", DbType.Int32, ColumnProperty.NotNull),
				new Column("FolderPath", DbType.String, 400),
				new Column("LogPath", DbType.String, 400),
				new Column("ResultPath", DbType.String, 400),
				new Column("FolderSize", DbType.Int64),
				new Column("ScanCoreWorkingTime", DbType.Int64),
				new Column("AnalyzedFiles", DbType.String, DbConstants.NVarCharMax),
				new Column("AnalyzedSize", DbType.Int64),
				new Column("AnalyzedLinesCount", DbType.Int64),
				new Column("LowSeverityVulns", DbType.Int32),
				new Column("MediumSeverityVulns", DbType.Int32),
				new Column("HighSeverityVulns", DbType.Int32),
				new Column("FP", DbType.Int32),
				new Column("Todo", DbType.Int32),
				new Column("Reopen", DbType.Int32),
				new Column("Fixed", DbType.Int32),
				new Column("IncrementFP", DbType.Int32),
				new Column("IncrementTodo", DbType.Int32),
				new Column("IncrementReopen", DbType.Int32),
				new Column("IncrementFixed", DbType.Int32));

			database.AddForeignKey(
				Settings.Default.DataSchemaName,
				"Tasks",
				"ProjectId",
				Settings.Default.DataSchemaName,
				"Projects",
				constraint: ForeignKeyConstraint.Cascade);

			database.AddForeignKey(
				Settings.Default.DataSchemaName,
				"Tasks",
				"CreatedById",
				Settings.Default.SecuritySchemaName,
				"Users",
				constraint: ForeignKeyConstraint.Cascade);

			database.AddForeignKey(
				Settings.Default.DataSchemaName,
				"Tasks",
				"ModifiedById",
				Settings.Default.SecuritySchemaName,
				"Users");

			database.AddForeignKey(
				Settings.Default.DataSchemaName,
				"Tasks",
				"ScanCoreId",
				Settings.Default.DataSchemaName,
				"ScanCores",
				constraint: ForeignKeyConstraint.Cascade);
		}

		private static void CreateWorkflowRulesTable([NotNull] IDbTransformationProvider database)
		{
			database.AddTableWithIdPrimaryKeyColumn(
				Settings.Default.DataSchemaName,
				"WorkflowRules",
				new Column("ProjectId", DbType.Int64, ColumnProperty.NotNull),
				new Column("DisplayName", DbType.String, 400, ColumnProperty.NotNull),
				new Column("Description", DbType.String, DbConstants.NVarCharMax),
				new Column("Query", DbType.String, DbConstants.NVarCharMax, ColumnProperty.NotNull),
				new Column("IsEventTriggered", DbType.Boolean, ColumnProperty.NotNull),
				new Column("IsTimeTriggered", DbType.Boolean, ColumnProperty.NotNull),
				new Column("Start", DbType.DateTime2),
				new Column("IsRepeatable", DbType.Boolean),
				new Column("Repeat", DbType.String, 50),
				new Column("Added", DbType.DateTime2, ColumnProperty.NotNull),
				new Column("ActionKey", DbType.String, 400, ColumnProperty.NotNull),
				new Column("IsForAllEvents", DbType.Boolean, ColumnProperty.NotNull));

			database.AddForeignKey(
				Settings.Default.DataSchemaName,
				"WorkflowRules",
				"ProjectId",
				Settings.Default.DataSchemaName,
				"Projects",
				constraint: ForeignKeyConstraint.Cascade);

			database.AddTableWithIdPrimaryKeyColumn(
				Settings.Default.DataSchemaName,
				"WorkflowRuleToEvents",
				new Column("WorkflowRuleId", DbType.Int64, ColumnProperty.NotNull),
				new Column("EventId", DbType.Int64, ColumnProperty.NotNull));

			database.AddForeignKey(
				Settings.Default.DataSchemaName,
				"WorkflowRuleToEvents",
				"WorkflowRuleId",
				Settings.Default.DataSchemaName,
				"WorkflowRules",
				constraint: ForeignKeyConstraint.Cascade);

			database.AddForeignKey(
				Settings.Default.DataSchemaName,
				"WorkflowRuleToEvents",
				"EventId",
				Settings.Default.DataSchemaName,
				"Events",
				constraint: ForeignKeyConstraint.Cascade);
		}

		public override void Up(IDbTransformationProvider database)
		{
			CreateSchemas(database);
			CreateTables(database);
		}
	}
}