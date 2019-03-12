namespace Infrastructure
{
	using Microsoft.Practices.Unity;

	using Common.Container;
	using Common.Extensions;
	using Infrastructure.DataSource;
	using Infrastructure.Events;
	using Infrastructure.Extensions;
	using Infrastructure.Mail;
	using Infrastructure.MessageQueue;
	using Infrastructure.Policy;
	using Infrastructure.Security;
	using Infrastructure.Tags;
	using Infrastructure.Telemetry.Entities;
	using Infrastructure.Templates;
	using Infrastructure.Vulnerability;
	using Repository.Context;

	using Telemetry;

	public sealed class InfrastructureContainerModule: IContainerModule
	{
		public IUnityContainer Register(IUnityContainer container, ReuseScope reuseScope) => container
			.RegisterType<IDataSourceAccessValidator, DataSourceAccessValidator>(reuseScope)
			.RegisterType<IDataSourceFieldAccessValidator, DataSourceFieldAccessValidator>(reuseScope)
			.RegisterType<IDataSourceFieldInfoProvider, DataSourceFieldInfoProvider>(reuseScope)
			.RegisterType<IDataSourceInfoProvider, DataSourceInfoProvider>(reuseScope)
			.RegisterType<IConfigurationProvider, DatabaseConfigurationProvider>(reuseScope)
			.RegisterType<IMailClient, MailClient>(reuseScope)
			.RegisterType<IMailConnectionParametersProvider, DatabaseMailConnectionParametersProvider>(reuseScope)
			.RegisterType<IMailProvider, MailProvider>(reuseScope)
			.RegisterType<IEventProvider, EventProvider>(reuseScope)
			.RegisterType<IMessageQueue, DatabaseMessageQueue>(reuseScope)
			.RegisterType<ISdlPolicyProvider, SdlPolicyProvider>(reuseScope)
			.RegisterType<ITemplateProvider, TemplateProvider>(reuseScope)
			.RegisterType<ITemplateBuilder, TemplateBuilder>(reuseScope)
			.RegisterType<IAuthorityProvider, AuthorityProvider>(reuseScope)
			.RegisterType<IVulnerabilitySeverityResolver, XmlVulnerabilitySeverityResolver>(reuseScope)
			.RegisterType<IVulnerabilityShortTypeResolver, XmlVulnerabilityShortTypeResolver>(reuseScope)
			.RegisterType<IDataSourceAuthorityNameBuilder, DataSourceAuthorityNameBuilder>(reuseScope)
			.RegisterType<IDataSourceAuthorityProvider, DataSourceAuthorityProvider>(reuseScope)
			.RegisterType<ITagDataSourceProvider, TagDataSourceProvider>(reuseScope)
			.RegisterType<ITagEntityProvider, TagEntityProvider>(reuseScope)
			.RegisterType<ITagEntityRepositoryProvider, TagEntityRepositoryProvider>(reuseScope)
			.RegisterType<ITagProvider, TagProvider>(reuseScope)
			.RegisterType<ITagService, TagService>(reuseScope)
			.RegisterType<ITagValidator, TagValidator>(reuseScope)

			.RegisterType<IEntityTelemetryCreatorProvider, EntityTelemetryCreatorProvider>(reuseScope)
			.RegisterType<ITelemetryInitializer, TelemetryInitializer>(reuseScope)
			.RegisterType<ITelemetryRepositoryResolver, TelemetryRepositoryResolver>(reuseScope)
			.RegisterType<ITelemetryQueue, TelemetryQueue>(reuseScope)
			.RegisterType<ITelemetryRouter, TelemetryRouter>(reuseScope)
			.RegisterType<ITelemetryScopeProvider, TelemetryScopeProvider>(reuseScope)
			.RegisterType<ITelemetryScopeProvider, TelemetryScopeProvider>(reuseScope)

			.RegisterEntityTelemetryCreator<Projects, ProjectTelemetryCreator>(reuseScope)
			.RegisterEntityTelemetryCreator<Queries, QueryTelemetryCreator>(reuseScope)
			.RegisterEntityTelemetryCreator<Reports, ReportTelemetryCreator>(reuseScope)
			.RegisterEntityTelemetryCreator<Tasks, TaskTelemetryCreator>(reuseScope)
			.RegisterEntityTelemetryCreator<VcsPluginInfo, VcsPluginTelemetryCreator>(reuseScope)
			.RegisterEntityTelemetryCreator<ItPluginInfo, ItPluginTelemetryCreator>(reuseScope);
	}
}