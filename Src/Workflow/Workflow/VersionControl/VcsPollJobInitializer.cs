namespace Workflow.VersionControl
{
	using System;
	using System.Linq;

	using Common.Extensions;
	using Common.Logging;
	using Infrastructure.Events;
	using Infrastructure.Scheduler;
	using Repository.Repositories;

	using Quartz;

	/// <summary>
	///     Provides methods to schedule <see cref="VcsPollJob"/>.
	/// </summary>
	/// <seealso cref="Infrastructure.Scheduler.ICustomJobInitializer"/>
	internal sealed class VcsPollJobInitializer: ICustomJobInitializer
	{
		private readonly IProjectRepository _projectRepository;

		private readonly ILog _log;

		private readonly IEventProvider _eventProvider;

		/// <summary>
		///     Initializes a new instance of the <see cref="VcsPollJobInitializer"/> class.
		/// </summary>
		/// <param name="projectRepository">The project repository.</param>
		/// <param name="log">The log.</param>
		/// <param name="eventProvider">The event provider.</param>
		public VcsPollJobInitializer(IProjectRepository projectRepository, ILog log, IEventProvider eventProvider)
		{
			_projectRepository = projectRepository;
			_log = log;
			_eventProvider = eventProvider;
		}

		/// <summary>
		///     Initializes the specified scheduler.
		/// </summary>
		/// <param name="scheduler">The scheduler.</param>
		public void Initialize(IScheduler scheduler)
		{
			var index = 0;
			foreach(var project in _projectRepository.Query().Where(_ => _.EnablePoll))
			{
				if(!project.PollTimeout.HasValue)
				{
					_log.Warning("Project {0} is configured to run poll, but poll timeout is not specified".FormatWith(project.Id));
					continue;
				}

				var job = new VcsPollJob(_eventProvider, _projectRepository)
						{
							Interval = new TimeSpan(0, 0, project.PollTimeout.Value)
						};

				var scheduledJob = JobBuilder
					.Create(job.GetType())
					.WithIdentity("PollJob.Project_{0}".FormatWith(project.Id))
					.UsingJobData("ProjectId", project.Id)
					.Build();

				var trigger = TriggerBuilder
					.Create()
					.WithIdentity("PollTrigger.Project_{0}".FormatWith(project.Id))
					.StartAt(SystemTime.UtcNow().AddSeconds(20 * index))
					.WithSimpleSchedule(x => x.WithInterval(job.Interval).RepeatForever())
					.Build();

				scheduler.ScheduleJob(scheduledJob, trigger);
				index++;
			}
		}
	}
}