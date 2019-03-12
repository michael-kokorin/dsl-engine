namespace Workflow.Event
{
	using System;

	using JetBrains.Annotations;

	using Infrastructure.Scheduler;

	using Quartz;

	/// <summary>
	///   Job that processes events.
	/// </summary>
	/// <seealso cref="Infrastructure.Scheduler.ScheduledJob"/>
	[UsedImplicitly]
	[DisallowConcurrentExecution]
	public sealed class EventProcessingJob: ScheduledJob
	{
		private readonly IEventProcessor _eventProcessor;

		/// <summary>
		///   Initializes a new instance of the <see cref="EventProcessingJob"/> class.
		/// </summary>
		/// <param name="eventProcessor">The event processor.</param>
		public EventProcessingJob([NotNull] IEventProcessor eventProcessor)
		{
			_eventProcessor = eventProcessor;
		}

		/// <summary>
		///   Executes the job.
		/// </summary>
		/// <returns>
		///   Positive value to repeat the task, negative value or 0 - to finish the task and to wait time interval before the
		///   next run.
		/// </returns>
		protected override int Process() => Convert.ToInt32(_eventProcessor.ProcessNextEvent());
	}
}