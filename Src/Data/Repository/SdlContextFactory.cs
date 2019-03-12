namespace Repository
{
	using Repository.Context;

	internal sealed class SdlContextFactory: IDbContextFactory
	{
		/// <summary>
		///   Gets the context.
		/// </summary>
		/// <returns>The database context.</returns>
		public IDbContext GetContext() => new SdlContext();
	}
}