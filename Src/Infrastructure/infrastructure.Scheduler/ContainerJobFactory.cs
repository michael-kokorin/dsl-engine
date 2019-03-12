namespace Infrastructure.Scheduler
{
	using System;

	using JetBrains.Annotations;

	using Microsoft.Practices.Unity;

	using Common.Extensions;
	using Common.Logging;
	using Infrastructure.Scheduler.Properties;

	using Quartz;
	using Quartz.Spi;

	[UsedImplicitly]
	internal sealed class ContainerJobFactory: IJobFactory
	{
		private readonly IUnityContainer _container;

		private readonly ILog _log;

		public ContainerJobFactory(
			[NotNull] IUnityContainer container,
			[NotNull] ILog log)
		{
			_container = container;
			_log = log;
		}

		public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
		{
			if(bundle == null) throw new ArgumentNullException(nameof(bundle));
			if(scheduler == null) throw new ArgumentNullException(nameof(scheduler));

			var jobDetail = bundle.JobDetail;

			var jobContainer = _container.CreateChildContainer();

			var newJob = jobContainer.Resolve<ScheduledJob>(jobDetail.JobType.FullName);

			newJob.Container = jobContainer;

			_log.Trace(Resources.ContainerJobFactory_NewJob_JobCreated.FormatWith(newJob.GetType().FullName));

			return newJob;
		}

		public void ReturnJob(IJob job)
		{
			if(job == null) throw new ArgumentNullException(nameof(job));

			var baseJob = job as ScheduledJob;

			if(baseJob == null) return;

			baseJob.Container.Dispose();

			_log.Trace(
				Resources.ContainerJobFactory_ReturnJob_ContainerDisposed.FormatWith(baseJob.GetType().FullName));
		}
	}
}