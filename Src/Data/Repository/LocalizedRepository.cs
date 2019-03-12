namespace Repository
{
	using System;
	using System.Linq;

	using JetBrains.Annotations;

	using Repository.Localization;

	/// <summary>
	///   Represents base localized repository.
	/// </summary>
	/// <typeparam name="T">Type of repository entity.</typeparam>
	/// <seealso cref="Repository.Repository{T}"/>
	/// <seealso cref="Repository.ILocalizedRepository{T}"/>
	[UsedImplicitly]
	internal abstract class LocalizedRepository<T>: Repository<T>, ILocalizedRepository<T>
		where T: class, ILocalizedEntity
	{
		private readonly IUserLocalizationProvider _userLocalizationProvider;

		protected LocalizedRepository(
			[NotNull] IDbContextProvider dbContextProvider,
			[NotNull] IUserLocalizationProvider userLocalizationProvider)
			: base(dbContextProvider)
		{
			if(userLocalizationProvider == null) throw new ArgumentNullException(nameof(userLocalizationProvider));

			_userLocalizationProvider = userLocalizationProvider;
		}

		/// <summary>
		///   Queries entity with localization..
		/// </summary>
		/// <returns>Query.</returns>
		public IQueryable<T> LocalizedQuery()
		{
			var cultureId = _userLocalizationProvider.GetCultureId();

			return GetLocalizedQuery(cultureId);
		}

		/// <summary>
		///   Gets the localized query.
		/// </summary>
		/// <param name="cultureId">The culture identifier.</param>
		/// <returns>The query.</returns>
		protected abstract IQueryable<T> GetLocalizedQuery(long cultureId);
	}
}