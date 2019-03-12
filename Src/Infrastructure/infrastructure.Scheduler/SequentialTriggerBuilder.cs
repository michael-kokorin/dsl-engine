namespace Infrastructure.Scheduler
{
	using System;

	using JetBrains.Annotations;

	using Quartz;

	[UsedImplicitly]
	internal sealed class SequentialTriggerBuilder: ITriggerBuilder
	{
		private const int DefaultJobsInterval = 10;

		/// <summary>
		///     Creates the trigger.
		/// </summary>
		/// <param name="job">The job.</param>
		/// <param name="jobIndex">Index of the job.</param>
		/// <returns>
		///     The trigger.
		/// </returns>
		/// <exception cref="System.ArgumentNullException"><paramref name="job"/> is <see langword="null"/>.</exception>
		public ITrigger CreateTrigger(
			[NotNull] ScheduledJob job,
			int jobIndex)
		{
			if(job == null)
			{
				throw new ArgumentNullException(nameof(job));
			}

			var jobType = job.GetType();

			var trigger = TriggerBuilder
				.Create()
				.WithIdentity("Trigger." + jobType.Name)
				.StartAt(SystemTime.UtcNow().AddSeconds(DefaultJobsInterval * jobIndex))
				.WithSimpleSchedule(x => x.WithInterval(job.Interval).RepeatForever())
				.Build();

			return trigger;
		}
	}
}