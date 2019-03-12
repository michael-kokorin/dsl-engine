namespace Repository.Repositories
{
	using System.Linq;

	using JetBrains.Annotations;

	using Repository.Context;

	[UsedImplicitly]
	internal sealed class ScanAgentRepository: Repository<ScanAgents>, IScanAgentRepository
	{
		public ScanAgentRepository(IDbContextProvider dbContextProvider)
			: base(dbContextProvider)
		{
		}

		public ScanAgents GetByUid(string scanAgentUid) => Query().SingleOrDefault(_ => _.Uid == scanAgentUid);
	}
}