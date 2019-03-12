namespace Common.Licencing
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Extensions;

	public sealed class PluginLicenceComponent : ILicenceComponent
	{
		private readonly bool _isRestricted;

		private readonly IEnumerable<string> _pluginUids;

		public PluginLicenceComponent(bool isRestricted = false)
		{
			_isRestricted = isRestricted;
		}

		public PluginLicenceComponent([NotNull] IEnumerable<string> pluginUids)
		{
			if (pluginUids == null) throw new ArgumentNullException(nameof(pluginUids));

			_isRestricted = true;

			_pluginUids = pluginUids.ToArray();
		}

		public bool CanUse(string pluginType) =>
			_isRestricted == false || _pluginUids.Any(_ => _.Equals(pluginType, StringComparison.InvariantCulture));

		public IDictionary<string, string> GetCapabilities() =>
			new Dictionary<string, string>
			{
				{"Is restricted", _isRestricted.ToString()},
				{"Plugin types", _pluginUids.ToCommaSeparatedString()}
			};
	}
}