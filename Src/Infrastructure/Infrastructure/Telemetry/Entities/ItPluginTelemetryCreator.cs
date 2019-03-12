namespace Infrastructure.Telemetry.Entities
{
	using System;

	using JetBrains.Annotations;

	using Repository.Context;

	[UsedImplicitly]
	public sealed class ItPluginTelemetryCreator: IEntityTelemetryCreator<ItPluginInfo>
	{
		private readonly ITelemetryQueue _telemetryQueue;

		public ItPluginTelemetryCreator([NotNull] ITelemetryQueue telemetryQueue)
		{
			if(telemetryQueue == null) throw new ArgumentNullException(nameof(telemetryQueue));

			_telemetryQueue = telemetryQueue;
		}

		public void Save(
			string operationName,
			long? operationDuration,
			TelemetryOperationStatus telemetryOperationStatus,
			int? hResult,
			ItPluginInfo sourceEntity)
		{
			var itPluginTelemetry = new ItPluginTelemetry
									{
										EntityId = sourceEntity?.Plugins.Id,
										OperationName = operationName,
										OperationDuration = operationDuration,
										OperationStatus = (int)telemetryOperationStatus,
										OperationHResult = hResult,
										RelatedEntityId = sourceEntity?.TaskId,
										DisplayName = sourceEntity?.Plugins?.DisplayName,
										TypeFullName = sourceEntity?.Plugins?.TypeFullName,
										AssemblyName = sourceEntity?.Plugins?.AssemblyName
									};

			_telemetryQueue.Send(itPluginTelemetry);
		}
	}
}