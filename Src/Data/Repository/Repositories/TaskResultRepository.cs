namespace Repository.Repositories
{
	using JetBrains.Annotations;

	using Repository.Context;

	[UsedImplicitly]
	internal sealed class TaskResultRepository: Repository<TaskResults>, ITaskResultRepository
	{
		public TaskResultRepository(IDbContextProvider dbContextProvider): base(dbContextProvider)
		{
		}
	}
}