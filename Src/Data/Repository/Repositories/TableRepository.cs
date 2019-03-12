namespace Repository.Repositories
{
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Enums;
	using Repository.Context;
	using Repository.Localization;

	internal sealed class TableRepository: LocalizedRepository<Tables>, ITableRepository
	{
		private readonly IDbContextProvider _dbContextProvider;

		public TableRepository(
			[NotNull] IDbContextProvider dbContextProvider,
			[NotNull] IUserLocalizationProvider userLocalizationProvider): base(dbContextProvider, userLocalizationProvider)
		{
			_dbContextProvider = dbContextProvider;
		}

		/// <summary>
		///   Gets all available tables.
		/// </summary>
		/// <returns>
		///   Tables.
		/// </returns>
		public IQueryable<Tables> GetAvailable() => LocalizedQuery().Where(_ => _.Type != (int)DataSourceType.Closed);

		/// <summary>
		///   Gets available tables with the specified identifier.
		/// </summary>
		/// <param name="tableId">The table identifier.</param>
		/// <returns>
		///   Tables.
		/// </returns>
		public IQueryable<Tables> GetAvailable(long tableId) => GetAvailable().Where(_ => _.Id == tableId);

		/// <summary>
		///   Gets tables by the specified name.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns>
		///   Tables.
		/// </returns>
		public IQueryable<Tables> Get(string name) =>
			LocalizedQuery().Where(_ => _.Name == name);

		/// <summary>
		///   Gets the localized query.
		/// </summary>
		/// <param name="cultureId">The culture identifier.</param>
		/// <returns>The query.</returns>
		protected override IQueryable<Tables> GetLocalizedQuery(long cultureId) =>
			_dbContextProvider.GetContext().GetTables(cultureId);
	}
}