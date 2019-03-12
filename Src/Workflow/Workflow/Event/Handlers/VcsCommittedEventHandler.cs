namespace Workflow.Event.Handlers
{
	using System;
	using System.Collections.Generic;

	using JetBrains.Annotations;

	using Common.Enums;
	using Common.Extensions;
	using Common.Logging;
	using Common.Security;
	using Common.Time;
	using Infrastructure;
	using Infrastructure.Events;
	using Infrastructure.Telemetry;
	using Repository.Context;
	using Repository.Repositories;
	using Workflow.Properties;

	[UsedImplicitly]
	internal sealed class VcsCommittedEventHandler: IEventHandler
	{
		private readonly IEventProvider _eventProvider;

		private readonly ILog _log;

		private readonly ITaskRepository _taskRepository;

		private readonly ITelemetryScopeProvider _telemetryScopeProvider;

		private readonly ITimeService _timeService;

		private readonly IUserPrincipal _userPrincipal;

		public VcsCommittedEventHandler(
			[NotNull] IEventProvider eventProvider,
			[NotNull] ILog log,
			[NotNull] ITaskRepository taskRepository,
			[NotNull] ITelemetryScopeProvider telemetryScopeProvider,
			[NotNull] ITimeService timeService,
			[NotNull] IUserPrincipal userPrincipal)
		{
			if(eventProvider == null) throw new ArgumentNullException(nameof(eventProvider));
			if(log == null) throw new ArgumentNullException(nameof(log));
			if(taskRepository == null) throw new ArgumentNullException(nameof(taskRepository));
			if(telemetryScopeProvider == null) throw new ArgumentNullException(nameof(telemetryScopeProvider));
			if(timeService == null) throw new ArgumentNullException(nameof(timeService));
			if(userPrincipal == null) throw new ArgumentNullException(nameof(userPrincipal));

			_eventProvider = eventProvider;
			_log = log;
			_taskRepository = taskRepository;
			_telemetryScopeProvider = telemetryScopeProvider;
			_timeService = timeService;
			_userPrincipal = userPrincipal;
		}

		public bool CanHandle(Event eventToHandle) => eventToHandle.Key == EventKeys.VcsCommitted;

		public void Handle(Event eventToHandle)
		{
			using(var telemetryScope = _telemetryScopeProvider.Create<Tasks>(TelemetryOperationNames.Task.Create))
			{
				try
				{
					var projectId = long.Parse(eventToHandle.Data[Variables.ProjectId]);

					var task = new Tasks
								{
									Created = _timeService.GetUtc(),
									CreatedById = _userPrincipal.Info.Id,
									Finished = null,
									Modified = _timeService.GetUtc(),
									ModifiedById = _userPrincipal.Info.Id,
									ProjectId = projectId,
									Repository = eventToHandle.Data[Variables.Branch],
									SdlStatus = (int)SdlPolicyStatus.Unknown
								};

					telemetryScope.SetEntity(task);

					_taskRepository.Insert(task);

					// TODO добавить копирования настроек с проекта
					_eventProvider.Publish(
						new Event
						{
							Key = EventKeys.ScanTask.Created,
							Data = new Dictionary<string, string>
									{
										{Variables.ProjectId, projectId.ToString()},
										{Variables.TaskId, task.Id.ToString()}
									}
						});

					telemetryScope.WriteSuccess();

					_taskRepository.Save();

					_log.Debug(Resources.TaskCreated.FormatWith(task.Id, projectId, task.Repository));
				}
				catch(Exception ex)
				{
					telemetryScope.WriteException(ex);

					throw;
				}

			}
		}
	}
}