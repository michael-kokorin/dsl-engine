namespace Infrastructure.Plugins.Contracts
{
	/// <summary>
	///     Request for new issue creation
	/// </summary>
	public sealed class CreateIssueRequest
	{
		/// <summary>
		///     Gets or sets the issue description.
		/// </summary>
		/// <value>
		///     The issue description.
		/// </value>
		public string Description { get; set; }

		/// <summary>
		///     Gets or sets the issue title.
		/// </summary>
		/// <value>
		///     The title.
		/// </value>
		public string Title { get; set; }
	}
}