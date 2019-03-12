namespace Infrastructure.Query
{
	using Common.Enums;
	using Infrastructure.Engines.Dsl.Query;

	/// <summary>
	/// Query storage service API
	/// </summary>
	public interface IQueryStorage
	{
		long Create(long? projectId, string name);

		/// <summary>
		/// Gets the specified query identifier.
		/// </summary>
		/// <param name="queryId">The query identifier.</param>
		/// <returns></returns>
		QueryInfo Get(long queryId);

		/// <summary>
		/// Gets the specified project identifier.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <param name="name">The name.</param>
		/// <returns>Query object</returns>
		QueryInfo Get(long? projectId, string name);

		/// <summary>
		/// Gets the specified project identifier.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <param name="name">The name.</param>
		/// <returns>Query object</returns>
		QueryInfo GetMy(long? projectId, string name);

		/// <summary>
		/// Updates the specified query identifier.
		/// </summary>
		/// <param name="queryId">The query identifier.</param>
		/// <param name="query">The query.</param>
		/// <param name="name">The name.</param>
		/// <param name="comment">The comment.</param>
		/// <param name="privacyType">Type of the privacy.</param>
		/// <param name="visibilityType">Type of the visibility.</param>
		void Update(long queryId,
			string query,
			string name,
			string comment,
			QueryPrivacyType privacyType,
			QueryVisibilityType visibilityType);

		/// <summary>
		/// Updates the specified query identifier.
		/// </summary>
		/// <param name="queryId">The query identifier.</param>
		/// <param name="query">The query.</param>
		/// <param name="name">The name.</param>
		/// <param name="comment">The comment.</param>
		/// <param name="privacyType">Type of the privacy.</param>
		/// <param name="visibilityType">Type of the visibility.</param>
		void Update(long queryId,
			DslDataQuery query,
			string name,
			string comment,
			QueryPrivacyType privacyType,
			QueryVisibilityType visibilityType);
	}
}