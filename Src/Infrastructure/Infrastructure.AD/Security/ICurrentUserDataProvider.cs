namespace Infrastructure.AD.Security
{
	using Common.Security;

	/// <summary>
	///     Provides information about user by specified SID.
	/// </summary>
	internal interface ICurrentUserDataProvider
	{
		/// <summary>
		///     Gets the user information by the specified SID.
		/// </summary>
		/// <returns>The user information.</returns>
		UserInfo GetOrCreate();
	}
}