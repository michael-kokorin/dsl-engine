namespace Modules.Core.Services.UI.Handlers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Enums;
	using Common.Security;
	using Common.Time;
	using Common.Transaction;
	using Infrastructure;
	using Infrastructure.Events;
	using Infrastructure.Telemetry;
	using Modules.Core.Services.UI.Commands;
	using Repository;
	using Repository.Context;

	using Authorities = Common.Security.Authorities;

	[UsedImplicitly]
	internal sealed class StopTaskCommandHandler: CommandHandler<StopTaskCommand>
	{
		private readonly IEventProvider _eventProvider;

		private readonly IWriteRepository<Tasks> _repositoryTasks;

		private readonly ITelemetryScopeProvider _telemetryScopeProvider;

		private readonly ITimeService _timeService;

		private readonly IUserPrincipal _userPrincipal;

		public StopTaskCommandHandler(
			IUserAuthorityValidator userAuthorityValidator,
			IWriteRepository<Tasks> repositoryTasks,
			IUserPrincipal userPrincipal,
			ITimeService timeService,
			IUnitOfWork unitOfWork,
			[NotNull] IEventProvider eventProvider,
			[NotNull] ITelemetryScopeProvider telemetryScopeProvider)
			: base(userAuthorityValidator, unitOfWork, userPrincipal)
		{
			if(repositoryTasks == null) throw new ArgumentNullException(nameof(repositoryTasks));
			if(userPrincipal == null) throw new ArgumentNullException(nameof(userPrincipal));
			if(timeService == null) throw new ArgumentNullException(nameof(timeService));
			if(unitOfWork == null) throw new ArgumentNullException(nameof(unitOfWork));
			if(eventProvider == null) throw new ArgumentNullException(nameof(eventProvider));
			if(telemetryScopeProvider == null) throw new ArgumentNullException(nameof(telemetryScopeProvider));

			_repositoryTasks = repositoryTasks;
			_userPrincipal = userPrincipal;
			_timeService = timeService;
			_eventProvider = eventProvider;
			_telemetryScopeProvider = telemetryScopeProvider;
		}

		protected override string RequestedAuthorityName => Authorities.UI.Project.Tasks.StopTaskExecuting;

		protected override long? GetProjectIdForCommand(StopTaskCommand command) =>
			_repositoryTasks.Query()
							.Where(_ => _.Id == command.TaskId)
							.Select(_ => _.ProjectId)
							.Single();

		protected override void ProcessAuthorized(StopTaskCommand command)
		{
			using(var telemetryScope = _telemetryScopeProvider.Create<Tasks>(TelemetryOperationNames.Task.Stop))
			{
				try
				{
					var task = _repositoryTasks.GetById(command.TaskId);

					telemetryScope.SetEntity(task);

					task.Modified = _timeService.GetUtc();
					task.ModifiedById = _userPrincipal.Info.Id;
					task.SdlPolicyStatus = SdlPolicyStatus.Unknown;

					task.Cancel();

					_eventProvider.Publish(
						new Event
						{
							Key = EventKeys.ScanTask.Finished,
							Data = new Dictionary<string, string>
									{
									{Variables.TaskId, task.Id.ToString()},
									{Variables.ProjectId, task.ProjectId.ToString()}
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