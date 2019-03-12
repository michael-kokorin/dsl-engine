namespace Infrastructure.Notifications
{
	using System;

	using JetBrains.Annotations;

	using Common.Extensions;
	using Common.Logging;
	using Common.Validation;
	using Infrastructure.MessageQueue;

	internal sealed class NotificationProvider : INotificationProvider
	{
		private const MessageQueueKeys Key = MessageQueueKeys.Notifications;

		private readonly IMessageQueue _messageQueue;

		private readonly ILog _log;

		private readonly IValidator<Notification> _notificationValidator;

		public NotificationProvider(
			[NotNull] ILog log,
			[NotNull] IMessageQueue messageQueue,
			[NotNull] IValidator<Notification> notificationValidator)
		{
			if (log == null) throw new ArgumentNullException(nameof(log));
			if (messageQueue == null) throw new ArgumentNullException(nameof(messageQueue));
			if (notificationValidator == null) throw new ArgumentNullException(nameof(notificationValidator));

			_log = log;
			_messageQueue = messageQueue;
			_notificationValidator = notificationValidator;
		}

		public void Publish(Notification notification)
		{
			_notificationValidator.Validate(notification);

			using (var writeAction = _messageQueue.BeginWrite(Key))
			{
				var serializedNotification = notification.ToXml();

				writeAction.Send(serializedNotification);

				_log.Debug(Properties.Resources.NotificationHasBeenPublished.FormatWith(
					notification.Title,
					notification.Protocol,
					notification.Targets.ToCommaSeparatedString()));
			}
		}

		public Notification GetNext()
		{
			using (var readAction = _messageQueue.BeginRead(Key))
			{
				var notificationMessage = readAction.Read();

				if (string.IsNullOrEmpty(notificationMessage))
				{
					_log.Debug(Properties.Resources.ThereIsNoOneUnprocessedNotificationInQueue);

					return null;
				}

				var nextNotification = notificationMessage.FromXml<Notification>();

				_log.Debug(Properties.Resources.NotificationHasBeenReadFromQueue.FormatWith(
					nextNotification.Title,
					nextNotification.Protocol,
					nextNotification.Targets.ToCommaSeparatedString()));

				return nextNotification;
			}
		}
	}
}