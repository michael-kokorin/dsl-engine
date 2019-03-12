namespace Repository.Repositories
{
	using Repository.Context;

	/// <summary>
	///   Provide methods to access workflow actions.
	/// </summary>
	internal interface IWorkflowActionRepository: IWriteRepository<WorkflowActions>
	{
	}
}