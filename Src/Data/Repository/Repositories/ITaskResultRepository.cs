namespace Repository.Repositories
{
	using Repository.Context;

	/// <summary>
	///   Provides methods to manage task results.
	/// </summary>
	public interface ITaskResultRepository: IWriteRepository<TaskResults>
	{
	}
}