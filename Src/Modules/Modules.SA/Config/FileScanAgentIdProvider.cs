namespace Modules.SA.Config
{
	using System;

	using JetBrains.Annotations;

	using Common.Logging;

	internal sealed class FileScanAgentIdProvider: IScanAgentIdProvider
	{
		private readonly ILog _log;

		private readonly IScanAgentIdFilePathProvider _agentIdFilePathProvider;

		private readonly IScanAgentIdGenerator _scanAgentIdGenerator;

		public FileScanAgentIdProvider(
			[NotNull] ILog log,
			[NotNull] IScanAgentIdFilePathProvider agentIdFilePathProvider,
			[NotNull] IScanAgentIdGenerator scanAgentIdGenerator)
		{
			if (log == null) throw new ArgumentNullException(nameof(log));
			if (agentIdFilePathProvider == null) throw new ArgumentNullException(nameof(agentIdFilePathProvider));
			if (scanAgentIdGenerator == null) throw new ArgumentNullException(nameof(scanAgentIdGenerator));

			_log = log;
			_agentIdFilePathProvider = agentIdFilePathProvider;
			_scanAgentIdGenerator = scanAgentIdGenerator;
		}

		public string Get()
		{
			var filePath = _agentIdFilePathProvider.GetIdFilePath();

			string agentId;

			// ReSharper disable once InvertIf
			if (!TextFile.TryRead(filePath, out agentId) || string.IsNullOrEmpty(agentId))
			{
				agentId = _scanAgentIdGenerator.Generate();

				TextFile.Write(filePath, agentId);

				_log.Info($"New Agent Id saved. Id='{agentId}', IdFilePath='{filePath}'");
			}

			return agentId;
		}
	}
}