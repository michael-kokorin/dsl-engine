namespace Infrastructure.Plugins.Contracts
{
	using System;
	using System.Collections.Generic;

	using JetBrains.Annotations;

	/// <summary>
	///     Base interfact for a Version Control System
	/// </summary>
	public interface IVersionControlPlugin: ICorePlugin
	{
		/// <summary>
		///     Cleans up plugin
		/// </summary>
		void CleanUp(string folderPath);

		/// <summary>
		///     Commits the file.
		/// </summary>
		/// <param name="folderPath">Root folder path</param>
		/// <param name="branchId">The target branch identifier.</param>
		/// <param name="message">The commit message.</param>
		/// <param name="fileName">Local name of the file.</param>
		/// <param name="fileBody">The file body.</param>
		void Commit(
			[NotNull] [PathReference] string folderPath,
			[NotNull] string branchId,
			[CanBeNull] string message,
			[NotNull] string fileName,
			[NotNull] byte[] fileBody);

		/// <summary>
		///     Creates the branch.
		/// </summary>
		/// <param name="rootFolderPath">Root folder path</param>
		/// <param name="branchDisplayName">The branch display name.</param>
		/// <param name="parentBranchId">The parent branch identifier.</param>
		/// <returns>Information about created branch</returns>
		[NotNull]
		BranchInfo CreateBranch(
			[NotNull] [PathReference] string rootFolderPath,
			[NotNull] string branchDisplayName,
			[CanBeNull] string parentBranchId = null);

		/// <summary>
		///     Gets the available branches list.
		/// </summary>
		/// <returns>List of branches</returns>
		[NotNull]
		[ItemNotNull]
		IEnumerable<BranchInfo> GetBranches();

		/// <summary>
		///     Gets checkins history from VCS sinse the last checkin
		/// </summary>
		/// <param name="sinceUtc">The first checkin date</param>
		/// <param name="untilUtc">The last chekin date</param>
		/// <returns>List of checkins sinse the last chekin date</returns>
		[NotNull]
		[ItemNotNull]
		IEnumerable<Commit> GetCommits(
			[CanBeNull] DateTime? sinceUtc = null,
			[CanBeNull] DateTime? untilUtc = null);

		/// <summary>
		///     Gets the source codes
		/// </summary>
		/// <param name="branchId">The target branch identifier.</param>
		/// <param name="targetPath">The target path to save sources.</param>
		void GetSources([NotNull] string branchId, [NotNull] [PathReference] string targetPath);
	}
}