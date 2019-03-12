namespace Common.Extensions
{
	using System;

	/// <summary>
	///   Provides extension methods for <see cref="DateTime"/>.
	/// </summary>
	public static class DateTimeExtension
	{
		/// <summary>
		///   Specifies that this instance is in UTC timezone.
		/// </summary>
		/// <param name="dateTime">The date time.</param>
		/// <returns>Converted instance.</returns>
		public static DateTime AsUtc(this DateTime dateTime) => DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
	}
}