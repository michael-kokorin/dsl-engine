namespace Common.Security
{
	/// <summary>
	///   Represents a user.
	/// </summary>
	public interface IUserPrincipal
	{
		/// <summary>
		///   Gets the user information.
		/// </summary>
		/// <value>
		///   The user information.
		/// </value>
		UserInfo Info { get; }
	}
}