namespace Repository.Repositories
{
	using System.Collections.Generic;
	using System.Linq;

	using Repository.Context;

	/// <summary>
	///   Provide methods to access reports.
	/// </summary>
	public interface IReportRepository: IWriteRepository<Reports>
	{
		/// <summary>
		///   Gets reports by the specified project identifier and report name.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <param name="name">The name.</param>
		/// <returns>Report.</returns>
		IQueryable<Reports> Get(long? projectId, string name);

		/// <summary>
		///   Gets reports by projects.
		/// </summary>
		/// <param name="projectId">The project identifiers.</param>
		/// <returns>Reports.</returns>
		// ReSharper disable once ReturnTypeCanBeEnumerable.Global
		// queryable here is required
		IQueryable<Reports> Get(IEnumerable<long> projectId);
	}
}