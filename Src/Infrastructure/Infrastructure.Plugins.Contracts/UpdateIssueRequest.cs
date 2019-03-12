namespace Infrastructure.Plugins.Contracts
{
	/// <summary>
	///     Update issue with specified info
	/// </summary>
	public sealed class UpdateIssueRequest
	{
		/// <summary>
		///     Gets or sets the description.
		/// </summary>
		/// <value>
		///     The description.
		/// </value>
		public string Description { get; set; }

		/// <summary>
		///     Gets or sets the identifier.
		/// </summary>
		/// <value>
		///     The identifier.
		/// </value>
		public string Id { get; set; }

		/// <summary>
		///     Gets or sets the status.
		/// </summary>
		/// <value>
		///     The status.
		/// </value>
		public IssueStatus Status { get; set; }

		/// <summary>
		///     Gets or sets the title.
		/// </summary>
		/// <value>
		///     The title.
		/// </value>
		public string Title { get; set; }
	}
}