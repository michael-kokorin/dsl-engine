namespace Common.Logging
{
	using System;

	using JetBrains.Annotations;

	using Common.Time;
	using Container;

	using Microsoft.Practices.Unity;

	[UsedImplicitly]
	public sealed class ExecutionTimeLogger : IDisposable
	{
		private readonly TimeMeasurement _timeMeasurement;

		private readonly ILog _log;

		private readonly string _mark;

		public ExecutionTimeLogger([NotNull] string mark)
		{
			if (mark == null) throw new ArgumentNullException(nameof(mark));

			_mark = mark;

			_log = IoC.GetContainer().Resolve<ILog>();

			_timeMeasurement = TimeMeasurement.Measure(WriteLog);
		}

		private void WriteLog(TimeSpan timeSpan) => _log.Trace($"Time trace. Mark='{_mark}', Execution time='{timeSpan.TotalMilliseconds}'ms");

		public void Dispose() => _timeMeasurement.Dispose();
	}
}