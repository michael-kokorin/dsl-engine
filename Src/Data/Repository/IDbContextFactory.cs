namespace Repository
{
	/// <summary>
	///   Represents database context factory contract.
	/// </summary>
	internal interface IDbContextFactory
	{
		/// <summary>
		///   Gets the context.
		/// </summary>
		/// <returns>The database context.</returns>
		IDbContext GetContext();
	}
}