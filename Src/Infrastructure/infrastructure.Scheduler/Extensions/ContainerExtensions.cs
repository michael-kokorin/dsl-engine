namespace Infrastructure.Scheduler.Extensions
{
	using Microsoft.Practices.Unity;

	using Common.Container;
	using Common.Extensions;

	/// <summary>
	///   Provides extension methods for IoC container.
	/// </summary>
	public static class ContainerExtensions
	{
		/// <summary>
		///   Registers the job.
		/// </summary>
		/// <typeparam name="T">Type of the job.</typeparam>
		/// <param name="container">The container.</param>
		/// <param name="reuseScope">The reuse scope.</param>
		/// <returns>The container.</returns>
		public static IUnityContainer RegisterJob<T>(this IUnityContainer container, ReuseScope reuseScope)
			where T: ScheduledJob => container.RegisterType<ScheduledJob, T>(typeof(T).FullName, reuseScope);
	}
}