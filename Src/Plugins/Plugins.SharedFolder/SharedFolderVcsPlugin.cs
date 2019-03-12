namespace Plugins.SharedFolder
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading;

	using Infrastructure.Plugins.Common.Contracts;
	using Infrastructure.Plugins.Contracts;
	using Plugins.SharedFolder.Properties;

	using PluginSettingDefinition = Infrastructure.Plugins.Contracts.PluginSettingDefinition;

	// ReSharper disable once UnusedMember.Global
	public sealed class SharedFolderVcsPlugin : IVersionControlPlugin
	{
		private IDictionary<string, string> _settingValues;

		/// <summary>
		///     Gets the dictionary of plugin instance settings
		/// </summary>
		/// <returns>Settings dictionary</returns>
		public PluginSettingGroupDefinition GetSettings() =>
			new PluginSettingGroupDefinition
			{
				DisplayName = "Shared folder",
				Code = "vcs_shared_folder",
				SettingDefinitions = new List<Infrastructure.Plugins.Common.Contracts.PluginSettingDefinition>
									{
										new Infrastructure.Plugins.Common.Contracts.PluginSettingDefinition
										{
											DisplayName = "Folder name",
											Code = SharedFolderSettings.FolderUri,
											SettingType = SettingType.Text,
											SettingOwner = SettingOwner.Project,
											IsAuthentication = true
										}
									}
			};

		/// <summary>
		///     Gets the plugin key.
		/// </summary>
		/// <value>
		///     The plugin key.
		/// </value>
		public string Key => "vcs_shared_folder";

		/// <summary>
		///     Gets the plugin title
		/// </summary>
		/// <value>Plugin title</value>
		public string Title => "Shared folder";

		/// <summary>
		///     Initialize plugin by settings
		/// </summary>
		/// <param name="values">The setting values</param>
		public void LoadSettingValues(IDictionary<string, string> values) => _settingValues = values;

		/// <summary>
		///     Gets the current user.
		/// </summary>
		/// <returns>User information</returns>
		public User GetCurrentUser()
		{
			var currentUser = Thread.CurrentPrincipal.Identity;

			return new User
			{
				DisplayName = currentUser.Name,
				InfoUrl = null,
				Login = currentUser.Name
			};
		}

		/// <summary>
		///     Gets the source codes
		/// </summary>
		/// <param name="branchId">The target branch identifier.</param>
		/// <param name="targetPath">The target path to save sources.</param>
		public void GetSources(string branchId, string targetPath)
		{
			using (var sharedFolder = GetClient())
			{
				sharedFolder.CopyTo(branchId, targetPath);
			}
		}

		/// <summary>
		///     Gets the available branches list.
		/// </summary>
		/// <returns>List of branches</returns>
		public IEnumerable<BranchInfo> GetBranches()
		{
			using (var sharedFolder = GetClient())
			{
				var subfolders = new List<BranchInfo>();

				subfolders.AddRange(
					sharedFolder
						.GetSubfolders()
						.Select(
							_ => new BranchInfo
							{
								Id = _,
								IsDefault = false,
								Name = _
							}));

				subfolders.Add(
					new BranchInfo
					{
						Id = @"\",
						IsDefault = true,
						Name = $"\\ ({Resources.RootFolder})"
					});

				return subfolders.OrderBy(_ => _.Id);
			}
		}

		/// <summary>
		///     Creates the branch.
		/// </summary>
		/// <param name="rootFolderPath">Root folder path. Unused for Shared folders</param>
		/// <param name="branchDisplayName">The branch name.</param>
		/// <param name="parentBranchId">The parent branch name.</param>
		/// <returns>Information about created branch</returns>
		public BranchInfo CreateBranch(string rootFolderPath, string branchDisplayName, string parentBranchId)
		{
			if (parentBranchId == null) throw new ArgumentNullException(nameof(parentBranchId));

			using (var sharedFolder = GetClient())
			{
				sharedFolder.CopyLocal(parentBranchId, branchDisplayName);

				return new BranchInfo
				{
					Id = branchDisplayName,
					IsDefault = false,
					Name = branchDisplayName
				};
			}
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
			using (var sharedFolder = GetClient())
			{
				sharedFolder.CreateFile(branchId, fileName, fileBody);
			}
		}

		/// <summary>
		///     Gets checkins history from VCS sinse the last checkin
		/// </summary>
		/// <param name="sinceUtc">The first checkin date</param>
		/// <param name="untilUtc">The last chekin date</param>
		/// <returns>List of checkins sinse the last chekin date</returns>
		public IEnumerable<Commit> GetCommits(DateTime? sinceUtc = null, DateTime? untilUtc = null)
			=> Enumerable.Empty<Commit>();

		/// <summary>
		///     Cleans up plugin
		/// </summary>
		public void CleanUp(string folderPath)
		{
			// do nothing for shared folders
		}

		private SharedFolderClient GetClient() =>
			new SharedFolderClient(_settingValues[SharedFolderSettings.FolderUri]);
	}
}