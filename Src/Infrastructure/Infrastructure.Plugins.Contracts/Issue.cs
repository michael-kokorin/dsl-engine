namespace Infrastructure.Plugins.Contracts
{
	using System;

	/// <summary>
	///     Represents information about Issue
	/// </summary>
	public sealed class Issue
	{
		/// <summary>
		///     Gets or sets the assigned to.
		/// </summary>
		/// <value>
		///     The assigned to.
		/// </value>
		public string AssignedTo { get; set; }

		/// <summary>
		///     Gets or sets the created.
		/// </summary>
		/// <value>
		///     The created.
		/// </value>
		public DateTime Created { get; set; }

		/// <summary>
		///     Gets or sets the created by.
		/// </summary>
		/// <value>
		///     The created by.
		/// </value>
		public string CreatedBy { get; set; }

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

		/// <summary>
		///     Gets or sets the URL.
		/// </summary>
		/// <value>
		///     The URL.
		/// </value>
		public string Url { get; set; }
	}
}