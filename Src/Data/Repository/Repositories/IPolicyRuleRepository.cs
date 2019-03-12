namespace Repository.Repositories
{
	using System.Linq;

	using Repository.Context;

	/// <summary>
	///   Provide methods to access policy rules.
	/// </summary>
	public interface IPolicyRuleRepository: IWriteRepository<PolicyRules>
	{
		/// <summary>
		///   Gets policy rules by project.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <returns>Policy rules.</returns>
		// ReSharper disable once ReturnTypeCanBeEnumerable.Global
		// queryable here is required
		IQueryable<PolicyRules> Get(long projectId);

		/// <summary>
		///   Gets SDL policy rules by project and name
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <param name="name">The SDL policy rule name.</param>
		/// <returns>Policy rules</returns>
		IQueryable<PolicyRules> Get(long projectId, string name);
	}
}