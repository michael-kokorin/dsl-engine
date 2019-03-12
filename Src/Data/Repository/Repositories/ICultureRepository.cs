namespace Repository.Repositories
{
	using System.Linq;

	using Repository.Context;

	/// <summary>
	///   Provides methods to manage cultures.
	/// </summary>
	public interface ICultureRepository: IWriteRepository<Cultures>
	{
		/// <summary>
		///   Gets cultures by the specified code.
		/// </summary>
		/// <param name="code">The code.</param>
		/// <returns>Cultures.</returns>
		IQueryable<Cultures> Get(string code);
	}
}