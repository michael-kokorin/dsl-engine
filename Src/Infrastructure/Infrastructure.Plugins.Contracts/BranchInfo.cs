namespace Infrastructure.Plugins.Contracts
{
	/// <summary>
	///     Present branch information from Version Control System
	/// </summary>
	public sealed class BranchInfo
	{
		/// <summary>
		///     Gets or sets the branch identifier.
		/// </summary>
		/// <value>
		///     The branch identifier.
		/// </value>
		public string Id { get; set; }

		/// <summary>
		///     Gets or sets a value indicating whether this branch is default in the target VCS.
		/// </summary>
		/// <value>
		///     <c>true</c> if this branch is default; otherwise, <c>false</c>.
		/// </value>
		public bool IsDefault { get; set; }

		/// <summary>
		///     Gets or sets the branch display name.
		/// </summary>
		/// <value>
		///     The branch display name.
		/// </value>
		public string Name { get; set; }
	}
}