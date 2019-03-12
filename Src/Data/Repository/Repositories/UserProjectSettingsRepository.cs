namespace Repository.Repositories
{
	using System.Linq;

	using JetBrains.Annotations;

	using Repository.Context;

	[UsedImplicitly]
	internal sealed class UserProjectSettingsRepository:
		Repository<UserProjectSettings>,
		IUserProjectSettingsRepository
	{
		public UserProjectSettingsRepository(IDbContextProvider dbContextProvider): base(dbContextProvider)
		{
		}

		/// <summary>
		///   Gets settings by user and project.
		/// </summary>
		/// <param name="userId">The user identifier.</param>
		/// <param name="projectId">The project identifier.</param>
		/// <returns>
		///   Settings.
		/// </returns>
		public UserProjectSettings GetByUserAndProject(long userId, long projectId) =>
			Query().SingleOrDefault(
				_ =>
				(_.UserId == userId) &&
				(_.ProjectId == projectId));
	}
}