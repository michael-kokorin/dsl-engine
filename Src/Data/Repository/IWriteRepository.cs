namespace Repository
{
	/// <summary>
	///   Writable repository
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IWriteRepository<T>: IReadRepository<T>
		where T: class, IEntity
	{
		/// <summary>
		///   Deletes the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		void Delete(T entity);

		/// <summary>
		///   Inserts the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		void Insert(T entity);

		/// <summary>
		///   Saves changes in current repository.
		/// </summary>
		void Save();
	}
}