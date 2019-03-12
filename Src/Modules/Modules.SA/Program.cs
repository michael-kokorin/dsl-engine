namespace Modules.SA
{
	using System;
	using System.Threading;

	using JetBrains.Annotations;

	using Common;
	using Common.Container;
	using Common.Logging;
	using Infrastructure.Plugins.Common;
	using Infrastructure.Scheduler;
	using Modules.SA.Properties;

	/// <summary>
	///     Contains entry point of the application.
	/// </summary>
	internal static class Program
	{
		/// <summary>
		///     The main entry point for the application.
		/// </summary>
		private static void Main([UsedImplicitly] string[] args)
		{
			IoC.InitDefault(
				ReuseScope.Container,
				new CommonContainerModule(),
				new ScanAgentContainerModule(),
				new CommonPluginsContainerModule(),
				new SchedulerContainerModule());

			var logger = IoC.Resolve<ILog>();

			logger.Trace(Resources.SAStarted);

			try
			{
				var pluginProvider = IoC.Resolve<IPluginProvider>();

				pluginProvider.Initialize();

				logger.Trace("Plugins initialized: \n" + string.Join("\n", pluginProvider.GetAllAvailablePluginKeys()));

				var jobScheduler = IoC.Resolve<IJobScheduler>();

				jobScheduler.Start(false);

				logger.Trace("Job scheduler started");

				Thread.Sleep(Timeout.Infinite);
			}
			catch(Exception exception)
			{
				logger.Error(Resources.SA, exception);
			}

			logger.Trace(Resources.SAFinished);
		}
	}
}