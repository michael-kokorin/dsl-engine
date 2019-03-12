namespace Repository.Repositories
{
	using System.Linq;

	using Repository.Context;

	/// <summary>
	///   Provide methods to access workflow rules.
	/// </summary>
	public interface IWorkflowRuleRepository: IWriteRepository<WorkflowRules>
	{
		/// <summary>
		///   Gets workflow rules by event and project.
		/// </summary>
		/// <param name="eventKey">The event key.</param>
		/// <param name="projectId">The project identifier.</param>
		/// <returns>Workflow rules.</returns>
		// ReSharper disable once ReturnTypeCanBeEnumerable.Global
		// queryable here is required
		IQueryable<WorkflowRules> GetByEventAndProject(string eventKey, long projectId);
	}
}