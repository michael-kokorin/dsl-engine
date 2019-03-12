namespace Infrastructure.Scheduler
{
	using Quartz;

	/// <summary>
	///     Provides methods to initialize a custom job.
	/// </summary>
	public interface ICustomJobInitializer
	{
		/// <summary>
		///     Initializes the specified scheduler.
		/// </summary>
		/// <param name="scheduler">The scheduler.</param>
		void Initialize(IScheduler scheduler);
	}
}