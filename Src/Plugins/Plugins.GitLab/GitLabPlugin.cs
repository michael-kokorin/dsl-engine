namespace Plugins.GitLab
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Infrastructure.Plugins.Common.Contracts;
	using Infrastructure.Plugins.Contracts;
	using Plugins.GitLab.Client;
	using Plugins.GitLab.Client.Entity;

	using User = Infrastructure.Plugins.Contracts.User;

	public abstract class GitLabPlugin: ICorePlugin
	{
		private IDictionary<string, string> _settingValues;

		/// <summary>
		///     Gets the plugin title
		/// </summary>
		/// <value>Plugin title</value>
		public virtual string Title => "GitLab";

		protected GitLabClient GetClient()
			=> GitLabClient.Connect(GetSetting(GitLabSetting.Host), GetSetting(GitLabSetting.Token));

		/// <summary>
		///     Gets the current user.
		/// </summary>
		/// <returns>User information</returns>
		public User GetCurrentUser()
		{
			var user = GetClient().Users.GetCurrent().Result;

			return new User
			{
				DisplayName = user.Data.Name,
				InfoUrl = user.Data.WebUrl,
				Login = user.Data.UserName
			};
		}

		protected string GetSetting(GitLabSetting setting) => _settingValues[setting.ToString()];

		public PluginSettingGroupDefinition GetSettings() =>
			new PluginSettingGroupDefinition
			{
				DisplayName = "Gitlab",
				Code = "gitlab",
				SettingDefinitions = new List<Infrastructure.Plugins.Common.Contracts.PluginSettingDefinition>
				{
					new Infrastructure.Plugins.Common.Contracts.PluginSettingDefinition
					{
						Code = GitLabSetting.Token.ToString(),
						DisplayName = "Private token",
						SettingType = SettingType.Password,
						SettingOwner = SettingOwner.User,
						IsAuthentication = true
					},
					new Infrastructure.Plugins.Common.Contracts.PluginSettingDefinition
					{
						Code = GitLabSetting.RepositoryName.ToString(),
						DisplayName = "Project name",
						SettingType = SettingType.Text,
						SettingOwner = SettingOwner.Project,
						IsAuthentication = true
					},
					new Infrastructure.Plugins.Common.Contracts.PluginSettingDefinition
					{
						Code = GitLabSetting.RepositoryOwner.ToString(),
						DisplayName = "Project owner",
						SettingType = SettingType.Text,
						SettingOwner = SettingOwner.Project,
						IsAuthentication = true
					},
					new Infrastructure.Plugins.Common.Contracts.PluginSettingDefinition
					{
						Code = GitLabSetting.Host.ToString(),
						DisplayName = "Host",
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
		public void LoadSettingValues(IDictionary<string, string> values)
		{
			if (values == null)
			{
				throw new ArgumentNullException(nameof(values));
			}

			_settingValues = values;
		}

		protected Project GetProject(GitLabClient client)
		{
			var projects = client.Projects.Accessible().Result;

			if (projects.Data == null)
			{
				return null;
			}

			var repositoryName = GetSetting(GitLabSetting.RepositoryName);

			var userName = GetSetting(GitLabSetting.RepositoryOwner);

			var project = projects.Data.SingleOrDefault(
				_ =>
					(_.Name == repositoryName) &&
					(_.Owner?.UserName == userName));

			return project;
		}
	}
}