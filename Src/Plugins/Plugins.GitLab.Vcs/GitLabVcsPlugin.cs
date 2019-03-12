namespace Plugins.GitLab.Vcs
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	using Infrastructure.Plugins.Contracts;
	using Plugins.GitLab.Client.Api;

	public sealed class GitLabVcsPlugin: GitLabPlugin, IVersionControlPlugin
	{
		/// <summary>
		///     Gets the source codes
		/// </summary>
		/// <param name="branchId">The target branch identifier.</param>
		/// <param name="targetPath">The target path to save sources.</param>
		public void GetSources(string branchId, string targetPath)
		{
			var client = GetClient();

			var project = GetProject(client);

			var branch = client.Branches.Get(project.Id, branchId).Result;

			var result = client.Branches.GetArchive(project.Id, branch.Data.Commit.Id).Result;

			var filePath = Path.Combine(targetPath, "temp.zip");

			File.WriteAllBytes(filePath, result.Data);

			FileExtractor.Extract(filePath, targetPath);

			File.Delete(filePath);
		}

		/// <summary>
		///     Gets the available branches list.
		/// </summary>
		/// <returns>List of branches</returns>
		public IEnumerable<BranchInfo> GetBranches()
		{
			var client = GetClient();

			var project = GetProject(client);

			if (project == null)
			{
				return Enumerable.Empty<BranchInfo>();
			}

			var branches = client.Branches.Get(project.Id).Result;

			return branches.Data.Select(
				_ => new BranchInfo
				{
					Id = _.Name,
					IsDefault = project.DefaultBranch.Equals(_.Name),
					Name = _.Name
				});
		}

		/// <summary>
		///     Creates the branch.
		/// </summary>
		/// <param name="rootFolderPath">Root folder path</param>
		/// <param name="branchDisplayName">The branch display name.</param>
		/// <param name="parentBranchId">The parent branch identifier.</param>
		/// <returns>Information about created branch</returns>
		public BranchInfo CreateBranch(string rootFolderPath, string branchDisplayName, string parentBranchId = null)
		{
			var client = GetClient();

			var project = GetProject(client);

			var branch = client.Branches.Create(project.Id, branchDisplayName, parentBranchId).Result;

			return new BranchInfo
			{
				Id = branch.Data.Name,
				IsDefault = false,
				Name = branch.Data.Name
			};
		}

		/// <summary>
		///     Commits the file.
		/// </summary>
		/// <param name="folderPath">Root folder path</param>
		/// <param name="branchId">The target branch identifier.</param>
		/// <param name="message">The commit message.</param>
		/// <param name="fileName">Local name of the file.</param>
		/// <param name="fileBody">The file body.</param>
		public void Commit(string folderPath, string branchId, string message, string fileName, byte[] fileBody)
		{
			var client = GetClient();

			var project = GetProject(client);

			var gitFileName = fileName.Replace("\\", "/");

			client.Files
				.Update(
					project.Id,
					new UpdateFile(branchId, gitFileName, message, fileBody))
				.Wait();
		}

		/// <summary>
		///     Gets checkins history from VCS sinse the last checkin
		/// </summary>
		/// <param name="sinceUtc">The first checkin date</param>
		/// <param name="untilUtc">The last chekin date</param>
		/// <returns>List of checkins sinse the last chekin date</returns>
		public IEnumerable<Commit> GetCommits(DateTime? sinceUtc = null, DateTime? untilUtc = null)
		{
			var client = GetClient();

			var projects = client.Projects.Accessible().Result;

			var commits = new List<Commit>();

			foreach (var project in projects.Data)
			{
				var projectCommits = client.Commits.Get(project.Id, sinceUtc, untilUtc).Result;

				commits.AddRange(
					projectCommits.Data
						.Select(
							_ => new Commit
							{
								Branch = project.Name,
								Committed = _.CreatedAt,
								Key = _.Id,
								Title = _.Title
							}));
			}

			return commits;
		}

		/// <summary>
		///     Cleans up plugin
		/// </summary>
		public void CleanUp(string folderPath)
		{
		}
	}
}