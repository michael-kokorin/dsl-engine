namespace Common.Time
{
	using System;

	/// <summary>
	///   Provides info related to time.
	/// </summary>
	public interface ITimeService
	{
		/// <summary>
		///   Gets current UTC time.
		/// </summary>
		/// <returns>Current UTC time.</returns>
		DateTime GetUtc();
	}
}