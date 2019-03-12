namespace Common.Enums
{
	/// <summary>
	///   Represents SDL policy status of task or project.
	/// </summary>
	public enum SdlPolicyStatus
	{
		/// <summary>
		///   The unknown status.
		/// </summary>
		Unknown = 0,

		/// <summary>
		///   SDL policy is satisfied.
		/// </summary>
		Success = 1,

		/// <summary>
		///   SDL policy is not satisfied.
		/// </summary>
		Failed = 2,

		/// <summary>
		///   SDL policy can't be checked because of error.
		/// </summary>
		Error = 3
	}
}