namespace Plugins.GitHub.Vcs.Extensions
{
	using JetBrains.Annotations;

	using Octokit;

	using Infrastructure.Plugins.Contracts;

	/// <summary>
	///     Extension for the Octokit branch object
	/// </summary>
	internal static class ObjectExtension
	{
		/// <summary>
		///     Convert Octokit branch object to the model
		/// </summary>
		/// <param name="branch">The Octokit branch object</param>
		/// <returns>VCS Branch info</returns>
		[NotNull]
		public static BranchInfo ToModel([NotNull] this Branch branch) =>
			new BranchInfo
			{
				Id = branch.Name,
				Name = branch.Name
			};
	}
}