namespace Workflow.Event
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using JetBrains.Annotations;

	using Microsoft.Practices.Unity;

	using Common.Logging;
	using Infrastructure.Events;

	[UsedImplicitly]
	internal sealed class EventHandlerDispatcher: IEventHandlerDispatcher
	{
		private readonly IUnityContainer _unityContainer;

		public EventHandlerDispatcher([NotNull] IUnityContainer unityContainer)
		{
			if(unityContainer == null) throw new ArgumentNullException(nameof(unityContainer));

			_unityContainer = unityContainer;
		}

		/// <summary>
		///   Gets handlers for the specified event.
		/// </summary>
		/// <param name="eventToHandle">The event to handle.</param>
		/// <returns>Event handlers.</returns>
		[LogMethod(LogInputParameters = true)]
		public IEnumerable<IEventHandler> Get([NotNull] Event eventToHandle)
		{
			if(eventToHandle == null) throw new ArgumentNullException(nameof(eventToHandle));

			return _unityContainer.ResolveAll<IEventHandler>().Where(x => x.CanHandle(eventToHandle)).ToArray();
		}
	}
}