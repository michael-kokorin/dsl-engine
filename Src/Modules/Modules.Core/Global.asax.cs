namespace Modules.Core
{
	using System;
	using System.Web;
	using System.Web.Mvc;
	using System.Web.Routing;

	using Microsoft.Practices.Unity;

	using Common;
	using Common.Container;
	using Common.Logging;
	using DbActions;
	using DbMigrations;
	using DbUpdateCommon;
	using Infrastructure;
	using Infrastructure.AD;
	using Infrastructure.Engines;
	using Infrastructure.Engines.Dsl;
	using Infrastructure.Notifications;
	using Infrastructure.Plugins;
	using Infrastructure.Plugins.Common;
	using Infrastructure.Query;
	using Infrastructure.Reports;
	using Infrastructure.RequestHandling;
	using Infrastructure.Rules;
	using Infrastructure.Scheduler;
	using Infrastructure.UserInterface;
	using Modules.Core.Environment;
	using Packages;
	using Repository;
	using Workflow;
	using Workflow.GitHub;
	using Workflow.SA;

	/// <summary>
	///     Represents the application itself.
	/// </summary>
	/// <seealso cref="System.Web.HttpApplication"/>
	public class Global: HttpApplication
	{
		private IUnityContainer _schedulerContainer;

		public static IJobScheduler JobScheduler { get; private set; }

		/// <summary>
		///     Handles the AuthenticateRequest event of the Application control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		protected void Application_AuthenticateRequest(object sender, EventArgs e)
		{
		}

		/// <summary>
		///     Handles the BeginRequest event of the Application control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		protected void Application_BeginRequest(object sender, EventArgs e)
		{
		}

		/// <summary>
		///     Handles the End event of the Application control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		protected void Application_End(object sender, EventArgs e)
		{
		}

		/// <summary>
		///     Handles the Error event of the Application control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		protected void Application_Error(object sender, EventArgs e)
		{
			var exception = Server.GetLastError();
			var logger = IoC.GetContainer().Resolve<ILog>();
			logger.Fatal("Core error occurred", exception);
		}

		/// <summary>
		///     Handles the Start event of the Application control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		protected void Application_Start(object sender, EventArgs e)
		{
			var modules =
				new IContainerModule[]
				{
					new AdContainerModule(),
					new CommonContainerModule(),
					new EnginesContainerModule(),
					new CommonPluginsContainerModule(),
					new PluginsContainerModule(),
					new RepositoryContainerModule(),
					new CoreContainerModule(),
					new QueryContainerModule(),
					new EnginesDslModule(),
					new InfrastructureContainerModule(),
					new UserInterfaceInfrastructureContainerModule(),
					new PackagesContainerModule(),
					new DbMigrationsModule(),
					new DbUpdateCommonModule(),
					new DbActionsModule(),
					new RequestHandlingModule(),
					new ReportContainerModule(),
					new RulesContainerModule(),
					new NotificationsContainerModule(),
					new WorkflowContainerModule(),
					new ScanWorkflowModule(),
					new SchedulerContainerModule(),
					new GitHubWorkflowContainerModule()
				};

			IoC.InitDefault(ReuseScope.PerRequest, modules);

			var globalContainer = IoC.GetContainer();

			var migrator = globalContainer.Resolve<IDbMigrator>();
			migrator.MigrationLatest();

			EnvironmentInitializer.Initialize(
				modules,
				ReuseScope.Container,
				container =>
				{
					var env = container.Resolve<IEnvironmentProvider>();

					env.Prepare();
				});

			_schedulerContainer = EnvironmentInitializer.InitializeContainer(modules, ReuseScope.Hierarchy);
			JobScheduler = _schedulerContainer.Resolve<IJobScheduler>();
			JobScheduler.Start(true);

			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
		}

		/// <summary>
		///     Handles the End event of the Session control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		protected void Session_End(object sender, EventArgs e)
		{
		}

		/// <summary>
		///     Handles the Start event of the Session control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		protected void Session_Start(object sender, EventArgs e)
		{
		}
	}
}