namespace Common.Time
{
	using System;

	internal sealed class DateTimeService: ITimeService
	{
		/// <summary>
		///   Gets current UTC time.
		/// </summary>
		/// <returns>
		///   Current UTC time.
		/// </returns>
		public DateTime GetUtc() => DateTime.UtcNow;
	}
}