namespace Workflow.Event
{
	using System.Collections.Generic;

	using Infrastructure.Events;

	/// <summary>
	///   Provides methods to get handler for specified event.
	/// </summary>
	internal interface IEventHandlerDispatcher
	{
		/// <summary>
		///   Gets handlers for the specified event.
		/// </summary>
		/// <param name="eventToHandle">The event to handle.</param>
		/// <returns>Event handlers.</returns>
		IEnumerable<IEventHandler> Get(Event eventToHandle);
	}
}