namespace Plugins.Tfs.Vcs
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	using JetBrains.Annotations;

	using Microsoft.TeamFoundation.VersionControl.Client;
	using Microsoft.TeamFoundation.VersionControl.Common;

	using Infrastructure.Plugins.Contracts;

	public sealed class TfsVcsPlugin : TfsPlugin, IVersionControlPlugin
	{
		public override string Title => "Team Foundation Server";

		/// <summary>
		///     Gets the source codes
		/// </summary>
		/// <param name="branchId">The target branch identifier.</param>
		/// <param name="targetPath">The target path to save sources.</param>
		public void GetSources(string branchId, string targetPath) => GetSources(branchId, targetPath, true);

		/// <summary>
		///     Gets the available branches list.
		/// </summary>
		/// <returns>List of branches</returns>
		public IEnumerable<BranchInfo> GetBranches()
		{
			var vcs = GetVcsClient();

			var branches = vcs.QueryRootBranchObjects(RecursionType.Full);

			return branches
				.Where(_ => _.Properties.RootItem.IsDeleted == false)
				.Select(
					_ => new BranchInfo
					{
						Id = _.Properties.RootItem.Item,
						IsDefault = false, // there is no default branch in TFS
						Name = _.Properties.RootItem.Item
					})
				.OrderBy(_ => _.Name);
		}

		public BranchInfo CreateBranch(string rootFolderPath, string branchDisplayName, string parentBranchId)
		{
			if (rootFolderPath == null)
			{
				throw new ArgumentNullException(nameof(rootFolderPath));
			}
			if (branchDisplayName == null)
			{
				throw new ArgumentNullException(nameof(branchDisplayName));
			}

			var vcs = GetVcsClient();

			string targetBranchId;

			if (parentBranchId != null)
			{
				// combine parent branch folder path with display branch name
				// for example: '$/src/parent' combline with 'child' == '$/src/child'

				var parentFolderpath = parentBranchId.Substring(0, parentBranchId.LastIndexOf('/'));

				targetBranchId = $"{parentFolderpath}/{branchDisplayName}";
			}
			else
			{
				targetBranchId = $"$/{branchDisplayName}";
			}

			// if branch name to create equals source branch name
			if (targetBranchId != parentBranchId)
			{
				if (vcs.ServerItemExists(targetBranchId, ItemType.Any))
				{
					// remove exists branch
					vcs.Destroy(
						new ItemSpec(targetBranchId, RecursionType.Full),
						VersionSpec.Latest,
						null,
						DestroyFlags.Silent);
				}

				vcs.CreateBranch(parentBranchId, targetBranchId, VersionSpec.Latest);
			}

			try
			{
				var sourceFolder = SourceFolder.Create(rootFolderPath, targetBranchId);

				GetSources(targetBranchId, sourceFolder.BranchFolderName, false);
			}
			catch
			{
				vcs.DeleteBranchObject(new ItemIdentifier(targetBranchId));

				throw;
			}

			return new BranchInfo
			{
				Id = targetBranchId,
				IsDefault = false, // there is no default branch in TFS
				Name = targetBranchId
			};
		}

		public void Commit(string folderPath, string branchId, string message, string fileName, byte[] fileBody)
		{
			var folder = SourceFolder.Create(folderPath, branchId);

			var vcs = GetVcsClient();

			var workspace = vcs.GetWorkspace(folder.BranchFolderName);

			var fileExists = folder.Exists(fileName);

			var filePath = folder.SaveFile(fileName, fileBody);

			if (fileExists)
			{
				workspace.PendEdit(filePath);
			}
			else
			{
				workspace.PendAdd(filePath);
			}

			var pendingChanges = workspace.GetPendingChanges(folder.BranchFolderName, RecursionType.Full);

			workspace.CheckIn(pendingChanges, message);
		}

		public IEnumerable<Commit> GetCommits(DateTime? sinceUtc = null, DateTime? untilUtc = null)
		{
			var fromDateVersion = sinceUtc != null ? new DateVersionSpec(sinceUtc.Value) : null;

			var toDateVersion = untilUtc != null ? new DateVersionSpec(untilUtc.Value) : null;

			var vcs = GetVcsClient();

			var branches = GetBranches();

			var commits = new List<Commit>();

			// ReSharper disable once LoopCanBePartlyConvertedToQuery
			foreach (var branch in branches)
			{
				var changesets = vcs.QueryHistory(
					branch.Id,
					VersionSpec.Latest,
					0,
					RecursionType.Full,
					null,
					fromDateVersion,
					toDateVersion,
					int.MaxValue,
					true,
					true)
					.Cast<Changeset>()
					.Select(
						_ => new Commit
						{
							Branch = branch.Id,
							Committed = _.CreationDate,
							Key = _.ChangesetId.ToString(),
							Title = _.Comment
						});

				commits.AddRange(changesets);
			}

			return commits;
		}

		/// <summary>
		///     Detach all workspaces from the folder
		/// </summary>
		/// <param name="folderPath">The folder path with sources.</param>
		public void CleanUp(string folderPath)
		{
			var vcs = GetVcsClient();

			try
			{
				var workspace = vcs.GetWorkspace(folderPath);

				workspace?.Delete();
			}
			catch
			{
				// do nothing
			}

			var workspaces = vcs.QueryWorkspaces(null, vcs.AuthorizedUser, Environment.MachineName);

			var nestedWorkspaces = workspaces
				.Where(
					w => w.Folders
						.Any(
							f => f.LocalItem.StartsWith(
								folderPath,
								StringComparison.CurrentCultureIgnoreCase)));

			foreach (var nestedWorkspace in nestedWorkspaces)
			{
				nestedWorkspace.Delete();
			}
		}

		private void GetSources([NotNull] string branchId, [NotNull] [PathReference] string targetPath, bool removeWorkspace)
		{
			var vcs = GetVcsClient();

			// remove previously created child workspaces for specified path
			CleanUp(targetPath);

			// clear folder before the party
			if (Directory.Exists(targetPath))
			{
				Directory.Delete(targetPath, true);
			}

			Directory.CreateDirectory(targetPath);

			var workspaceName = Guid.NewGuid().ToString("N");

			var workspace = vcs.CreateWorkspace(workspaceName, vcs.AuthorizedUser);

			try
			{
				workspace.Map(branchId, targetPath);

				workspace.Get();
			}
			catch
			{
				// remove workspace only if exception handled
				workspace.Delete();

				throw;
			}

			if (removeWorkspace)
			{
				workspace.Delete();
			}
		}

		private VersionControlServer GetVcsClient() => GetClient().GetService<VersionControlServer>();
	}
}