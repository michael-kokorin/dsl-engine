namespace Repository.Localization
{
	/// <summary>
	///   Provides current culture for current thread.
	/// </summary>
	internal interface IUserLocalizationProvider
	{
		/// <summary>
		///   Gets the culture identifier from dictionary.
		/// </summary>
		/// <returns></returns>
		long GetCultureId();
	}
}