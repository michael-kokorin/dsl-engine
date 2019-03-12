namespace Infrastructure.Scheduler
{
	using System.Collections.Generic;

	/// <summary>
	///   Provides methods to load jobs.
	/// </summary>
	internal interface ISchedulerJobTypesLoader
	{
		/// <summary>
		///   Loads all jobs.
		/// </summary>
		/// <returns>Loaded jobs.</returns>
		IEnumerable<ScheduledJob> Load();
	}
}