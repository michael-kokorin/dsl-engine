namespace Infrastructure.Plugins.Contracts
{
	/// <summary>
	///     Represents a user.
	/// </summary>
	public sealed class User
	{
		/// <summary>
		///     Gets or sets the display name.
		/// </summary>
		/// <value>
		///     The display name.
		/// </value>
		public string DisplayName { get; set; }

		/// <summary>
		///     Gets or sets the information URL.
		/// </summary>
		/// <value>
		///     The information URL.
		/// </value>
		public string InfoUrl { get; set; }

		/// <summary>
		///     Gets or sets the login.
		/// </summary>
		/// <value>
		///     The login.
		/// </value>
		public string Login { get; set; }
	}
}