namespace Repository.Repositories
{
	using Repository.Context;

	/// <summary>
	///   Provides methods to manage scan agents.
	/// </summary>
	public interface IScanAgentRepository: IWriteRepository<ScanAgents>
	{
		ScanAgents GetByUid(string scanAgentUid);
	}
}