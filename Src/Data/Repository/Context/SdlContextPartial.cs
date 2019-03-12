namespace Repository.Context
{
	using System.Data.Entity;

	/// <summary>
	///   Represents database context.
	/// </summary>
	/// <seealso cref="System.Data.Entity.DbContext"/>
	/// <seealso cref="Repository.IDbContext"/>
	partial class SdlContext: IDbContext
	{
		static SdlContext()
		{
			Database.SetInitializer(new ValidateDatabase<SdlContext>());
		}

		/// <summary>
		///   Gets the table instance.
		/// </summary>
		/// <typeparam name="T">Table entity type</typeparam>
		/// <returns>
		///   DbSet for table instance access.
		/// </returns>
		public IDbSet<T> Table<T>() where T: class, IEntity => Set<T>();

		/// <summary>
		///   Creates the transaction.
		/// </summary>
		/// <returns>
		///   Transaction for this context
		/// </returns>
		public DbContextTransaction CreateTransaction() => Database.BeginTransaction();

		/// <summary>
		///   Applies the updates.
		/// </summary>
		public void ApplyUpdates() => SaveChanges();
	}
}