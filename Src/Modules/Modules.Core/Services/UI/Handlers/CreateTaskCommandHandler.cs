namespace Modules.Core.Services.UI.Handlers
{
	using System;
	using System.Collections.Generic;

	using JetBrains.Annotations;

	using Common.Enums;
	using Common.Security;
	using Common.Time;
	using Common.Transaction;
	using Infrastructure;
	using Infrastructure.Events;
	using Infrastructure.Telemetry;
	using Modules.Core.Helpers;
	using Modules.Core.Services.UI.Commands;
	using Repository.Context;
	using Repository.Repositories;

	using Authorities = Common.Security.Authorities;

	[UsedImplicitly]
	internal sealed class CreateTaskCommandHandler: CommandHandler<CreateTaskCommand>
	{
		private readonly IEventProvider _eventProvider;

		private readonly ITaskRepository _repositoryTasks;

		private readonly ISettingsHelper _settingsHelper;

		private readonly ITelemetryScopeProvider _telemetryScopeProvider;

		private readonly ITimeService _timeService;

		private readonly IUserPrincipal _userPrincipal;

		public CreateTaskCommandHandler(
			[NotNull] IUserAuthorityValidator userAuthorityValidator,
			[NotNull] ITimeService timeService,
			[NotNull] IUserPrincipal userPrincipal,
			[NotNull] ITaskRepository repositoryTasks,
			[NotNull] ITelemetryScopeProvider telemetryScopeProvider,
			[NotNull] IUnitOfWork unitOfWork,
			[NotNull] IEventProvider eventProvider,
			[NotNull] ISettingsHelper settingsHelper)
			: base(userAuthorityValidator, unitOfWork, userPrincipal)
		{
			if(userAuthorityValidator == null) throw new ArgumentNullException(nameof(userAuthorityValidator));
			if(timeService == null) throw new ArgumentNullException(nameof(timeService));
			if(userPrincipal == null) throw new ArgumentNullException(nameof(userPrincipal));
			if(repositoryTasks == null) throw new ArgumentNullException(nameof(repositoryTasks));
			if(telemetryScopeProvider == null) throw new ArgumentNullException(nameof(telemetryScopeProvider));
			if(eventProvider == null) throw new ArgumentNullException(nameof(eventProvider));
			if(settingsHelper == null) throw new ArgumentNullException(nameof(settingsHelper));

			_telemetryScopeProvider = telemetryScopeProvider;
			_timeService = timeService;
			_userPrincipal = userPrincipal;
			_repositoryTasks = repositoryTasks;
			_eventProvider = eventProvider;
			_settingsHelper = settingsHelper;
		}

		protected override string RequestedAuthorityName => Authorities.UI.Project.Tasks.CreateNewTask;

		protected override long? GetProjectIdForCommand(CreateTaskCommand command) => command.ProjectId;

		protected override void ProcessAuthorized(CreateTaskCommand command)
		{
			using(var telemetryScope = _telemetryScopeProvider.Create<Tasks>(TelemetryOperationNames.Task.Create))
			{
				try
				{
					var currentDateTime = _timeService.GetUtc();

					var task = new Tasks
					{
						Created = currentDateTime,
						CreatedById = _userPrincipal.Info.Id,
						Finished = null,
						Modified = currentDateTime,
						ModifiedById = _userPrincipal.Info.Id,
						ProjectId = command.ProjectId,
						Repository = command.Repository,
						SdlStatus = (int)SdlPolicyStatus.Unknown
					};

					telemetryScope.SetEntity(task);

					_repositoryTasks.Insert(task);

					command.CreatedTaskId = task.Id;

			_eventProvider.Publish(
				new Event
				{
					Key = EventKeys.ScanTask.Created,
					Data = new Dictionary<string, string>
							{
								{Variables.ProjectId, command.ProjectId.ToString()},
								{Variables.TaskId, task.Id.ToString()}
							}
				});

					_repositoryTasks.Save();

					telemetryScope.WriteSuccess();
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