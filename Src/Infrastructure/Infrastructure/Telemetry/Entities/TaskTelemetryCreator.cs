namespace Infrastructure.Telemetry.Entities
{
	using System;

	using JetBrains.Annotations;

	using Repository.Context;

	[UsedImplicitly]
	internal sealed class TaskTelemetryCreator : IEntityTelemetryCreator<Tasks>
	{
		private readonly ITelemetryQueue _telemetryQueue;

		public TaskTelemetryCreator([NotNull] ITelemetryQueue telemetryQueue)
		{
			if(telemetryQueue == null) throw new ArgumentNullException(nameof(telemetryQueue));

			_telemetryQueue = telemetryQueue;
		}

		public void Save(string operationName,
			long? operationDuration,
			TelemetryOperationStatus telemetryOperationStatus,
			int? hResult,
			Tasks sourceEntity)
		{
			var telemetry = new TaskTelemetry
			{
				AnalyzedSize = sourceEntity?.AnalyzedSize,
				Branch = sourceEntity?.Repository,
				OperationDuration = operationDuration,
				EntityId = sourceEntity?.Id,
				FolderSize = sourceEntity?.FolderSize,
				ItPluginName = sourceEntity?.Projects?.Plugins?.DisplayName,
				VcsPluginName = sourceEntity?.Projects?.Plugins1?.DisplayName,
				OperationHResult = hResult,
				OperationStatus = (int) telemetryOperationStatus,
				OperationName = operationName,
				TaskStatus = sourceEntity?.Status,
				TaskSdlStatus = sourceEntity?.SdlStatus,
				TaskResolution = sourceEntity?.Resolution,
				ScanCoreWorkTime = sourceEntity?.ScanCoreWorkingTime,
				RelatedEntityId = sourceEntity?.ProjectId
			};

			_telemetryQueue.Send(telemetry);
		}
	}
}