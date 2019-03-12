namespace Infrastructure.Scheduler
{
	using Microsoft.Practices.Unity;

	using Common.Container;
	using Common.Extensions;

	using Quartz.Spi;

	/// <summary>
	///   Represents container module for the project.
	/// </summary>
	/// <seealso cref="Common.Container.IContainerModule"/>
	public sealed class SchedulerContainerModule: IContainerModule
	{
		/// <summary>
		///   Registers binding to the specified container.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <param name="reuseScope">The reuse scope.</param>
		/// <returns>The container.</returns>
		public IUnityContainer Register(IUnityContainer container, ReuseScope reuseScope) =>
			container
				.RegisterType<IJobFactory, ContainerJobFactory>(reuseScope)
				.RegisterType<ITriggerBuilder, SequentialTriggerBuilder>(reuseScope)
				.RegisterType<ISchedulerJobTypesLoader, SchedulerJobTypesLoader>(reuseScope)
				.RegisterType<IJobNameBuilder, JobNameBuilder>(reuseScope)
				.RegisterType<IJobScheduler, JobScheduler>(ReuseScope.Container);
	}
}