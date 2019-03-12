namespace Repository.Repositories
{
	using System.Linq;

	using JetBrains.Annotations;

	using Repository.Context;

	[UsedImplicitly]
	internal sealed class TemplateRepository: Repository<Templates>, ITemplateRepository
	{
		public TemplateRepository(IDbContextProvider dbContextProvider): base(dbContextProvider)
		{
		}

		/// <summary>
		///   Gets template by key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>
		///   Template.
		/// </returns>
		public Templates GetByKey(string key) => Query().FirstOrDefault(x => x.Key == key);
	}
}