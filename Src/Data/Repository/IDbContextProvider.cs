namespace Repository
{
	/// <summary>
	///   Database current context provider
	/// </summary>
	internal interface IDbContextProvider
	{
		/// <summary>
		///   Gets the context.
		/// </summary>
		/// <returns></returns>
		IDbContext GetContext();
	}
}