namespace Infrastructure.Scheduler
{
	using System;

	using Microsoft.Practices.Unity;

	using Common.Logging;
	using Common.Time;
	using Common.Transaction;
	using Infrastructure.Telemetry;

	using Quartz;

	/// <summary>
	///     Represents a job to run.
	/// </summary>
	public abstract class ScheduledJob: IJob
	{
		/// <summary>
		///     Gets or sets the interval of the next job run.
		/// </summary>
		/// <value>Interval of the next job run.</value>
		public virtual TimeSpan Interval { get; set; } = new TimeSpan(0, 0, 0, 20);

		/// <summary>
		///     Gets the job execution context.
		/// </summary>
		/// <value>
		///     The job execution context.
		/// </value>
		protected IJobExecutionContext JobExecutionContext { get; private set; }

		public bool TelemetryRequired = false;

		/// <summary>
		///     Gets or sets the container.
		/// </summary>
		/// <value>
		///     The container.
		/// </value>
		internal IUnityContainer Container { get; set; }

		/// <summary>
		///     Called by the <see cref="T:Quartz.IScheduler"/> when a <see cref="T:Quartz.ITrigger"/>
		///     fires that is associated with the <see cref="T:Quartz.IJob"/>.
		/// </summary>
		/// <remarks>
		///     The implementation may wish to set a  result object on the
		///     JobExecutionContext before this method exits.  The result itself
		///     is meaningless to Quartz, but may be informative to
		///     <see cref="T:Quartz.IJobListener"/>s or
		///     <see cref="T:Quartz.ITriggerListener"/>s that are watching the job's
		///     execution.
		/// </remarks>
		/// <param name="context">The execution context.</param>
		public virtual void Execute(IJobExecutionContext context)
		{
			JobExecutionContext = context;

			var log = Container.Resolve<ILog>();

			var unitOfWork = Container.Resolve<IUnitOfWork>();

			var timeService = Container.Resolve<ITimeService>();

			ITelemetryQueue telemetryQueue = null;

			if(TelemetryRequired)
			{
				telemetryQueue = Container.Resolve<ITelemetryQueue>();

				telemetryQueue.AutoCommit = false;
			}

			var totalEntitiesProcessed = 0;

			var iterationEntitiesProcessed = 0;

			var startTime = timeService.GetUtc();

			do
			{
				log.Trace($"Scheduler job started. Job name='{GetType().FullName}'");

				try
				{
					using(var transaction = unitOfWork.BeginTransaction())
					{
						iterationEntitiesProcessed = Process();

						unitOfWork.Commit();

						transaction.Commit();
					}

					totalEntitiesProcessed += iterationEntitiesProcessed;

					log.Trace(
						$"Scheduler job iteration done. Job name='{GetType().FullName}', Entities processed='{iterationEntitiesProcessed}'");

					OnIterationDone(iterationEntitiesProcessed);
				}
				catch(Exception ex)
				{
					unitOfWork.Reset();

					log.Error($"Scheduler job failed. Job name='{GetType().FullName}'", ex);

					OnFailed(ex);
				}

				if (TelemetryRequired)
					telemetryQueue?.Commit();
			}
			while(iterationEntitiesProcessed > 0);

			var finishTime = timeService.GetUtc();

			var executionTime = finishTime - startTime;

			log.Debug(
				$"Scheduler job execution finished. Job name='{GetType().FullName}', Entities processed='{totalEntitiesProcessed}', Execution time='{executionTime}'");
		}

		/// <summary>
		///     Called when job is failed while execution
		/// </summary>
		// ReSharper disable once VirtualMemberNeverOverriden.Global
		// ReSharper disable once UnusedParameter.Global
		protected virtual void OnFailed(Exception exc)
		{
		}

		/// <summary>
		///     Called when job is finished successfully.
		/// </summary>
		// ReSharper disable once VirtualMemberNeverOverriden.Global
		// ReSharper disable once UnusedParameter.Global
		protected virtual void OnIterationDone(int entitiesProcessed)
		{
		}

		/// <summary>
		///     Executes the job.
		/// </summary>
		/// <returns>
		///     Positive value to repeat the task, negative value or 0 - to finish the task and to wait time interval before the
		///     next run.
		/// </returns>
		protected abstract int Process();
	}
}