namespace Infrastructure.Mail
{
	using System;

	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class DatabaseMailConnectionParametersProvider : IMailConnectionParametersProvider
	{
		private readonly IConfigurationProvider _configurationProvider;

		public DatabaseMailConnectionParametersProvider([NotNull] IConfigurationProvider configurationProvider)
		{
			if (configurationProvider == null) throw new ArgumentNullException(nameof(configurationProvider));

			_configurationProvider = configurationProvider;
		}

		public MailConnectionParameters Get()
		{
			var mailConnectionParameters = new MailConnectionParameters
			{
				Host = GetConfig(ConfigurationKeys.MailOutbox.Host),
				Mailbox = GetConfig(ConfigurationKeys.MailOutbox.MailBox),
				Password = GetConfig(ConfigurationKeys.MailOutbox.Password),
				IsSslEnabled = Convert.ToBoolean(GetConfig(ConfigurationKeys.MailOutbox.SslEnabled)),
				Port = Convert.ToInt32(GetConfig(ConfigurationKeys.MailOutbox.Port)),
				Username = GetConfig(ConfigurationKeys.MailOutbox.User)
			};

			return mailConnectionParameters;
		}

		public MailConnectionParameters TryGet()
		{
			var mailConnectionParameters = new MailConnectionParameters
			{
				Host = TryGetConfig(ConfigurationKeys.MailOutbox.Host, "contoso.com"),
				Mailbox = TryGetConfig(ConfigurationKeys.MailOutbox.MailBox, "mail@contoso.com"),
				Password = TryGetConfig(ConfigurationKeys.MailOutbox.Password, "p@$$w0rd"),
				IsSslEnabled = Convert.ToBoolean(TryGetConfig(ConfigurationKeys.MailOutbox.SslEnabled, "false")),
				Port = Convert.ToInt32(TryGetConfig(ConfigurationKeys.MailOutbox.Port, "25")),
				Username = TryGetConfig(ConfigurationKeys.MailOutbox.User, "mail")
			};

			return mailConnectionParameters;
		}

		public void Set(MailConnectionParameters parameters)
		{
			if (parameters == null)
				throw new ArgumentNullException(nameof(parameters));

			_configurationProvider.SetValue(ConfigurationKeys.MailOutbox.Host, parameters.Host);
			_configurationProvider.SetValue(ConfigurationKeys.MailOutbox.MailBox, parameters.Mailbox);
			_configurationProvider.SetValue(ConfigurationKeys.MailOutbox.Password, parameters.Password);
			_configurationProvider.SetValue(ConfigurationKeys.MailOutbox.SslEnabled, parameters.IsSslEnabled.ToString());
			_configurationProvider.SetValue(ConfigurationKeys.MailOutbox.Port, parameters.Port.ToString());
			_configurationProvider.SetValue(ConfigurationKeys.MailOutbox.User, parameters.Username);
		}

		private string GetConfig(string key)
		{
			var value = _configurationProvider.GetValue(key);

			if (!string.IsNullOrEmpty(value)) return value;

			throw new MailSettingIsNotDefinedException();
		}

		private string TryGetConfig(string key, string defaultValue)
		{
			try
			{
				return GetConfig(key);
			}
			catch (MailSettingIsNotDefinedException)
			{
				_configurationProvider.SetValue(key, defaultValue);

				return defaultValue;
			}
		}
	}
}