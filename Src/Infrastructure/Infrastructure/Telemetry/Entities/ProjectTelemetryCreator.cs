namespace Infrastructure.Telemetry.Entities
{
	using System;

	using JetBrains.Annotations;

	using Repository.Context;

	[UsedImplicitly]
	internal sealed class ProjectTelemetryCreator: IEntityTelemetryCreator<Projects>
	{
		private readonly ITelemetryQueue _telemetryQueue;

		public ProjectTelemetryCreator([NotNull] ITelemetryQueue telemetryQueue)
		{
			if(telemetryQueue == null) throw new ArgumentNullException(nameof(telemetryQueue));

			_telemetryQueue = telemetryQueue;
		}

		public void Save(
			string operationName,
			long? operationDuration,
			TelemetryOperationStatus telemetryOperationStatus,
			int? hResult,
			Projects sourceEntity)
		{
			var projectTelemetry = new ProjectTelemetry
									{
										EntityId = sourceEntity?.Id,
										OperationName = operationName,
										OperationDuration = operationDuration,
										OperationStatus = (int)telemetryOperationStatus,
										ProjectName = sourceEntity?.DisplayName,
										OperationHResult = hResult,
										RelatedEntityId = null,
										CommitToIt = sourceEntity?.CommitToIt,
										CommitToVcs = sourceEntity?.CommitToVcs,
										SyncWithVcs = sourceEntity?.VcsSyncEnabled,
										PoolingTimeout = sourceEntity?.PollTimeout,
										EnablePooling = sourceEntity?.EnablePoll
									};

			_telemetryQueue.Send(projectTelemetry);
		}
	}
}