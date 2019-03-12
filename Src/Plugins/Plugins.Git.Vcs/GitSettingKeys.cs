namespace Plugins.Git.Vcs
{
	public enum GitSettingKeys
	{
		/// <summary>
		/// The Git repository URL
		/// Ex. https://gitlab.com/user/repo.git
		/// </summary>
		Url,

		/// <summary>
		/// Username to access it
		/// </summary>
		Username,

		/// <summary>
		/// User password
		/// </summary>
		Password,

		/// <summary>
		/// The email for commit
		/// </summary>
		Email
	}
}