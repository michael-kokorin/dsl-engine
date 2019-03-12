namespace Repository.Repositories
{
	using Repository.Context;

	/// <summary>
	///   Provides methods to manage user project settings.
	/// </summary>
	public interface IUserProjectSettingsRepository: IWriteRepository<UserProjectSettings>
	{
		/// <summary>
		///   Gets settings by user and project.
		/// </summary>
		/// <param name="userId">The user identifier.</param>
		/// <param name="projectId">The project identifier.</param>
		/// <returns>Settings.</returns>
		UserProjectSettings GetByUserAndProject(long userId, long projectId);
	}
}