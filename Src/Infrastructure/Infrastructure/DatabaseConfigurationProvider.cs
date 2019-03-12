namespace Infrastructure
{
	using System;
	using System.Linq;

	using JetBrains.Annotations;

	using Repository.Context;
	using Repository.Repositories;

	internal sealed class DatabaseConfigurationProvider : IConfigurationProvider
	{
		private readonly IConfigurationRepository _configurationRepository;

		public DatabaseConfigurationProvider([NotNull] IConfigurationRepository configurationRepository)
		{
			if (configurationRepository == null) throw new ArgumentNullException(nameof(configurationRepository));

			_configurationRepository = configurationRepository;
		}

		public string GetValue(string key)
		{
			if (string.IsNullOrEmpty(key))
				throw new ArgumentNullException(nameof(key));

			return _configurationRepository.GetByKey(key).Select(_ => _.Value).SingleOrDefault();
		}

		public void SetValue(string key, string value)
		{
			if (string.IsNullOrEmpty(key))
				throw new ArgumentNullException(nameof(key));

			var config = _configurationRepository.GetByKey(key).SingleOrDefault();

			if (config == null)
			{
				config = new Configuration
				{
					Name = key,
					Value = value
				};

				_configurationRepository.Insert(config);
			}
			else
			{
				config.Value = value;
			}

			_configurationRepository.Save();
		}
	}
}