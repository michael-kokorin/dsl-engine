namespace Infrastructure.Telemetry.Entities
{
	using System;

	using JetBrains.Annotations;

	using Repository.Context;

	[UsedImplicitly]
	internal sealed class ReportTelemetryCreator : IEntityTelemetryCreator<Reports>
	{
		private readonly ITelemetryQueue _telemetryQueue;

		public ReportTelemetryCreator([NotNull] ITelemetryQueue telemetryQueue)
		{
			if(telemetryQueue == null) throw new ArgumentNullException(nameof(telemetryQueue));

			_telemetryQueue = telemetryQueue;
		}

		public void Save(string operationName,
			long? operationDuration,
			TelemetryOperationStatus telemetryOperationStatus,
			int? hResult,
			Reports sourceEntity)
		{
			if (operationName == null) throw new ArgumentNullException(nameof(operationName));

			var reportTelemetry = new ReportTelemetry
			{
				EntityId = sourceEntity?.Id,
				OperationName = operationName,
				OperationStatus = (int) telemetryOperationStatus,
				OperationDuration= operationDuration,
				OperationHResult = hResult,
				RelatedEntityId = sourceEntity?.ProjectId,
				DisplayName = sourceEntity?.DisplayName,
				IsSystem = sourceEntity?.IsSystem
			};

			_telemetryQueue.Send(reportTelemetry);
		}
	}
}