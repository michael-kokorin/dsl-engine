namespace Workflow.SA
{
	using Microsoft.Practices.Unity;

	using Common.Container;
	using Common.Extensions;
	using Infrastructure.Scheduler;

	/// <summary>
	///     Represents container module for the project.
	/// </summary>
	/// <seealso cref="Common.Container.IContainerModule"/>
	public sealed class ScanWorkflowModule: IContainerModule
	{
		/// <summary>
		///     Registers binding to the specified container.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <param name="reuseScope">The reuse scope.</param>
		/// <returns>The container.</returns>
		public IUnityContainer Register(IUnityContainer container, ReuseScope reuseScope) =>
			container
				.RegisterType<ScheduledJob, PreScanTaskJob>(typeof(PreScanTaskJob).FullName, reuseScope)
				.RegisterType<ScheduledJob, PostScanTask>(typeof(PostScanTask).FullName, reuseScope);
	}
}