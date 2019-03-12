namespace Infrastructure.Extensions
{
	using JetBrains.Annotations;

	public static class ConfigurationProviderExtension
	{
		[UsedImplicitly]
		public static string GetValueOrDefault(this IConfigurationProvider configurationProvider,
			string key,
			string defaultValue)
		{
			var value = configurationProvider.GetValue(key);

			return value ?? defaultValue;
		}

		[UsedImplicitly]
		public static string GetOrCreate(this IConfigurationProvider provider, string key, string defaultValue)
		{
			var value = provider.GetValue(key);

			if (value == null)
			{
				provider.SetValue(key, defaultValue);
			}

			return value ?? defaultValue;
		}
	}
}