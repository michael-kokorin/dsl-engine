namespace Repository.Repositories
{
	using System.Linq;

	using Repository.Context;

	/// <summary>
	///   Provides methods to manage configurations.
	/// </summary>
	public interface IConfigurationRepository: IWriteRepository<Configuration>
	{
		/// <summary>
		///   Gets configuration by key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>The configuration.</returns>
		IQueryable<Configuration> GetByKey(string key);
	}
}