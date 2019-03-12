namespace Repository.Repositories
{
	using System.Linq;

	using Repository.Context;

	/// <summary>
	///   Provides methods to manage project.
	/// </summary>
	public interface IProjectRepository: IWriteRepository<Projects>
	{
		/// <summary>
		///   Gets project by the specified alias.
		/// </summary>
		/// <param name="alias">The alias.</param>
		/// <returns>Projects.</returns>
		IQueryable<Projects> Get(string alias);

		/// <summary>
		///   Get project which have VCS sync option enabled.
		/// </summary>
		/// <returns>Projects.</returns>
		IQueryable<Projects> VcsSyncEnabled();
	}
}