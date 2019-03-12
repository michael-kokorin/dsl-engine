namespace Repository.Repositories
{
	using System.Linq;

	using JetBrains.Annotations;

	using Repository.Context;

	[UsedImplicitly]
	internal sealed class ConfigurationRepository: Repository<Configuration>, IConfigurationRepository
	{
		public ConfigurationRepository(IDbContextProvider dbContextProvider): base(dbContextProvider)
		{
		}

		/// <summary>
		///   Gets configuration by key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>The configuration.</returns>
		public IQueryable<Configuration> GetByKey(string key) =>
			Query().Where(_ => _.Name == key);
	}
}