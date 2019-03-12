namespace Common.Security
{
	using System.Collections.Generic;

	/// <summary>
	///   Provides methods to validate authorities.
	/// </summary>
	public interface IUserAuthorityValidator
	{
		/// <summary>
		///   Gets the projects for user..
		/// </summary>
		/// <param name="userId">The user identifier.</param>
		/// <param name="authorityNames">The authority names.</param>
		/// <returns>Collection of project identifiers.</returns>
		IEnumerable<long> GetProjects(long userId, IEnumerable<string> authorityNames);

		/// <summary>
		///   Determines whether the specified user has authorities.
		/// </summary>
		/// <param name="userId">The user identifier.</param>
		/// <param name="authorityNames">The authority names.</param>
		/// <param name="projectId">The project identifier.</param>
		/// <returns><see langword="true"/> if the user has authorities; otherwise, <see langword="false"/>.</returns>
		bool HasUserAuthorities(long userId, IEnumerable<string> authorityNames, long? projectId);
	}
}