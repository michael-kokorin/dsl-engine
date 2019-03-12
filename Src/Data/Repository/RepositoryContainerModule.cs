namespace Repository
{
	using Microsoft.Practices.Unity;

	using Common.Container;
	using Common.Extensions;
	using Common.Transaction;
	using Repository.Context;
	using Repository.Extensions;
	using Repository.Localization;
	using Repository.Repositories;

	/// <summary>
	///   Represents container module for this project.
	/// </summary>
	/// <seealso cref="Common.Container.IContainerModule"/>
	public sealed class RepositoryContainerModule: IContainerModule
	{
		/// <summary>
		///   Registers binding to the specified container.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <param name="reuseScope">The reuse scope.</param>
		/// <returns>The container.</returns>
		public IUnityContainer Register(IUnityContainer container, ReuseScope reuseScope) => container

			// repositories
			.RegisterRepository<Authorities, IAuthorityRepository, AuthorityRepository>(reuseScope)
			.RegisterRepository<Configuration, IConfigurationRepository, ConfigurationRepository>(reuseScope)
			.RegisterRepository<Cultures, ICultureRepository, CultureRepository>(reuseScope)
			.RegisterRepository<NotificationRules, INotificationRuleRepository, NotificationRuleRepository>(reuseScope)
			.RegisterRepository<Projects, IProjectRepository, ProjectRepository>(reuseScope)
			.RegisterRepository<Plugins, IPluginRepository, PluginRepository>(reuseScope)
			.RegisterRepository<UserProjectSettings, IUserProjectSettingsRepository, UserProjectSettingsRepository>(
				reuseScope)
			.RegisterRepository<Reports, IReportRepository, ReportRepository>(reuseScope)
			.RegisterRepository<Roles, IRoleRepository, RoleRepository>(reuseScope)
			.RegisterRepository<RoleAuthorities, IRoleAuthorityRepository, RoleAuthorityRepository>(reuseScope)
			.RegisterRepository<Tasks, ITaskRepository, TaskRepository>(reuseScope)
			.RegisterLocalized<Tables, ITableRepository, TableRepository>(reuseScope)
			.RegisterLocalized<TableColumns, ITableColumnsRepository, TableColumnsRepository>(reuseScope)
			.RegisterRepository<Users, IUserRepository, UserRepository>(reuseScope)
			.RegisterRepository<UserInterfaces, IUserInterfaceRepository, UserInterfaceRepository>(reuseScope)
			.RegisterRepository<ScanAgents, IScanAgentRepository, ScanAgentRepository>(reuseScope)
			.RegisterRepository<TaskResults, ITaskResultRepository, TaskResultRepository>(reuseScope)
			.RegisterRepository<Events, IEventRepository, EventRepository>(reuseScope)
			.RegisterRepository<Queries, IQueryRepository, QueryRepository>(reuseScope)
			.RegisterRepository<Queue, IQueueRepository, QueueRepository>(reuseScope)
			.RegisterRepository<Templates, ITemplateRepository, TemplateRepository>(reuseScope)
			.RegisterRepository<QueryEntityNames, IQueryEntityNameRepository, QueryEntityNameRepository>(reuseScope)
			.RegisterRepository<WorkflowActions, IWorkflowActionRepository, WorkflowActionRepository>(reuseScope)
			.RegisterRepository<WorkflowRules, IWorkflowRuleRepository, WorkflowRuleRepository>(reuseScope)
			.RegisterRepository<PolicyRules, IPolicyRuleRepository, PolicyRuleRepository>(reuseScope)
			.RegisterRepository<SettingValues, ISettingValuesRepository, SettingValuesRepository>(reuseScope)
			.RegisterRepository<Settings, ISettingRepository, SettingRepository>(reuseScope)
			.RegisterRepository<SettingGroups, ISettingGroupRepository, SettingGroupRepository>(reuseScope)

			.RegisterRepository<ProjectTelemetry, IProjectTelemetryRepository, ProjectTelemetryRepository>(reuseScope)
			.RegisterRepository<QueryTelemetry, IQueryTelemetryRepositroy, QueryTelemetryRepository>(reuseScope)
			.RegisterRepository<ReportTelemetry, IReportTelemetryRepository, ReportTelemetryRepository>(reuseScope)
			.RegisterRepository<TaskTelemetry, ITaskTelemetryRepository, TaskTelemetryRepository>(reuseScope)
			.RegisterRepository<VcsPluginTelemetry, IVcsPluginTelemetryRepository, VcsPluginTelemetryRepository>(reuseScope)
			.RegisterRepository<ItPluginTelemetry, IItPluginTelemetryRepository, ItPluginTelemetryRepository>(reuseScope)

			// tools
			.RegisterType<IUserLocalizationProvider, UserLocalizationProvider>(reuseScope)
			.RegisterType<IDbContextFactory, SdlContextFactory>(reuseScope)
			.RegisterType<IUnitOfWork, UnitOfWork>(reuseScope)
			.RegisterType<IDbContextProvider, UnitOfWork>(reuseScope);
	}
}