namespace Repository.Repositories
{
	using JetBrains.Annotations;

	using Repository.Context;

	[UsedImplicitly]
	internal sealed class WorkflowActionRepository: Repository<WorkflowActions>, IWorkflowActionRepository
	{
		public WorkflowActionRepository(IDbContextProvider dbContextProvider): base(dbContextProvider)
		{
		}
	}
}