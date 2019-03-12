namespace Repository.Repositories
{
	using System.Linq;

	using JetBrains.Annotations;

	using Repository.Context;

	[UsedImplicitly]
	internal sealed class WorkflowRuleRepository: Repository<WorkflowRules>, IWorkflowRuleRepository
	{
		public WorkflowRuleRepository(IDbContextProvider dbContextProvider): base(dbContextProvider)
		{
		}

		/// <summary>
		///   Gets workflow rules by event and project.
		/// </summary>
		/// <param name="eventKey">The event key.</param>
		/// <param name="projectId">The project identifier.</param>
		/// <returns>
		///   Workflow rules.
		/// </returns>
		public IQueryable<WorkflowRules> GetByEventAndProject(string eventKey, long projectId)
			=> Query()
				.Where(x => x.ProjectId == projectId)
				.Where(_ => _.IsForAllEvents || _.WorkflowRuleToEvents.Any(e => e.Events.Key == eventKey));
	}
}