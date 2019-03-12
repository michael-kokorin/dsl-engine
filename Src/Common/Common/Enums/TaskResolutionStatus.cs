namespace Common.Enums
{
	/// <summary>
	///   Represents status of task resolution.
	/// </summary>
	public enum TaskResolutionStatus
	{
		/// <summary>
		///   The new resolution.
		/// </summary>
		New = 0,

		/// <summary>
		///   The in progress resolution.
		/// </summary>
		InProgress = 1,

		/// <summary>
		///   The error resolution.
		/// </summary>
		Error = 2,

		/// <summary>
		///   The cancelled resolution.
		/// </summary>
		Cancelled = 3,

		/// <summary>
		///   The successful resolution.
		/// </summary>
		Successful = 4
	}
}