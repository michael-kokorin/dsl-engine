namespace Infrastructure.Scheduler
{
	/// <summary>
	///   Provides methods to build name for the job.
	/// </summary>
	internal interface IJobNameBuilder
	{
		/// <summary>
		///   Gets the name of the job.
		/// </summary>
		/// <param name="job">The job.</param>
		/// <returns>The name of the job.</returns>
		string GetJobName(ScheduledJob job);
	}
}