namespace Workflow.SA
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Enums;
	using Infrastructure;
	using Infrastructure.Events;
	using Infrastructure.Scheduler;
	using Infrastructure.Telemetry;
	using Repository.Context;
	using Repository.Repositories;

	using Settings = Workflow.SA.Properties.Settings;

	/// <summary>
	///     Provides base behavior for scan task processing job.
	/// </summary>
	/// <seealso cref="Infrastructure.Scheduler.ScheduledJob"/>
	internal abstract class ScanTaskJob: ScheduledJob
	{
		private readonly IEventProvider _eventProvider;

		private readonly ITaskRepository _taskRepository;

		private readonly ITelemetryScopeProvider _telemetryScopeProvider;

		protected ScanTaskJob(
			[NotNull] IEventProvider eventProvider,
			[NotNull] ITaskRepository taskRepository,
			[NotNull] ITelemetryScopeProvider telemetryScopeProvider)
		{
			if(eventProvider == null) throw new ArgumentNullException(nameof(eventProvider));
			if(taskRepository == null) throw new ArgumentNullException(nameof(taskRepository));
			if(telemetryScopeProvider == null) throw new ArgumentNullException(nameof(telemetryScopeProvider));

			_eventProvider = eventProvider;
			_taskRepository = taskRepository;
			_telemetryScopeProvider = telemetryScopeProvider;
		}

		/// <summary>
		///     Gets the end name of the event.
		/// </summary>
		/// <value>
		///     The end name of the event.
		/// </value>
		[NotNull]
		protected abstract string EndEventName { get; }

		/// <summary>
		///     Gets the end status.
		/// </summary>
		/// <value>
		///     The end status.
		/// </value>
		protected abstract TaskStatus EndStatus { get; }

		/// <summary>
		///     Gets the start name of the event.
		/// </summary>
		/// <value>
		///     The start name of the event.
		/// </value>
		[NotNull]
		protected abstract string StartEventName { get; }

		/// <summary>
		///     Gets the start status.
		/// </summary>
		/// <value>
		///     The start status.
		/// </value>
		protected abstract TaskStatus StartStatus { get; }

		/// <summary>
		///     Gets the take status.
		/// </summary>
		/// <value>
		///     The take status.
		/// </value>
		protected abstract TaskStatus TakeStatus { get; }

		/// <summary>
		///     Gets the interval of the next job run.
		/// </summary>
		/// <value>Interval of the next job run.</value>
		public override TimeSpan Interval => Settings.Default.TaskIterationSleep;

		/// <summary>
		///     Gets the next task.
		/// </summary>
		/// <returns>The task.</returns>
		[CanBeNull]
		private Tasks GetNextTask() =>
			_taskRepository
				.GetByStatus(TakeStatus)
				.OrderBy(_ => _.Created)
				.FirstOrDefault();

		/// <summary>
		///     Makes the end.
		/// </summary>
		/// <param name="task">The task.</param>
		private void MakeEnd([NotNull] Tasks task)
		{
			task.TaskStatus = EndStatus;

			PublishEndEvent(task.Id, task.ProjectId);
		}

		/// <summary>
		///     Makes the start.
		/// </summary>
		/// <param name="task">The task.</param>
		private void MakeStart([NotNull] Tasks task)
		{
			task.TaskStatus = StartStatus;

			PublishStartEvent(task.Id, task.ProjectId);
		}

		/// <summary>
		///     Executes the job.
		/// </summary>
		/// <returns>
		///     Positive value to repeat the task, negative value or 0 - to finish the task and to wait time interval before the
		///     next run.
		/// </returns>
		protected override int Process()
		{
			var task = GetNextTask();

			if(task == null)
			{
				return 0;
			}

			using(var telemetryScope = _telemetryScopeProvider.Create<Tasks>(TelemetryOperationNames.Task.Update))
			{
				try
				{
					telemetryScope.SetEntity(task);

					MakeStart(task);

					ProcessTask(task);

					MakeEnd(task);

					telemetryScope.WriteSuccess();
				}
				catch (Exception ex)
				{
					telemetryScope.WriteException(ex);

					try
					{
						MakeFail(task);
					}
					catch (Exception internalException)
					{
						telemetryScope.WriteException(internalException);
					}
				}
			}

			return 1;
		}

		private void MakeFail([NotNull] Tasks task)
		{
			task.Fail();
			_taskRepository.Save();
			PublishTaskEvent(EventKeys.ScanTask.Finished, task.Id, task.ProjectId);
		}

		/// <summary>
		///     Processes the task.
		/// </summary>
		/// <param name="task">The task.</param>
		protected abstract void ProcessTask(Tasks task);

		/// <summary>
		///     Publishes the end event.
		/// </summary>
		/// <param name="taskId">The task identifier.</param>
		/// <param name="projectId">The project identifier.</param>
		protected virtual void PublishEndEvent(long taskId, long projectId)
			=> PublishTaskEvent(EndEventName, taskId, projectId);

		/// <summary>
		///     Publishes the start event.
		/// </summary>
		/// <param name="taskId">The task identifier.</param>
		/// <param name="projectId">The project identifier.</param>
		private void PublishStartEvent(long taskId, long projectId) =>
			PublishTaskEvent(StartEventName, taskId, projectId);

		/// <summary>
		///     Publishes the task event.
		/// </summary>
		/// <param name="eventName">Name of the event.</param>
		/// <param name="taskId">The task identifier.</param>
		/// <param name="projectId">The project identifier.</param>
		protected void PublishTaskEvent(string eventName, long taskId, long projectId) =>
			_eventProvider.Publish(
				new Event
				{
					Key = eventName,
					Data = new Dictionary<string, string>
							{
								{Variables.TaskId, taskId.ToString()},
								{Variables.ProjectId, projectId.ToString()}
							}
				});
	}
}