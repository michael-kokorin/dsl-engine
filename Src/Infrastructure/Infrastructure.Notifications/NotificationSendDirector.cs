namespace Infrastructure.Notifications
{
	using System;

	using JetBrains.Annotations;

	using Common.Extensions;
	using Common.Logging;

	[UsedImplicitly]
	internal sealed class NotificationSendDirector : INotificationSendDirector
	{
		private readonly ILog _log;

		private readonly INotificationSenderProvider _notificationSenderProvider;

		public NotificationSendDirector(
			[NotNull] ILog log, [NotNull]
		INotificationSenderProvider notificationSenderProvider)
		{
			if (log == null) throw new ArgumentNullException(nameof(log));
			if (notificationSenderProvider == null) throw new ArgumentNullException(nameof(notificationSenderProvider));

			_log = log;
			_notificationSenderProvider = notificationSenderProvider;
		}

		public void Send(Notification notification)
		{
			if (notification == null)
				throw new ArgumentNullException(nameof(notification));

			var protocolSender = _notificationSenderProvider.Get(notification.Protocol);

			protocolSender.Send(notification);

			_log.Debug(Properties.Resources.NotificationHasBeenSent.FormatWith(
				notification.Title,
				notification.Protocol,
				notification.Targets.ToCommaSeparatedString()));
		}
	}
}