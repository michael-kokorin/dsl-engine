namespace Repository
{
	using Common.Data;

	/// <summary>
	///   Readable repository
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IReadRepository<out T>: IDataSource<T> where T: class, IEntity
	{
		/// <summary>
		///   Gets the entities by its identifier.
		/// </summary>
		/// <param name="id">The entity identifier.</param>
		/// <returns></returns>
		T GetById(long id);
	}
}