namespace Modules.SA.Config
{
	using System;

	using JetBrains.Annotations;

	using Common.Logging;
	using Common.Settings;

	internal sealed class ConfigScanAgentIdProvider: IScanAgentIdProvider
	{
		private readonly IConfigManager _configManager;

		private readonly ILog _log;

		private readonly IScanAgentIdGenerator _scanAgentIdGenerator;

		public ConfigScanAgentIdProvider(
			[NotNull] IConfigManager configManager,
			[NotNull] ILog log,
			[NotNull] IScanAgentIdGenerator scanAgentIdGenerator)
		{
			if (configManager == null)
				throw new ArgumentNullException(nameof(configManager));
			if (log == null) throw new ArgumentNullException(nameof(log));
			if (scanAgentIdGenerator == null) throw new ArgumentNullException(nameof(scanAgentIdGenerator));

			_configManager = configManager;
			_log = log;
			_scanAgentIdGenerator = scanAgentIdGenerator;
		}

		public string Get()
		{
			var agentId = _configManager.Get("AgentId");

			// ReSharper disable once InvertIf
			if (string.IsNullOrEmpty(agentId))
			{
				agentId = _scanAgentIdGenerator.Generate();

				_configManager.Set("AgentId", agentId);

				_log.Info($"New Agent Id generated. Id='{agentId}'");
			}

			return agentId;
		}
	}
}