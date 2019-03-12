namespace Plugins.GitHub
{
	using System.Collections.Generic;

	using JetBrains.Annotations;

	using Octokit;

	using Infrastructure.Plugins.Common.Contracts;
	using Infrastructure.Plugins.Contracts;
	using Plugins.GitHub.Extensions;

	using PluginSettingDefinition = Infrastructure.Plugins.Contracts.PluginSettingDefinition;
	using User = Infrastructure.Plugins.Contracts.User;

	public abstract class GitHubPlugin: ICorePlugin
	{
		private IDictionary<string, string> _settingValues;

		/// <summary>
		///     Gets the dictionary of plugin instance settings
		/// </summary>
		/// <returns>Settings dictionary</returns>
		public PluginSettingGroupDefinition GetSettings() =>
			new PluginSettingGroupDefinition
			{
				DisplayName = "Github",
				Code = "github",
				SettingDefinitions = new List<Infrastructure.Plugins.Common.Contracts.PluginSettingDefinition>
									{
										new Infrastructure.Plugins.Common.Contracts.PluginSettingDefinition
										{
											DisplayName = "Client Id",
											Code = GitHubItSettingKeys.ClientToken.ToString(),
											SettingType = SettingType.Text,
											SettingOwner = SettingOwner.User,
											IsAuthentication = true
										},
										new Infrastructure.Plugins.Common.Contracts.PluginSettingDefinition
										{
											DisplayName = "Repository name",
											Code = GitHubItSettingKeys.RepositoryName.ToString(),
											SettingType = SettingType.Text,
											SettingOwner = SettingOwner.Project,
											IsAuthentication = true
										},
										new Infrastructure.Plugins.Common.Contracts.PluginSettingDefinition
										{
											DisplayName = "Repository owner",
											Code = GitHubItSettingKeys.RepositoryOwner.ToString(),
											SettingType = SettingType.Text,
											SettingOwner = SettingOwner.Project,
											IsAuthentication = true
										}
									}
			};

		/// <summary>
		///     Gets the plugin title
		/// </summary>
		/// <value>Plugin title</value>
		public abstract string Title { get; }

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
			var client = GetClient();

			var currentUser = client.User.Current().Result;

			return currentUser.ToModel();
		}

		[NotNull]
		private static GitHubClient CreateClient() =>
			new GitHubClient(new ProductHeaderValue("PtSdlVcsPlugin"));

		[NotNull]
		protected GitHubClient GetClient()
		{
			var clientToken = GetSetting(GitHubItSettingKeys.ClientToken);

			var github = CreateClient();

			github.Credentials = new Credentials(clientToken);
			return github;
		}

		protected string GetSetting(GitHubItSettingKeys key)
		{
			if((_settingValues == null) ||
				(_settingValues.Count == 0))
			{
				return null;
			}

			return _settingValues[key.ToString()];
		}
	}
}