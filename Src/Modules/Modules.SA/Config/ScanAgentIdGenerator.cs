namespace Modules.SA.Config
{
	using System;

	using JetBrains.Annotations;

	using Common.Logging;

	internal sealed class ScanAgentIdGenerator: IScanAgentIdGenerator
	{
		private readonly ILog _log;

		public ScanAgentIdGenerator([NotNull] ILog log)
		{
			if (log == null) throw new ArgumentNullException(nameof(log));

			_log = log;
		}

		public string Generate()
		{
			var agentId = Guid.NewGuid().ToString("N");

			_log.Info($"New Agent Id generaged. Id='{agentId}'");

			return agentId;
		}
	}
}