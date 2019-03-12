namespace Plugins.GitLab
{
	public enum GitLabSetting
	{
		/// <summary>
		/// GitLab host address.
		/// Ex. https://gitlab.com
		/// </summary>
		Host,

		/// <summary>
		/// The repository name
		/// </summary>
		RepositoryName,

		/// <summary>
		/// The repository owner
		/// </summary>
		RepositoryOwner,

		/// <summary>
		/// User private token to access GitLab repository
		/// </summary>
		Token
	}
}