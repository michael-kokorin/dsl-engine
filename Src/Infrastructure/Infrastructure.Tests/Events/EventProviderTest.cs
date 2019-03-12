namespace Infrastructure.Tests.Events
{
	using System.Collections.Generic;

	using FluentAssertions;

	using Moq;

	using NUnit.Framework;

	using Common.Extensions;
	using Infrastructure.Events;
	using Infrastructure.MessageQueue;
	using Repository.Repositories;

	[TestFixture]
	public sealed class EventProviderTest
	{
		private IEventProvider _target;

		private Mock<IMessageQueue> _messageQueue;

		private Mock<IQueueReader> _queueReader;

		private Mock<IQueueWriter> _queueWriter;

		private Mock<IEventRepository> _eventRepository;

		private Event _event;

		private string SerializedEvent => _event?.ToXml();

		[SetUp]
		public void SetUp()
		{
			_queueReader = new Mock<IQueueReader>();

			_queueWriter = new Mock<IQueueWriter>();

			_messageQueue = new Mock<IMessageQueue>();

			_eventRepository = new Mock<IEventRepository>();

			_messageQueue.Setup(_ => _.BeginRead(It.IsAny<MessageQueueKeys>())).Returns(_queueReader.Object);
			_messageQueue.Setup(_ => _.BeginWrite(It.IsAny<MessageQueueKeys>())).Returns(_queueWriter.Object);

			_target = new EventProvider(_messageQueue.Object, _eventRepository.Object);

			_event = new Event
			{
				Key = "event_key",
				Data = new Dictionary<string, string>
				{
					{"1", "2"},
					{"3", "4"}
				}
			};
		}

		[Test]
		public void ShouldSendSerializedEventToQueueWriter()
		{
			_target.Publish(_event);

			_messageQueue.Verify(_ => _.BeginWrite(MessageQueueKeys.Events), Times.Once);
			_queueWriter.Verify(_ => _.Send(SerializedEvent), Times.Once);
			_queueWriter.Verify(_ => _.Dispose(), Times.Once);
		}

		[Test]
		public void ShouldReadNextEventFromQueue()
		{
			_queueReader.Setup(_ => _.Read()).Returns(SerializedEvent);

			var result = _target.GetNext();

			result.ToXml().Should().BeEquivalentTo(SerializedEvent);
			_messageQueue.Verify(_ => _.BeginRead(MessageQueueKeys.Events), Times.Once);
			_queueReader.Verify(_ => _.Read(), Times.Once);
			_queueReader.Verify(_ => _.Dispose(), Times.Once);
		}
	}
}