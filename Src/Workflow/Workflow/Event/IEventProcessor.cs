namespace Workflow.Event
{
	/// <summary>
	///   Provides methods to process events.
	/// </summary>
	public interface IEventProcessor
	{
		/// <summary>
		///   Processes the next event.
		/// </summary>
		/// <returns><see langword="true"/> if there are more events to process; otherwise, <see langword="false"/>.</returns>
		bool ProcessNextEvent();
	}
}