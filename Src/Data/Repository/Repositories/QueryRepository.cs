namespace Repository.Repositories
{
	using System.Collections.Generic;
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Enums;
	using Repository.Context;

	[UsedImplicitly]
	internal sealed class QueryRepository: Repository<Queries>, IQueryRepository
	{
		public QueryRepository([NotNull] IDbContextProvider dbContextProvider): base(dbContextProvider)
		{
		}

		/// <summary>
		///   Gets queries by specified query ids.
		/// </summary>
		/// <param name="queryIds">The query ids.</param>
		/// <returns>
		///   Queries.
		/// </returns>
		public IQueryable<Queries> Get(IEnumerable<long> queryIds) => Query()
			.Where(_ => queryIds.Contains(_.Id) || ((_.ProjectId == null) && _.IsSystem));

		/// <summary>
		///   Gets queries by the specified name.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="projectId">The project identifier.</param>
		/// <returns>
		///   Queries.
		/// </returns>
		public IQueryable<Queries> Get(string name, long? projectId = null) => GetAvailable(projectId)
			.Where(_ => _.Name == name);

		/// <summary>
		///   Gets queries by the specified user identifier.
		/// </summary>
		/// <param name="userId">The user identifier.</param>
		/// <param name="name">The name.</param>
		/// <param name="projectId">The project identifier.</param>
		/// <returns>
		///   Queries.
		/// </returns>
		public Queries Get(long userId, string name, long? projectId = null) => GetAvailable(projectId)
			.SingleOrDefault(
				_ =>
				(_.CreatedById == userId) &&
				(_.Name == name));

		/// <summary>
		///   Gets queries by the specified user identifier.
		/// </summary>
		/// <param name="userId">The user identifier.</param>
		/// <param name="projectId">The project identifier.</param>
		/// <returns>
		///   Queries.
		/// </returns>
		public IQueryable<Queries> Get(long userId, long? projectId) => GetAvailable(projectId)
			.Where(
				_ =>
				(_.CreatedById == userId) ||
				(_.Privacy != (int)QueryPrivacyType.Private));

		/// <summary>
		///   Gets queries by the specified project identifier.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <returns>
		///   Queries.
		/// </returns>
		public IQueryable<Queries> Get(long projectId) => GetAvailable(projectId)
			.Where(_ => (_.ProjectId == projectId) || (_.IsSystem && (_.ProjectId == null)));

		/// <summary>
		///   Gets the available.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <returns></returns>
		private IQueryable<Queries> GetAvailable(long? projectId) => Query()
			.Where(_ => (_.ProjectId == projectId) || ((_.ProjectId == null) && _.IsSystem));
	}
}