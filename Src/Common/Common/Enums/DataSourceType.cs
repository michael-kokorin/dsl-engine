namespace Common.Enums
{
	/// <summary>
	///   Database table type
	/// </summary>
	public enum DataSourceType
	{
		/// <summary>
		///   Table with user data
		/// </summary>
		User = 0,

		/// <summary>
		///   Table with system data
		/// </summary>
		System = 1,

		/// <summary>
		///   Table with data, that can't be shown to user
		/// </summary>
		Closed = 2
	}
}