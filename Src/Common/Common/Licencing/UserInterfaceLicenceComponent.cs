namespace Common.Licencing
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;

	using JetBrains.Annotations;

	public sealed class UserInterfaceLicenceComponent : ILicenceComponent
	{
		private readonly IReadOnlyDictionary<string, string> _capabilities;

		public IDictionary<string, string> GetCapabilities() => (IDictionary<string, string>) _capabilities;

		public UserInterfaceLicenceComponent([NotNull] IDictionary<string, string> capabilities)
		{
			if (capabilities == null) throw new ArgumentNullException(nameof(capabilities));

			_capabilities = new ConcurrentDictionary<string, string>(capabilities);
		}

		public string Get(string capabilityKey) =>
			_capabilities.ContainsKey(capabilityKey)
				? _capabilities[capabilityKey]
				: null;
	}
}