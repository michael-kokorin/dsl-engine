namespace Modules.Core
{
	using Microsoft.Practices.Unity;

	using Common.Container;
	using Common.Data;
	using Common.Extensions;
	using Common.Settings;
	using Modules.Core.Contracts.Dto;
	using Modules.Core.Contracts.Query.Dto;
	using Modules.Core.Contracts.Report.Dto;
	using Modules.Core.Contracts.UI.Dto;
	using Modules.Core.Contracts.UI.Dto.Admin;
	using Modules.Core.Contracts.UI.Dto.Data;
	using Modules.Core.Contracts.UI.Dto.UserSettings;
	using Modules.Core.Environment;
	using Modules.Core.Helpers;
	using Modules.Core.Services.Query.CommandHandlers;
	using Modules.Core.Services.Query.Commands;
	using Modules.Core.Services.Query.DataQueries;
	using Modules.Core.Services.Query.DataQueryHandlers;
	using Modules.Core.Services.Report.CommandHandlers;
	using Modules.Core.Services.Report.Commands;
	using Modules.Core.Services.Report.DataQueries;
	using Modules.Core.Services.Report.DataQueryHandlers;
	using Modules.Core.Services.UI.Commands;
	using Modules.Core.Services.UI.Handlers;
	using Modules.Core.Services.UI.Queries;
	using Modules.Core.Services.UI.QueryHandlers;

	/// <summary>
	///   Represents container module for the project.
	/// </summary>
	/// <seealso cref="Common.Container.IContainerModule"/>
	internal sealed class CoreContainerModule : IContainerModule
	{
		public IUnityContainer Register(IUnityContainer container, ReuseScope reuseScope) => container
			.RegisterType<IDataSourceInitializer, DataSourceInitializer>(reuseScope)
			.RegisterType<IEnvironmentProvider, EnvironmentProvider>(reuseScope)
			.RegisterType<ISettingsHelper, SettingsHelper>(reuseScope)
			.RegisterType<ISettingConditionValidatorProvider, SettingConditionValidatorProvider>(reuseScope)
			.RegisterType<ISettingConditionValidator, MinSettingConditionValidator>(
				typeof(MinSettingConditionValidator).FullName,
				reuseScope)
			.RegisterType<ISettingValidator, SettingValidator>(reuseScope)
			.RegisterType<IDataSourceAccessInitializer, DataSourceAccessInitializer>(reuseScope)
			.RegisterType<IConfigManager, WebConfigManager>(reuseScope)

			// command handlers
			.RegisterCommandHandler<CreateProjectCommand, CreateProjectCommandHandler>(reuseScope)
			.RegisterCommandHandler<CreateTaskCommand, CreateTaskCommandHandler>(reuseScope)
			.RegisterCommandHandler<StopTaskCommand, StopTaskCommandHandler>(reuseScope)
			.RegisterCommandHandler<UpdateProjectSettingsCommand, UpdateProjectSettingsCommandHandler>(reuseScope)
			.RegisterCommandHandler<UpdateProjectPluginSettingsCommand, UpdateProjectPluginSettingsCommandHandler>(reuseScope)
			.RegisterCommandHandler<UpdateNotificationRuleCommand, UpdateNotificationRuleCommandHandler>(reuseScope)

			// query handlers
			// UI API
			.RegisterDataQueryHandler<GetUsersByProjectQuery, UserDto[], GetUsersByProjectQueryHandler>(reuseScope)
			.RegisterDataQueryHandler<GetProjectTasksQuery, TableDto, GetProjectTasksQueryHandler>(reuseScope)
			.RegisterDataQueryHandler<GetTaskResultsQuery, TableDto, GetTaskResultsQueryHandler>(reuseScope)
			.RegisterDataQueryHandler<GetRolesByUserQuery, UserRoleDto[], GetRolesByUserQueryHandler>(reuseScope)
			.RegisterDataQueryHandler<GetRolesByProjectQuery, UserRoleDto[], GetRolesByProjectQueryHandler>(reuseScope)
			.RegisterDataQueryHandler<GetNotificationQuery, NotificationRuleDto, GetNotificationQueryHandler>(reuseScope)
			.RegisterDataQueryHandler<GetSystemSettingsQuery, SettingsDto, GetSystemSettingsQueryHandler>(reuseScope)
			.RegisterDataQueryHandler<GetPluginsByTypeQuery, PluginDto[], GetPluginsByTypeQueryHandler>(reuseScope)
			.RegisterDataQueryHandler<GetPluginByIdQuery, PluginDto, GetPluginByIdQueryHandler>(reuseScope)
			.RegisterDataQueryHandler<GetPluginsByProjectQuery, PluginDto[], GetPluginsByProjectQueryHandler>(reuseScope)
			.RegisterDataQueryHandler<GetCapabilityByKeyQuery, string, GetCapabilityByKeyQueryHandler>(reuseScope)
			.RegisterDataQueryHandler<GetLicenceInfoQuery, LicenceInfoDto, GetLicenceInfoQueryHandler>(reuseScope)

			// Query API
			.RegisterDataQueryHandler<QueryByIdQuery, QueryDto, QueryByIdQueryHandler>(reuseScope)
			.RegisterDataQueryHandler<CanEditQueryQuery, bool, CanEditQueryQueryHandler>(reuseScope)
			.RegisterDataQueryHandler<GetDataSourceFieldsQuery, DataSourceFieldDto[], GetDataSourceFieldsQueryHandler>(reuseScope)
			.RegisterDataQueryHandler<DataSourcesQuery, DataSourceDto[], DataSourcesQueryHandler>(reuseScope)
			.RegisterDataQueryHandler<GetQueriesListQuery, TableDto, GetQueriesListQueryHandler>(reuseScope)
			.RegisterDataQueryHandler<QueryByNameQuery, QueryDto, QueryByNameQueryHandler>(reuseScope)
			.RegisterDataQueryHandler<GetQueryPrivacyQuery, ReferenceItemDto[], GetQueryPrivacyQueryHandler>(reuseScope)
			.RegisterDataQueryHandler<GetQueryResultsQuery, TableDto, GetQueryResultsQueryHandler>(reuseScope)
			.RegisterDataQueryHandler<GetQueryTextQuery, string, GetQueryTextQueryHandler>(reuseScope)
			.RegisterDataQueryHandler<GetQueryModelQuery, string, GetQueryModelQueryHandler>(reuseScope)
			.RegisterDataQueryHandler<GetQueryResultsQuery, TableDto, GetQueryResultsQueryHandler>(reuseScope)
			.RegisterDataQueryHandler<GetQueryFilterConditionsQuery, ReferenceItemDto[], GetQueryFilterConditionsQueryHandler>(
				reuseScope)
			.RegisterDataQueryHandler<GetQueryFilterOperationsQuery, ReferenceItemDto[], GetQueryFilterOperationsQueryHandler>(
				reuseScope)
			.RegisterCommandHandler<CreateQueryCommand, CreateQueryCommandHandler>(reuseScope)
			.RegisterCommandHandler<UpdateQueryCommand, UpdateQueryCommandHandler>(reuseScope)

			// Report API
			.RegisterDataQueryHandler<BuildReportQuery, ReportFileDto, BuildReportQueryHandler>(reuseScope)
			.RegisterDataQueryHandler<GetReportQuery, ReportDto, GetReportQueryHandler>(reuseScope)
			.RegisterDataQueryHandler<GetReportsListQuery, TableDto, GetReportsListQueryHandler>(reuseScope)
			.RegisterCommandHandler<DeleteReportCommand, DeleteReportCommandHandler>(reuseScope)
			.RegisterCommandHandler<UpdateReportCommand, UpdateReportCommandHandler>(reuseScope)

			// request executors
			.RegisterDataQueryHandler<GetQueryModelQuery, string, GetQueryModelQueryHandler>(reuseScope)
			.RegisterDataQueryHandler<GetSortDirectionsQuery, ReferenceItemDto[], GetSortDirectionsQueryHandler>(reuseScope);
	}
}