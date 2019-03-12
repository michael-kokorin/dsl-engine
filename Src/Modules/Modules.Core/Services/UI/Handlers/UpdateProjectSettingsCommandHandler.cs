namespace Modules.Core.Services.UI.Handlers
{
	using System;

	using JetBrains.Annotations;

	using Common.Extensions;
	using Common.Security;
	using Common.Time;
	using Common.Transaction;
	using Infrastructure.Telemetry;
	using Modules.Core.Services.UI.Commands;
	using Repository;
	using Repository.Context;
	using Workflow.VersionControl;

	using Quartz;

	using Authorities = Common.Security.Authorities;

	[UsedImplicitly]
	internal sealed class UpdateProjectSettingsCommandHandler: ProjectCommandHandler<UpdateProjectSettingsCommand>
	{
		private readonly ITelemetryScopeProvider _telemetryScopeProvider;

		public UpdateProjectSettingsCommandHandler(
			IUserAuthorityValidator userAuthorityValidator,
			IWriteRepository<Projects> repositoryProjects,
			ITimeService timeService,
			IUnitOfWork unitOfWork,
			IUserPrincipal userPrincipal,
			[NotNull] ITelemetryScopeProvider telemetryScopeProvider)
			: base(
				userAuthorityValidator,
				repositoryProjects,
				timeService,
				unitOfWork,
				userPrincipal)
		{
			if(telemetryScopeProvider == null) throw new ArgumentNullException(nameof(telemetryScopeProvider));

			_telemetryScopeProvider = telemetryScopeProvider;
		}

		protected override string RequestedAuthorityName => Authorities.UI.Project.Settings.Edit;

		protected override long? GetProjectIdForCommand(UpdateProjectSettingsCommand command) => command.ProjectId;

		protected override void UpdateProject(Projects project, UpdateProjectSettingsCommand command)
		{
			using(var telemetryScope = _telemetryScopeProvider.Create<Projects>(TelemetryOperationNames.Prroject.Update))
			{
				try
				{
					telemetryScope.SetEntity(project);

					project.Alias = command.Alias;
					project.CommitToIt = command.CommitToIt;
					project.CommitToVcs = command.CommitToVcs;
					project.DefaultBranchName = command.DefaultBranchName;
					project.Description = command.Description;
					project.DisplayName = command.DisplayName;
					project.VcsSyncEnabled = command.VcsSyncEnabled;

					if (project.EnablePoll && !command.EnablePoll)
					{
						Global.JobScheduler.DeleteJob("PollJob.Project_{0}".FormatWith(project.Id));
					}

					if (!project.EnablePoll && command.EnablePoll && command.PollTimeout.HasValue)
					{
						ScheduleJob(project.Id, command.PollTimeout.Value);
					}

					if (project.EnablePoll && command.EnablePoll && command.PollTimeout.HasValue &&
						(project.PollTimeout != command.PollTimeout))
					{
						Global.JobScheduler.DeleteJob("PollJob.Project_{0}".FormatWith(project.Id));

						ScheduleJob(project.Id, command.PollTimeout.Value);
					}

					project.EnablePoll = command.EnablePoll;
					project.PollTimeout = command.PollTimeout;

					telemetryScope.WriteSuccess();
				}
				catch(Exception ex)
				{
					telemetryScope.WriteException(ex);

					throw;
				}
			}
		}

		private static void ScheduleJob(long projectId, int timeout)
		{
			var scheduledJob = JobBuilder
				.Create(typeof(VcsPollJob))
				.WithIdentity("PollJob.Project_{0}".FormatWith(projectId))
				.UsingJobData("ProjectId", projectId)
				.Build();

			var trigger = TriggerBuilder
				.Create()
				.WithIdentity("PollTrigger.Project_{0}".FormatWith(projectId))
				.StartAt(SystemTime.UtcNow().AddSeconds(20))
				.WithSimpleSchedule(x => x.WithInterval(new TimeSpan(0, 0, timeout)).RepeatForever())
				.Build();

			Global.JobScheduler.StartJob(scheduledJob, trigger);
		}
	}
}