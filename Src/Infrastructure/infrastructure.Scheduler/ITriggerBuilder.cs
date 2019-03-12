namespace Infrastructure.Scheduler
{
	using Quartz;

	/// <summary>
	///   Provides methods to build triggers for jobs.
	/// </summary>
	internal interface ITriggerBuilder
	{
		/// <summary>
		///   Creates the trigger.
		/// </summary>
		/// <param name="job">The job.</param>
		/// <param name="jobIndex">Index of the job.</param>
		/// <returns>The trigger.</returns>
		ITrigger CreateTrigger(ScheduledJob job, int jobIndex);
	}
}