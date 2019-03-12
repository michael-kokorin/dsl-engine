namespace Infrastructure.Scheduler
{
	using System;

	using Quartz;

	/// <summary>
	///     Provides ability to schedule jobs.
	/// </summary>
	public interface IJobScheduler: IDisposable
	{
		/// <summary>
		///     Start jobs.
		/// </summary>
		void Start(bool telemetryRequired);

		/// <summary>
		///     Stop jobs.
		/// </summary>
		void Stop();

		void DeleteJob(string jobKey);

		void StartJob(IJobDetail job, ITrigger trigger);
	}
}