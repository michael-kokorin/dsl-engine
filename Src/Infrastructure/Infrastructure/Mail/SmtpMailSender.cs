namespace Infrastructure.Mail
{
	using System;
	using System.Net.Mail;
	using System.Text;

	using JetBrains.Annotations;

	internal sealed class SmtpMailSender : IMailSender
	{
		private readonly SmtpClient _client;

		private readonly string _from;

		public SmtpMailSender(SmtpClient client, string from)
		{
			if (client == null) throw new ArgumentNullException(nameof(client));
			if (@from == null) throw new ArgumentNullException(nameof(@from));

			_client = client;
			_from = from;
		}

		~SmtpMailSender()
		{
			Dispose(false);
		}

		private bool _disposed;

		public void Dispose() => Dispose(true);

		private void Dispose(bool disposing)
		{
			if (_disposed)
				return;

			if (disposing)
			{
				_client.Dispose();
			}

			_disposed = true;
		}

		public void Send([NotNull] Email mail)
		{
			if (mail == null) throw new ArgumentNullException(nameof(mail));

			using (var message = new MailMessage
			{
				From = new MailAddress(_from),
				Subject = mail.Subject,
				Body = mail.Body,
				BodyEncoding = Encoding.UTF8,
				IsBodyHtml = mail.IsHtml
			})
			{
				if (mail.To != null)
				{
					foreach (var recipient in mail.To)
					{
						message.To.Add(recipient);
					}
				}

				if (mail.Attachments != null)
				{
					foreach (var attachment in mail.Attachments)
					{
						message.Attachments.Add(new System.Net.Mail.Attachment(attachment.Content, attachment.Title));
					}
				}

				_client.Send(message);
			}
		}
	}
}