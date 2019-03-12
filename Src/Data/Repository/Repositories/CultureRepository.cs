namespace Repository.Repositories
{
	using System.Linq;

	using JetBrains.Annotations;

	using Repository.Context;

	[UsedImplicitly]
	internal sealed class CultureRepository: Repository<Cultures>, ICultureRepository
	{
		public CultureRepository([NotNull] IDbContextProvider dbContextProvider)
			: base(dbContextProvider)
		{
		}

		/// <summary>
		///   Gets cultures by the specified code.
		/// </summary>
		/// <param name="code">The code.</param>
		/// <returns>
		///   Cultures.
		/// </returns>
		public IQueryable<Cultures> Get(string code) => Query()
			.Where(_ => _.Code == code);
	}
}