namespace Repository.Repositories
{
	using System.Linq;

	using Repository.Context;

	/// <summary>
	///   Provides methods to manage tag entities.
	/// </summary>
	public interface ITagEntityRepository: IWriteRepository<TagEntities>
	{
		/// <summary>
		///   Gets tag entities by specified tag identifier.
		/// </summary>
		/// <param name="tagId">The tag identifier.</param>
		/// <returns>Tag entities.</returns>
		IQueryable<TagEntities> Get(long tagId);

		/// <summary>
		///   Gets tag entities by specified tag identifier.
		/// </summary>
		/// <param name="tagId">The tag identifier.</param>
		/// <param name="tableId">The table identifier.</param>
		/// <param name="entityId">The entity identifier.</param>
		/// <returns>Tag entities.</returns>
		IQueryable<TagEntities> Get(long tagId, long tableId, long entityId);
	}
}