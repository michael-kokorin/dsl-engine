namespace Repository.Repositories
{
	using System.Linq;

	using JetBrains.Annotations;

	using Repository.Context;

	[UsedImplicitly]
	internal sealed class PolicyRuleRepository: Repository<PolicyRules>, IPolicyRuleRepository
	{
		public PolicyRuleRepository(IDbContextProvider dbContextProvider): base(dbContextProvider)
		{
		}

		/// <summary>
		///   Gets policy rules by project.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <returns>
		///   Policy rules.
		/// </returns>
		public IQueryable<PolicyRules> Get(long projectId) => Query().Where(_ => _.ProjectId == projectId);

		/// <summary>
		///   Gets SDL policy rules by project and name
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <param name="name">The SDL policy rule name.</param>
		/// <returns>
		///   Policy rules
		/// </returns>
		public IQueryable<PolicyRules> Get(long projectId, string name)
			=> Query().Where(_ => (_.ProjectId == projectId) && (_.Name == name));
	}
}