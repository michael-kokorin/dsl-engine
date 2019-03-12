namespace Common.Time
{
	using System;
	using System.Diagnostics;

	using JetBrains.Annotations;

	/// <summary>
	///   Provides method to measure execution time.
	/// </summary>
	internal sealed class TimeMeasurement: IDisposable
	{
		private readonly Action<TimeSpan> _measureCallback;

		private readonly Stopwatch _stopwatch;

		/// <summary>
		///   Initializes a new instance of the <see cref="TimeMeasurement"/> class.
		/// </summary>
		/// <param name="measureCallback">The measure callback.</param>
		private TimeMeasurement([NotNull] Action<TimeSpan> measureCallback)
		{
			if (measureCallback == null) throw new ArgumentNullException(nameof(measureCallback));

			_measureCallback = measureCallback;

			if(_measureCallback != null)
				_stopwatch = Stopwatch.StartNew();
		}

		/// <summary>
		///   Releases unmanaged and - optionally - managed resources.
		/// </summary>
		public void Dispose()
		{
			if(_stopwatch == null) return;

			_stopwatch.Stop();

			_measureCallback?.Invoke(_stopwatch.Elapsed);
		}

		/// <summary>
		///   Measures the specified callback execution time.
		/// </summary>
		/// <param name="measureCallback">The callback to measure.</param>
		/// <returns>Execution time.</returns>
		public static TimeMeasurement Measure(Action<TimeSpan> measureCallback) => new TimeMeasurement(measureCallback);
	}
}