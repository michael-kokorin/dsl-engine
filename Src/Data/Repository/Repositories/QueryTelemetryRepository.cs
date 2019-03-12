namespace Repository.Repositories
{
	using System;

	using JetBrains.Annotations;

	using Repository.Context;

	[UsedImplicitly]
	internal sealed class QueryTelemetryRepository : Repository<QueryTelemetry>, IQueryTelemetryRepositroy
	{
		/// <summary>
		///   Initializes a new instance of the <see cref="Repository{T}"/> class.
		/// </summary>
		/// <param name="dbContextProvider">The database context provider.</param>
		/// <exception cref="ArgumentNullException"><paramref name="dbContextProvider"/> is <see langword="null"/>.</exception>
		public QueryTelemetryRepository([NotNull] IDbContextProvider dbContextProvider) : base(dbContextProvider)
		{
		}
	}
}