namespace Infrastructure.Plugins.Contracts
{
	/// <summary>
	///     Issue status
	/// </summary>
	public enum IssueStatus
	{
		/// <summary>
		///     Status cant be mapped or unknown
		/// </summary>
		Unknown = 0,

		/// <summary>
		///     Status for new issues
		/// </summary>
		New = 1,

		/// <summary>
		///     Issue is not closed
		/// </summary>
		Open = 2,

		/// <summary>
		///     All work for the issue is done
		/// </summary>
		Closed = 3
	}
}