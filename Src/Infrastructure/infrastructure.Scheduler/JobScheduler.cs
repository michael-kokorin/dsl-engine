using System.Linq;

namespace Infrastructure.Scheduler
{
	using System;

	using JetBrains.Annotations;

	using Microsoft.Practices.Unity;

	using Common.Extensions;
	using Common.Logging;
	using Infrastructure.Scheduler.Properties;

	using Quartz;
	using Quartz.Impl;
	using Quartz.Spi;

	[UsedImplicitly]
	internal sealed class JobScheduler: IJobScheduler
	{
		private readonly IJobFactory _jobFactory;

		private readonly IJobNameBuilder _jobNameBuilder;

		private readonly IUnityContainer _unityContainer;

		private readonly ILog _log;

		private readonly ISchedulerJobTypesLoader _schedulerJobTypesLoader;

		private readonly ITriggerBuilder _triggerBuilder;

		private IScheduler _scheduler;

		public JobScheduler(

			[NotNull] ILog log,
			[NotNull] ITriggerBuilder triggerBuilder,
			[NotNull] ISchedulerJobTypesLoader schedulerJobTypesLoader,
			[NotNull] IJobFactory jobFactory,
			[NotNull] IJobNameBuilder jobNameBuilder,
			[NotNull] IUnityContainer unityContainer)
		{
			_schedulerJobTypesLoader = schedulerJobTypesLoader;
			_jobFactory = jobFactory;
			_jobNameBuilder = jobNameBuilder;
			_unityContainer = unityContainer;
			_triggerBuilder = triggerBuilder;
			_log = log;
		}

		/// <summary>
		///   Start jobs.
		/// </summary>
		public void Start(bool telemetryRequired)
		{
			if(_scheduler != null)
			{
				_log.Debug(Resources.JobScheduler_Start_Scheduler_already_started);

				return;
			}

			_scheduler = StdSchedulerFactory.GetDefaultScheduler();
			_scheduler.JobFactory = _jobFactory;

			var jobs = _schedulerJobTypesLoader.Load();

			var jobIndex = 0;

			foreach(var job in jobs.Where(job => !(job is ICustomScheduledJob)))
			{
				jobIndex++;

				var jobType = job.GetType();

				var scheduledJob = JobBuilder
					.Create(jobType)
					.WithIdentity(_jobNameBuilder.GetJobName(job))
					.Build();

				job.TelemetryRequired = telemetryRequired;

				var trigger = _triggerBuilder.CreateTrigger(job, jobIndex);

				_scheduler.ScheduleJob(scheduledJob, trigger);

				_log.Trace(Resources.JobScheduler_Start_Scheduled_job.FormatWith(jobType.FullName));
			}

			var initializers = _unityContainer.ResolveAll<ICustomJobInitializer>();

			foreach(var customJobInitializer in initializers)
			{
				customJobInitializer.Initialize(_scheduler);
			}

			_scheduler.Start();

			_log.Trace(Resources.JobScheduler_Start_Scheduler_started);
		}

		/// <summary>
		///   Stop jobs.
		/// </summary>
		public void Stop()
		{
			if(_scheduler == null)
			{
				return;
			}

			try
			{
				_scheduler.Shutdown();

				_log.Debug(Resources.JobScheduler_Stop_Scheduler_succesfully_stopped);
			}
			catch(Exception exc)
			{
				_log.Error(Resources.JobScheduler_Stop_Error_throwen_while_scheduler_stopping, exc);
			}
			finally
			{
				_scheduler = null;
			}
		}

		public void DeleteJob(string jobKey) => _scheduler.DeleteJob(new JobKey(jobKey));

		public void StartJob(IJobDetail job, ITrigger trigger) => _scheduler.ScheduleJob(job, trigger);

		/// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
		/// <filterpriority>2</filterpriority>
		public void Dispose() => Stop();
	}
}