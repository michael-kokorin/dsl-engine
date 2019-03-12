namespace Infrastructure.Scheduler
{
	using System.Collections.Generic;

	using JetBrains.Annotations;

	using Microsoft.Practices.Unity;

	internal sealed class SchedulerJobTypesLoader: ISchedulerJobTypesLoader
	{
		private readonly IUnityContainer _container;

		public SchedulerJobTypesLoader([NotNull] IUnityContainer unityContainer)
		{
			_container = unityContainer;
		}

		/// <summary>
		///   Loads all jobs.
		/// </summary>
		/// <returns>Loaded jobs.</returns>
		public IEnumerable<ScheduledJob> Load() =>
			_container.ResolveAll<ScheduledJob>();
	}
}