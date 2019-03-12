namespace Common.Logging
{
	using System;

	using NLog;

	internal sealed class NLogLog: ILog
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		/// <summary>
		///   Logs message with "Debug" severity.
		/// </summary>
		/// <param name="message">The message.</param>
		public void Debug(string message) => WriteLog(LogLevel.Debug, message);

		/// <summary>
		///   Logs message and exception with "Error" severity.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="exception">The exception.</param>
		public void Error(string message, Exception exception = null) => WriteLog(LogLevel.Error, message, exception);

		/// <summary>
		///   Logs message and exception with "Fatal" severity.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="exception">The exception.</param>
		public void Fatal(string message, Exception exception = null) => WriteLog(LogLevel.Fatal, message, exception);

		/// <summary>
		///   Logs message and exception with "Warning" severity.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="exception">The exception.</param>
		public void Warning(string message, Exception exception = null) => WriteLog(LogLevel.Warn, message, exception);

		/// <summary>
		///   Logs message with "Info" severity.
		/// </summary>
		/// <param name="message">The message.</param>
		public void Info(string message) => WriteLog(LogLevel.Info, message);

		/// <summary>
		///   Logs message with "Trace" severity.
		/// </summary>
		/// <param name="message">The message.</param>
		public void Trace(string message) => WriteLog(LogLevel.Trace, message);

		private static void WriteLog(LogLevel level, string message, Exception exception = null)
		{
			var logEvent = new LogEventInfo(level, null, message) {Exception = exception};

			Logger.Log(typeof(NLogLog), logEvent);
		}
	}
}