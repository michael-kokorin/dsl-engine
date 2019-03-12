namespace Infrastructure.Telemetry.Entities
{
	using System;

	using JetBrains.Annotations;

	using Repository.Context;

	[UsedImplicitly]
	internal sealed class VcsPluginTelemetryCreator: IEntityTelemetryCreator<VcsPluginInfo>
	{
		private readonly ITelemetryQueue _telemetryQueue;

		public VcsPluginTelemetryCreator([NotNull] ITelemetryQueue telemetryQueue)
		{
			if(telemetryQueue == null) throw new ArgumentNullException(nameof(telemetryQueue));

			_telemetryQueue = telemetryQueue;
		}

		public void Save(
			string operationName,
			long? operationDuration,
			TelemetryOperationStatus telemetryOperationStatus,
			int? hResult,
			VcsPluginInfo sourceEntity)
		{
			var vcsPluginTelemetry = new VcsPluginTelemetry
									{
										AssemblyName = sourceEntity?.Plugin?.AssemblyName,
										CommittedSize = sourceEntity?.CommittedSourcesSize,
										CreatedBranchName = sourceEntity?.CreatedBranchName,
										DisplayName = sourceEntity?.Plugin?.DisplayName,
										DownloadedSourcesSize = sourceEntity?.DownloadedSourcesSize,
										OperationDuration = operationDuration,
										EntityId = sourceEntity?.Plugin?.Id,
										OperationName = operationName,
										OperationStatus = (int) telemetryOperationStatus,
										OperationHResult = hResult,
										TypeFullName = sourceEntity?.Plugin?.TypeFullName,
										RelatedEntityId = sourceEntity?.TaskId
									};

			_telemetryQueue.Send(vcsPluginTelemetry);
		}
	}
}