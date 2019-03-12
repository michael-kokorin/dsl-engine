namespace Infrastructure.Mail
{
	using System;
	using System.Net;
	using System.Net.Mail;

	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class MailClient : IMailClient
	{
		public IMailSender BeginSend([NotNull] MailConnectionParameters parameters)
		{
			if (parameters == null) throw new ArgumentNullException(nameof(parameters));

			var client = new SmtpClient(parameters.Host, parameters.Port)
			{
				Credentials = new NetworkCredential(parameters.Username, parameters.Password),
				EnableSsl = parameters.IsSslEnabled
			};

			return new SmtpMailSender(client, parameters.Mailbox);
		}
	}
}