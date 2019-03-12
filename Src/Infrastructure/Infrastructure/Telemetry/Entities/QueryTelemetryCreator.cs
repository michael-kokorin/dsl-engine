namespace Infrastructure.Telemetry.Entities
{
	using System;

	using JetBrains.Annotations;

	using Repository.Context;

	[UsedImplicitly]
	internal sealed class QueryTelemetryCreator : IEntityTelemetryCreator<Queries>
	{
		private readonly ITelemetryQueue _telemetryQueue;

		public QueryTelemetryCreator([NotNull] ITelemetryQueue telemetryQueue)
		{
			if(telemetryQueue == null) throw new ArgumentNullException(nameof(telemetryQueue));

			_telemetryQueue = telemetryQueue;
		}

		public void Save(string operationName,
			long? operationDuration,
			TelemetryOperationStatus telemetryOperationStatus,
			int? hResult,
			Queries entity)
		{
			var queryTelemetry = new QueryTelemetry
			{
				EntityId = entity?.Id,
				OperationName = operationName,
				OperationDuration = operationDuration,
				OperationStatus = (int) telemetryOperationStatus,
				OperationHResult = hResult,
				RelatedEntityId = entity?.ProjectId,
				DisplayName = entity?.Name,
				Privacy = entity?.Privacy,
				IsSystem = entity?.IsSystem,
				Visibility = entity?.Visibility,
				Comment = entity?.Comment
			};

			_telemetryQueue.Send(queryTelemetry);
		}
	}
}