namespace Workflow.VersionControl
{
	using JetBrains.Annotations;

	/// <summary>
	///     Provide methods to build branch name.
	/// </summary>
	public interface IBranchNameBuilder
	{
		/// <summary>
		///     Builds branch name from the specified information.
		/// </summary>
		/// <param name="info">The information.</param>
		/// <returns>Branch name.</returns>
		string Build([NotNull] BranchNameBuilderInfo info);

		/// <summary>
		///     Gets the information.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns>The info.</returns>
		[NotNull]
		BranchNameBuilderInfo GetInfo([NotNull] string name);

		/// <summary>
		///     Determines whether the specified name is our branch.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns><see langword="true"/> if branch is our; otherwise, <see langword="false"/>.</returns>
		bool IsOurBranch([NotNull] string name);
	}
}