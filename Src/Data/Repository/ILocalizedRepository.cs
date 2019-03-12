namespace Repository
{
	using System.Linq;

	/// <summary>
	///   Represents repository that supports localization.
	/// </summary>
	/// <typeparam name="T">Type of repository entity.</typeparam>
	/// <seealso cref="Repository.IWriteRepository{T}"/>
	public interface ILocalizedRepository<T>: IWriteRepository<T>
		where T: class, ILocalizedEntity
	{
		/// <summary>
		///   Queries entity with localization..
		/// </summary>
		/// <returns>Query.</returns>
		IQueryable<T> LocalizedQuery();
	}
}