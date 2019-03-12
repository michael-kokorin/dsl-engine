namespace Workflow.Extensions
{
	using Microsoft.Practices.Unity;

	using Common.Container;
	using Common.Extensions;
	using Workflow.Event;

	/// <summary>
	///   Provides extension methods for container.
	/// </summary>
	internal static class ContainerExtension
	{
		/// <summary>
		///   Registers the event.
		/// </summary>
		/// <typeparam name="TEventHandler">The type of the event handler.</typeparam>
		/// <param name="container">The container.</param>
		/// <param name="reuseScope">The reuse scope.</param>
		/// <returns>The container.</returns>
		public static IUnityContainer RegisterEvent<TEventHandler>(
			this IUnityContainer container,
			ReuseScope reuseScope)
			where TEventHandler: class, IEventHandler
			=> container.RegisterType<IEventHandler, TEventHandler>(typeof(TEventHandler).FullName, reuseScope);
	}
}