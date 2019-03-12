namespace Repository
{
	using System;
	using System.Data.Entity;
	using System.Linq;

	using Repository.Context;

	/// <summary>
	///   Database context
	/// </summary>
	public interface IDbContext: IDisposable
	{
		/// <summary>
		///   Applies the updates.
		/// </summary>
		void ApplyUpdates();

		/// <summary>
		///   Creates the transaction.
		/// </summary>
		/// <returns>Transaction for this context</returns>
		DbContextTransaction CreateTransaction();

		/// <summary>
		///   Gets the settings.
		/// </summary>
		/// <param name="cultureId">The culture identifier.</param>
		/// <returns>The settings.</returns>
		IQueryable<Settings> GetSettings(long? cultureId);

		/// <summary>
		///   Gets the table columns.
		/// </summary>
		/// <param name="cultureId">The culture identifier.</param>
		/// <returns>The table columns.</returns>
		IQueryable<TableColumns> GetTableColumns(long? cultureId);

		/// <summary>
		///   Gets the tables.
		/// </summary>
		/// <param name="cultureId">The culture identifier.</param>
		/// <returns>The tables.</returns>
		IQueryable<Tables> GetTables(long? cultureId);

		/// <summary>
		///   Table instance.
		/// </summary>
		/// <typeparam name="T">Table entity type.</typeparam>
		/// <returns>DbSet for table instance access.</returns>
		IDbSet<T> Table<T>() where T: class, IEntity;
	}
}