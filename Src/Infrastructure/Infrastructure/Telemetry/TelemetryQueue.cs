namespace Infrastructure.Telemetry
{
	using System;
	using System.Collections.Generic;

	using JetBrains.Annotations;

	using Repository;

	[UsedImplicitly]
	internal sealed class TelemetryQueue: ITelemetryQueue
	{
		private readonly ITelemetryInitializer _telemetryInitializer;

		private readonly Queue<ITelemetry> _telemetryQueue;

		private readonly ITelemetryRouter _telemetryRouter;

		public TelemetryQueue(
			[NotNull] ITelemetryRouter telemetryRouter,
			[NotNull] ITelemetryInitializer telemetryInitializer)
		{
			if(telemetryRouter == null) throw new ArgumentNullException(nameof(telemetryRouter));
			if(telemetryInitializer == null) throw new ArgumentNullException(nameof(telemetryInitializer));

			_telemetryRouter = telemetryRouter;
			_telemetryInitializer = telemetryInitializer;

			_telemetryQueue = new Queue<ITelemetry>();

			AutoCommit = true;
		}

		public bool AutoCommit { get; set; }

		public void Send<T>(T telemetryData) where T: class, ITelemetry
		{
			if(telemetryData == null) throw new ArgumentNullException(nameof(telemetryData));

			_telemetryInitializer.Initialize(telemetryData);

			_telemetryQueue.Enqueue(telemetryData);

			if(AutoCommit)
				Commit();
		}

		public void Commit()
		{
			while(_telemetryQueue.Count > 0)
			{
				var telemetryDate = _telemetryQueue.Dequeue();

				_telemetryRouter.Send((dynamic)telemetryDate);
			}
		}
	}
}