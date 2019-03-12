namespace Common.Logging
{
	using System;

	/// <summary>
	///   Saves information into logs.
	/// </summary>
	public interface ILog
	{
		/// <summary>
		///   Logs message with "Debug" severity.
		/// </summary>
		/// <param name="message">The message.</param>
		void Debug(string message);

		/// <summary>
		///   Logs message and exception with "Error" severity.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="exception">The exception.</param>
		void Error(string message, Exception exception = null);

		/// <summary>
		///   Logs message and exception with "Fatal" severity.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="exception">The exception.</param>
		void Fatal(string message, Exception exception = null);

		/// <summary>
		///   Logs message with "Info" severity.
		/// </summary>
		/// <param name="message">The message.</param>
		void Info(string message);

		/// <summary>
		///   Logs message with "Trace" severity.
		/// </summary>
		/// <param name="message">The message.</param>
		void Trace(string message);

		/// <summary>
		///   Logs message and exception with "Warning" severity.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="exception">The exception.</param>
		void Warning(string message, Exception exception = null);
	}
}