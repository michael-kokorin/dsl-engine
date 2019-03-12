namespace Infrastructure.Events
{
	using System;
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Extensions;
	using Infrastructure.MessageQueue;
	using Repository.Repositories;

	internal sealed class EventProvider : IEventProvider
	{
		private readonly IEventRepository _eventRepository;

		private readonly IMessageQueue _messageQueue;

		public EventProvider([NotNull] IMessageQueue messageQueue,
			[NotNull] IEventRepository eventRepository)
		{
			if (messageQueue == null) throw new ArgumentNullException(nameof(messageQueue));
			if (eventRepository == null) throw new ArgumentNullException(nameof(eventRepository));

			_messageQueue = messageQueue;
			_eventRepository = eventRepository;
		}

		public void Publish(Event eventToPublish)
		{
			if (eventToPublish == null)
				throw new ArgumentNullException(nameof(eventToPublish));

			using (var sendAction = _messageQueue.BeginWrite(MessageQueueKeys.Events))
			{
				var serializedEvent = eventToPublish.ToXml();

				sendAction.Send(serializedEvent);
			}
		}

		public Event GetNext()
		{
			using (var readAction = _messageQueue.BeginRead(MessageQueueKeys.Events))
			{
				var eventMessage = readAction.Read();

				if (string.IsNullOrEmpty(eventMessage)) return null;

				var nextEvent = eventMessage.FromXml<Event>();

				return nextEvent;
			}
		}

		public void RegisterType(string key, string description)
		{
			if (string.IsNullOrEmpty(key))
				throw new ArgumentNullException(nameof(key));

			if (_eventRepository.GetByKey(key).Any())
				return;

			_eventRepository.Insert(
				new Repository.Context.Events
				{
					Key = key,
					Name = description
				});

			_eventRepository.Save();
		}
	}
}