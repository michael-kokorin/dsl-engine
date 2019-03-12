namespace Repository.Repositories
{
	using System.Collections.Generic;
	using System.Linq;

	using JetBrains.Annotations;

	using Repository.Context;

	[UsedImplicitly]
	internal sealed class ReportRepository : Repository<Reports>, IReportRepository
	{
		public ReportRepository(IDbContextProvider dbContextProvider)
			: base(dbContextProvider)
		{
		}

		/// <summary>
		///   Gets reports by the specified project identifier and report name.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <param name="name">The name.</param>
		/// <returns>
		///   Report.
		/// </returns>
		public IQueryable<Reports> Get(long? projectId, string name) =>
			Query().Where(_ => (_.ProjectId == projectId || (_.ProjectId == null && _.IsSystem)) && (_.DisplayName == name));

		/// <summary>
		///   Gets the specified project ids.
		/// </summary>
		/// <param name="projectIds">The project ids.</param>
		/// <returns></returns>
		public IQueryable<Reports> Get(IEnumerable<long> projectIds) =>
			Query().Where(_ =>
				((_.ProjectId == null) && _.IsSystem) ||
				projectIds.Contains((long) _.ProjectId));
	}
}