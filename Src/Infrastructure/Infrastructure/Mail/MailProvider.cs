namespace Infrastructure.Mail
{
	using System;

	using JetBrains.Annotations;

	internal sealed class MailProvider : IMailProvider
	{
		private readonly IMailConnectionParametersProvider _connectionParametersProvider;

		private readonly IMailClient _mailClient;

		public MailProvider(
			[NotNull] IMailClient mailClient,
			[NotNull] IMailConnectionParametersProvider connectionParametersProvider)
		{
			if (mailClient == null) throw new ArgumentNullException(nameof(mailClient));
			if (connectionParametersProvider == null) throw new ArgumentNullException(nameof(connectionParametersProvider));

			_mailClient = mailClient;
			_connectionParametersProvider = connectionParametersProvider;
		}

		public IMailSender BeginSend()
		{
			var parameters = _connectionParametersProvider.Get();

			if (parameters == null)
				throw new IncorrectMailSettingsException();

			return _mailClient.BeginSend(parameters);
		}
	}
}