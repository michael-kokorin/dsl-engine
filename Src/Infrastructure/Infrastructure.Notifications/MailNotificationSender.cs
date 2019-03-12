namespace Infrastructure.Notifications
{
	using System;
	using System.IO;
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Extensions;
	using Common.Logging;
	using Infrastructure.Mail;

	internal sealed class MailNotificationSender : INotificationSender
	{
		private readonly ILog _log;

		private readonly IMailProvider _mailProvider;

		public MailNotificationSender(
			[NotNull] ILog log,
			[NotNull] IMailProvider mailProvider)
		{
			if (log == null) throw new ArgumentNullException(nameof(log));
			if (mailProvider == null) throw new ArgumentNullException(nameof(mailProvider));

			_log = log;
			_mailProvider = mailProvider;
		}

		public void Send([NotNull] Notification notification)
		{
			if (notification == null) throw new ArgumentNullException(nameof(notification));

			const bool isHtml = true;

			using (var client = _mailProvider.BeginSend())
			{
				var mail = new Email
				{
					Attachments = notification
						.Attachments
						?.Select(_ => new Attachment
						{
							Content = new MemoryStream(_.Content),
							Title = _.Title
						}),
					Body = notification.Message,
					IsHtml = isHtml,
					To = notification.Targets,
					Subject = notification.Title
				};

				client.Send(mail);
			}

			_log.Trace(Properties.Resources.SmptMessageHasBeenSent
				.FormatWith(
					notification.Title,
					notification.Targets.ToCommaSeparatedString(),
					isHtml));
		}
	}
}