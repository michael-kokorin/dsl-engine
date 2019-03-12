namespace Infrastructure.Notifications.Tests
{
	using FluentAssertions;

	using Moq;

	using NUnit.Framework;

	using Common.Enums;
	using Common.Extensions;
	using Common.Logging;
	using Common.Validation;
	using Infrastructure.MessageQueue;
	using Infrastructure.Notifications;

	[TestFixture]
	public sealed class NotificationProviderTest
	{
		private INotificationProvider _target;

		private Mock<IMessageQueue> _messageQueue;

		private Mock<ILog> _log;

		private Mock<IQueueReader> _queueReader;

		private Mock<IQueueWriter> _queueWriter;

		private Mock<IValidator<Notification>> _notificationValidator;

		private Notification _notification;

		private string SerializedNotification => _notification.ToXml();

		[SetUp]
		public void SetUp()
		{
			_notification = new Notification
			{
				Protocol = NotificationProtocolType.Email,
				Title = "Test notification"
			};

			_queueReader = new Mock<IQueueReader>();
			_queueReader.Setup(_ => _.Read()).Returns(SerializedNotification);

			_queueWriter = new Mock<IQueueWriter>();

			_messageQueue = new Mock<IMessageQueue>();
			_log = new Mock<ILog>();

			_messageQueue.Setup(_ => _.BeginRead(It.IsAny<MessageQueueKeys>())).Returns(_queueReader.Object);
			_messageQueue.Setup(_ => _.BeginWrite(It.IsAny<MessageQueueKeys>())).Returns(_queueWriter.Object);

			_notificationValidator = new Mock<IValidator<Notification>>();

			_target = new NotificationProvider(_log.Object, _messageQueue.Object, _notificationValidator.Object);
		}

		[Test]
		public void ShouldNotReturnNotification()
		{
			_queueReader.Setup(_ => _.Read()).Returns((string)null);

			var result = _target.GetNext();

			result.Should().BeNull();
		}

		[Test]
		public void ShouldReturnNextNotification()
		{
			var result = _target.GetNext();

			result.ToXml().Should().BeEquivalentTo(SerializedNotification);
		}

		[Test]
		public void ShouldAddNewNotificationToQueue()
		{
			_target.Publish(_notification);

			_notificationValidator.Verify(_ => _.Validate(_notification), Times.Once);
			_messageQueue.Verify(_ => _.BeginWrite(MessageQueueKeys.Notifications), Times.Once);
			_queueWriter.Verify(_ => _.Send(SerializedNotification), Times.Once);
		}
	}
}