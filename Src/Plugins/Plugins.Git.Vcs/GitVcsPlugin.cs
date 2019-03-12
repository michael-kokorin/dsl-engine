namespace Plugins.Git.Vcs
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using System.Reflection;

	using Infrastructure.Plugins.Common.Contracts;
	using Infrastructure.Plugins.Contracts;
	using Plugins.Git.Vcs.Client;
	using Plugins.Git.Vcs.Extensions;

	using Commit = Infrastructure.Plugins.Contracts.Commit;
	using PluginSettingDefinition = Infrastructure.Plugins.Contracts.PluginSettingDefinition;

	public sealed class GitVcsPlugin : IVersionControlPlugin
	{
		/// <summary>
		///     Gets the plugin title
		/// </summary>
		/// <value>Plugin title</value>
		public string Title => "Git";

		private IDictionary<string, string> _settingValues;

		public PluginSettingGroupDefinition GetSettings() =>
			new PluginSettingGroupDefinition
			{
				DisplayName = "Git VCS",
				Code = "git_vcs",
				SettingDefinitions = new List<Infrastructure.Plugins.Common.Contracts.PluginSettingDefinition>
									{
										new Infrastructure.Plugins.Common.Contracts.PluginSettingDefinition
										{
											DisplayName = "User name",
											Code = GitSettingKeys.Username.ToString(),
											SettingType = SettingType.Text,
											SettingOwner = SettingOwner.User,
											IsAuthentication = true
										},
										new Infrastructure.Plugins.Common.Contracts.PluginSettingDefinition
										{
											DisplayName = "Password",
											Code = GitSettingKeys.Password.ToString(),
											SettingType = SettingType.Password,
											SettingOwner = SettingOwner.User,
											IsAuthentication = true
										},
										new Infrastructure.Plugins.Common.Contracts.PluginSettingDefinition
										{
											DisplayName = "Email for commit",
											Code = GitSettingKeys.Email.ToString(),
											SettingType = SettingType.Text,
											SettingOwner = SettingOwner.User,
											IsAuthentication = true
										},
										new Infrastructure.Plugins.Common.Contracts.PluginSettingDefinition
										{
											DisplayName = "URI",
											Code = GitSettingKeys.Url.ToString(),
											SettingType = SettingType.Text,
											SettingOwner = SettingOwner.Project,
											IsAuthentication = true
										}
									}
			};

		/// <summary>
		///     Initialize plugin by settings
		/// </summary>
		/// <param name="values">The setting values</param>
		public void LoadSettingValues(IDictionary<string, string> values) => _settingValues = values;

		/// <summary>
		///     Gets the current user.
		/// </summary>
		/// <returns>User information</returns>
		public User GetCurrentUser() => new User
		{
			DisplayName = GetSetting(GitSettingKeys.Username),
			InfoUrl = null,
			Login = GetSetting(GitSettingKeys.Username)
		};

		/// <summary>
		///     Gets the source codes
		/// </summary>
		/// <param name="branchId">The target branch identifier.</param>
		/// <param name="targetPath">The target path to save sources.</param>
		public void GetSources(string branchId, string targetPath) =>
			new GitRepository(targetPath).CloneFrom(
				new CloneRepository(
					GetSetting(GitSettingKeys.Url),
					branchId,
					GetSetting(GitSettingKeys.Username),
					GetSetting(GitSettingKeys.Password)));

		/// <summary>
		///     Gets the available branches list.
		/// </summary>
		/// <returns>List of branches</returns>
		public IEnumerable<BranchInfo> GetBranches() =>
			GitRepository.ListBranches(new ListBranches(
				GetSetting(GitSettingKeys.Url),
				GetSetting(GitSettingKeys.Username),
				GetSetting(GitSettingKeys.Password)));

		/// <summary>
		///     Creates the branch.
		/// </summary>
		/// <param name="rootFolderPath">Root folder path</param>
		/// <param name="branchDisplayName">The branch display name.</param>
		/// <param name="parentBranchId">The parent branch identifier.</param>
		/// <returns>Information about created branch</returns>
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

			var branchFolderPath = Path.Combine(rootFolderPath, branchDisplayName);

			Copy(rootFolderPath, branchFolderPath);

			return new GitRepository(branchFolderPath)
				.CreateBranch(new CreateBranch(branchDisplayName,
					GetSetting(GitSettingKeys.Username),
					GetSetting(GitSettingKeys.Password)));
		}

		private static void Copy(string sourcePath, string targetPath)
		{
			if (sourcePath[sourcePath.Length - 1] == Path.DirectorySeparatorChar)
				sourcePath = sourcePath.Remove(sourcePath.Length - 1);

			var directories = Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories);

			var files = Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories);

			foreach (var dirPath in directories)
				Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));

			foreach (var newPath in files)
				File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
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
			var branchPath = Path.Combine(folderPath, branchId);

			var filePath = Path.Combine(branchPath, fileName);

			File.WriteAllBytes(filePath, fileBody);

			var gitRepo = new GitRepository(branchPath);

			gitRepo.Commit(new TakeCommit(branchId, fileName, GetSetting(GitSettingKeys.Email))
			{
				Message = message,
				Username = GetSetting(GitSettingKeys.Username),
				Password = GetSetting(GitSettingKeys.Password)
			});
		}

		/// <summary>
		///     Gets checkins history from VCS sinse the last checkin
		/// </summary>
		/// <param name="sinceUtc">The first checkin date</param>
		/// <param name="untilUtc">The last chekin date</param>
		/// <returns>List of checkins sinse the last chekin date</returns>
		public IEnumerable<Commit> GetCommits(DateTime? sinceUtc = null, DateTime? untilUtc = null)
		{
			var tempPath = Path.GetTempPath();

			var guid = Guid.NewGuid().ToString();

			var versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);

			var folderPath = Path.Combine(tempPath,
				versionInfo.CompanyName,
				versionInfo.ProductName,
				Title,
				guid);

			if (!Directory.Exists(folderPath))
				Directory.CreateDirectory(folderPath);

			var repository = new GitRepository(folderPath);

			repository.CloneFrom(new CloneRepository(
				GetSetting(GitSettingKeys.Url),
				null,
				GetSetting(GitSettingKeys.Username),
				GetSetting(GitSettingKeys.Password)));

			var branches = GetBranches();

			var commitsList = repository.GetCommits(
				branches.Select(_ => _.Name),
				new GetCommits(sinceUtc, untilUtc));

			DirectoryHelper.SafeDelete(folderPath);

			return commitsList;
		}

		/// <summary>
		///     Cleans up plugin
		/// </summary>
		public void CleanUp(string folderPath)
		{
		}


		private string GetSetting(GitSettingKeys key)
		{
			if ((_settingValues == null) ||
			    (_settingValues.Count == 0))
			{
				return null;
			}

			return _settingValues[key.ToString()];
		}
	}
}