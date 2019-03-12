namespace Repository.Repositories
{
	using System.Linq;

	using Repository.Context;

	/// <summary>
	///   Provides methods to manage tags.
	/// </summary>
	public interface ITagRepository: IWriteRepository<Tags>
	{
		/// <summary>
		///   Gets the specified tags.
		/// </summary>
		/// <param name="tag">The tag.</param>
		/// <returns>Tags.</returns>
		IQueryable<Tags> Get(string tag);

		/// <summary>
		///   Gets the specified tags.
		/// </summary>
		/// <param name="tag">The tag.</param>
		/// <param name="tableEntityName">Name of the table entity.</param>
		/// <returns>Tags.</returns>
		IQueryable<Tags> Get(string tag, string tableEntityName);
	}
}