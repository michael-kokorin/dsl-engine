namespace Plugins.Tfs
{
	using System;
	using System.Collections.Generic;
	using System.Net;

	using JetBrains.Annotations;

	using Microsoft.TeamFoundation.Client;
	using Microsoft.TeamFoundation.Framework.Client;
	using Microsoft.TeamFoundation.Framework.Common;

	using Infrastructure.Plugins.Common.Contracts;
	using Infrastructure.Plugins.Contracts;

	using PluginSettingDefinition = Infrastructure.Plugins.Common.Contracts.PluginSettingDefinition;

	public abstract class TfsPlugin: ICorePlugin
	{
		private IDictionary<string, string> _settingValues;

		/// <summary>
		///     Gets the dictionary of plugin instance settings
		/// </summary>
		/// <returns>Settings dictionary</returns>
		public virtual PluginSettingGroupDefinition GetSettings() =>
			new PluginSettingGroupDefinition
			{
				DisplayName = "TFS",
				Code = "tfs",
				SettingDefinitions = new List<PluginSettingDefinition>
				{
					new PluginSettingDefinition
					{
						DisplayName = "User name",
						Code = TfsSettings.UserName.ToString(),
						SettingType = SettingType.Text,
						SettingOwner = SettingOwner.User,
						IsAuthentication = true
					},
					new PluginSettingDefinition
					{
						DisplayName = "User password",
						Code = TfsSettings.UserPassword.ToString(),
						SettingType = SettingType.Password,
						SettingOwner = SettingOwner.User,
						IsAuthentication = true
					},
					new PluginSettingDefinition
					{
						DisplayName = "Host",
						Code = TfsSettings.HostName.ToString(),
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
		public void LoadSettingValues(IDictionary<string, string> values)
		{
			_settingValues = values;

			// fot SSL check disable
			ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
		}

		/// <summary>
		///     Gets the current user.
		/// </summary>
		/// <returns>User information</returns>
		public User GetCurrentUser()
		{
			var client = GetClient();

			var identityManagementService = client.GetService<IIdentityManagementService>();

			var userName = GetSetting(TfsSettings.UserName);

			var identity = identityManagementService.ReadIdentity(
				IdentitySearchFactor.Alias,
				userName,
				MembershipQuery.None,
				ReadIdentityOptions.ExtendedProperties);

			if (identity == null)
			{
				throw new InvalidOperationException("Current user is not found");
			}

			return new User
			{
				DisplayName = identity.DisplayName,
				Login = identity.UniqueName
			};
		}

		[NotNull]
		protected TfsTeamProjectCollection GetClient()
		{
			var collectionUri = new Uri(GetSetting(TfsSettings.HostName));

			var credential = new NetworkCredential(GetSetting(TfsSettings.UserName), GetSetting(TfsSettings.UserPassword));

			var basicCred = new BasicAuthCredential(credential);

			var tfsCred = new TfsClientCredentials(basicCred)
			{
				AllowInteractive = false
			};

			var teamProjectCollection = new TfsTeamProjectCollection(collectionUri, tfsCred);

			teamProjectCollection.EnsureAuthenticated();

			return teamProjectCollection;
		}

		protected string GetSetting(TfsSettings setting) => _settingValues[setting.ToString()];
	}
}