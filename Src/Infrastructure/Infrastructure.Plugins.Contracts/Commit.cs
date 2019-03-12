namespace Infrastructure.Plugins.Contracts
{
	using System;

	/// <summary>
	///     Information about Version control system commit
	/// </summary>
	public sealed class Commit
	{
		/// <summary>
		///     Gets or sets the branch.
		/// </summary>
		/// <value>
		///     The branch.
		/// </value>
		public string Branch { get; set; }

		/// <summary>
		///     Gets or sets the committed.
		/// </summary>
		/// <value>
		///     The committed.
		/// </value>
		public DateTime Committed { get; set; }

		/// <summary>
		///     Gets or sets the key.
		/// </summary>
		/// <value>
		///     The key.
		/// </value>
		public string Key { get; set; }

		/// <summary>
		///     Gets or sets the title.
		/// </summary>
		/// <value>
		///     The title.
		/// </value>
		public string Title { get; set; }
	}
}