namespace Infrastructure.Telemetry
{
	using System;
	using System.Diagnostics;

	using JetBrains.Annotations;

	internal sealed class TelemetryScope<T>: ITelemetryScope<T>
		where T: class
	{
		private readonly IEntityTelemetryCreator<T> _entityTelemetryCreator;

		private readonly string _operationName;

		private readonly Stopwatch _stopwatch;

		private T _entity;

		private int? _hResult;

		private bool _isFinished;

		internal TelemetryScope([NotNull] IEntityTelemetryCreator<T> entityTelemetryCreator, [NotNull] string operationName)
		{
			if(entityTelemetryCreator == null) throw new ArgumentNullException(nameof(entityTelemetryCreator));
			if(operationName == null) throw new ArgumentNullException(nameof(operationName));

			_entityTelemetryCreator = entityTelemetryCreator;
			_operationName = operationName;

			_stopwatch = Stopwatch.StartNew();
		}

		/// <summary>
		///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose() => WriteException();

		public void SetEntity(T entity)
		{
			if(entity == null) throw new ArgumentNullException(nameof(entity));

			_entity = entity;
		}

		public void SetHResult(int? hResult) => _hResult = hResult;

		public void WriteException(Exception exception = null)
		{
			if(exception != null)
				_hResult = exception.HResult;

			WriteTelemetry(TelemetryOperationStatus.Exception);
		}

		public void WriteSuccess() => WriteTelemetry(TelemetryOperationStatus.Ok);

		private void WriteTelemetry(TelemetryOperationStatus telemetryOperationStatus)
		{
			if(_isFinished)
				return;

			var measure = _stopwatch.ElapsedMilliseconds;

			_entityTelemetryCreator.Save(_operationName, measure, telemetryOperationStatus, _hResult, _entity);

			_stopwatch.Stop();

			_isFinished = true;
		}
	}
}