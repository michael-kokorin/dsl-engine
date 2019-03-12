namespace Workflow.Event
{
	using JetBrains.Annotations;

	using Infrastructure.Events;

	/// <summary>
	///     Provides methods to handle event.
	/// </summary>
	internal interface IEventHandler
	{
		/// <summary>
		///     Determines whether this instance can handle the specified event to handle.
		/// </summary>
		/// <param name="eventToHandle">The event to handle.</param>
		/// <returns><see langword="true"/> if this instance can handle the event; otherwise, <see langword="false"/>.</returns>
		bool CanHandle([NotNull] Event eventToHandle);

		/// <summary>
		///     Handles the specified event to handle.
		/// </summary>
		/// <param name="eventToHandle">The event to handle.</param>
		void Handle([NotNull] Event eventToHandle);
	}
}