namespace Workflow.Event
{
	using System;
	using System.Linq;

	using JetBrains.Annotations;

	using Infrastructure.Events;
	using Repository.Repositories;

	internal sealed class EventProcessor : IEventProcessor
	{
		private readonly IEventHandlerDispatcher _eventHandlerDispatcher;

		private readonly IEventProvider _eventProvider;

		private readonly IEventRepository _eventRepository;

		public EventProcessor(
			[NotNull] IEventHandlerDispatcher eventHandlerDispatcher,
			[NotNull] IEventProvider eventProvider,
			[NotNull] IEventRepository eventRepository)
		{
			if (eventHandlerDispatcher == null) throw new ArgumentNullException(nameof(eventHandlerDispatcher));
			if (eventProvider == null) throw new ArgumentNullException(nameof(eventProvider));
			if (eventRepository == null) throw new ArgumentNullException(nameof(eventRepository));

			_eventHandlerDispatcher = eventHandlerDispatcher;
			_eventProvider = eventProvider;
			_eventRepository = eventRepository;
		}

		/// <summary>
		///   Processes the next event.
		/// </summary>
		/// <returns><see langword="true"/> if there are more events to process; otherwise, <see langword="false"/>.</returns>
		public bool ProcessNextEvent()
		{
		var eventToProcess = _eventProvider.GetNext();

			if (eventToProcess == null)
				return false;

			var eventDb = _eventRepository.GetByKey(eventToProcess.Key).SingleOrDefault();

			if (eventDb == null)
				throw new UnknownEventException();

			var handlers = _eventHandlerDispatcher.Get(eventToProcess);

			foreach(var eventHandler in handlers)
			{
				eventHandler.Handle(eventToProcess);
			}

			return true;
		}
	}
}