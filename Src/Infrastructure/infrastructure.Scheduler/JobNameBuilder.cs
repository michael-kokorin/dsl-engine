namespace Infrastructure.Scheduler
{
	using System;

	using JetBrains.Annotations;

	using Common.Extensions;
	using Infrastructure.Scheduler.Properties;

	internal sealed class JobNameBuilder: IJobNameBuilder
	{
		/// <summary>
		///   Gets the name of the job.
		/// </summary>
		/// <param name="job">The job.</param>
		/// <returns>
		///   The name of the job.
		/// </returns>
		/// <exception cref="System.ArgumentNullException"><paramref name="job"/> is <see langword="null"/>.</exception>
		public string GetJobName([NotNull] ScheduledJob job)
		{
			if(job == null) throw new ArgumentNullException(nameof(job));

			return Resources.JobScheduler_Start_Job_Prefix.FormatWith(job.GetType().Name);
		}
	}
}