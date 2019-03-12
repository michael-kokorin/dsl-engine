namespace Common.Security
{
	/// <summary>
	///   Represents user information.
	/// </summary>
	public sealed class UserInfo
	{
		/// <summary>
		///   Gets or sets the display name.
		/// </summary>
		/// <value>
		///   The display name.
		/// </value>
		public string DisplayName { get; set; }

		/// <summary>
		///   Gets or sets the email.
		/// </summary>
		/// <value>
		///   The email.
		/// </value>
		public string Email { get; set; }

		/// <summary>
		///   Gets or sets the identifier.
		/// </summary>
		/// <value>
		///   The identifier.
		/// </value>
		public long Id { get; set; }

		/// <summary>
		///   Gets or sets the login.
		/// </summary>
		/// <value>
		///   The login.
		/// </value>
		public string Login { get; set; }

		/// <summary>
		///   Gets or sets the sid.
		/// </summary>
		/// <value>
		///   The sid.
		/// </value>
		public string Sid { get; set; }
	}
}