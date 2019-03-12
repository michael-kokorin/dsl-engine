namespace Workflow.VersionControl
{
	using System;
	using System.Collections.Generic;

	using JetBrains.Annotations;

	using Infrastructure;
	using Infrastructure.Events;
	using Infrastructure.Scheduler;
	using Repository.Repositories;

	using Quartz;

	/// <summary>
	///     Represents a job to poll vcs by the custom schedule.
	/// </summary>
	/// <seealso cref="Infrastructure.Scheduler.ScheduledJob"/>
	/// <seealso cref="Infrastructure.Scheduler.ICustomScheduledJob"/>
	[DisallowConcurrentExecution]
	public sealed class VcsPollJob: ScheduledJob, ICustomScheduledJob
	{
		private readonly IEventProvider _eventProvider;

		private readonly IProjectRepository _projectRepository;

		/// <summary>
		///     Initializes a new instance of the <see cref="VcsPollJob"/> class.
		/// </summary>
		/// <param name="eventProvider">The event provider.</param>
		/// <param name="projectRepository">The project repository.</param>
		/// <exception cref="ArgumentNullException">
		///     <paramref name="eventProvider"/> or <paramref name="projectRepository"/> is
		///     <see langword="null"/>.
		/// </exception>
		[UsedImplicitly]
		public VcsPollJob([NotNull] IEventProvider eventProvider, [NotNull] IProjectRepository projectRepository)
		{
			if(eventProvider == null)
			{
				throw new ArgumentNullException(nameof(eventProvider));
			}
			if(projectRepository == null)
			{
				throw new ArgumentNullException(nameof(projectRepository));
			}

			_eventProvider = eventProvider;
			_projectRepository = projectRepository;
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
			var projectId = (long)JobExecutionContext.MergedJobDataMap.Get("ProjectId");
			var project = _projectRepository.GetById(projectId);

			SendEvent(projectId, project.DefaultBranchName);

			return 0;
		}

		private void SendEvent(long projectId, string branchName) =>
			_eventProvider.Publish(
				new Event
				{
					Key = EventKeys.VcsCommitted,
					Data = new Dictionary<string, string>
							{
								{Variables.ProjectId, projectId.ToString()},
								{Variables.Branch, branchName}
							}
				});
	}
}