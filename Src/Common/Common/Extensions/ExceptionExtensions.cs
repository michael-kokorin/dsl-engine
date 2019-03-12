namespace Common.Extensions
{
	using System;

	using Common.Properties;

	/// <summary>
	///   Provides extension methods for <see cref="Exception"/>.
	/// </summary>
	public static class ExceptionExtensions
	{
		/// <summary>
		///   Formats the specified exception for good readability of output.
		/// </summary>
		/// <param name="exception">The exception.</param>
		/// <returns>Formatted content of exception.</returns>
		public static string Format(this Exception exception) => FormatException(exception);

		private static string FormatException(Exception exception)
		{
			var result = Resources.ExceptionFormatString.FormatWith(
				exception.GetType().FullName,
				exception.Message,
				exception.StackTrace);

			if(exception.InnerException != null)
				result += Resources.InnerExceptionFormatString.FormatWith(FormatException(exception.InnerException));

			return result;
		}
	}
}