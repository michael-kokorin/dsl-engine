namespace Infrastructure.Engines
{
	using System.Collections.Generic;

	using Infrastructure.Engines.Dsl;
	using Repository.Context;

	/// <summary>
	///     Provides users for the specified expression.
	/// </summary>
	public interface IUserProvider
	{
		/// <summary>
		///     Gets the users.
		/// </summary>
		/// <param name="subjects">The subjects.</param>
		/// <param name="projectId">The project identifier.</param>
		/// <returns>
		///     Users.
		/// </returns>
		IEnumerable<Users> GetUsers(SubjectsExpr subjects, long? projectId);
	}
}