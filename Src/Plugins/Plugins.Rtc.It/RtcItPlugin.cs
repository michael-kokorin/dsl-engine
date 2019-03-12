namespace Plugins.Rtc.It
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Linq;
	using System.Net;

	using Infrastructure.Plugins.Common.Contracts;
	using Infrastructure.Plugins.Contracts;
	using Plugins.Rtc.It.Client;
	using Plugins.Rtc.It.Client.Api;
	using Plugins.Rtc.It.Client.Entity;
	using Plugins.Rtc.It.Extensions;

	using Issue = Infrastructure.Plugins.Contracts.Issue;
	using User = Infrastructure.Plugins.Contracts.User;

	// ReSharper disable once UnusedMember.Global
	public sealed class RtcItPlugin : IIssueTrackerPlugin
	{
		private IDictionary<string, string> _settingValues;

		/// <summary>
		///     Gets the dictionary of plugin instance settings
		/// </summary>
		/// <returns>Settings dictionary</returns>
		public PluginSettingGroupDefinition GetSettings() =>
			new PluginSettingGroupDefinition
			{
				DisplayName = "IBM Rational Team Concert 6.x",
				Code = "rtc_it",
				SettingDefinitions = new List<Infrastructure.Plugins.Common.Contracts.PluginSettingDefinition>
									{
										new Infrastructure.Plugins.Common.Contracts.PluginSettingDefinition
										{
											Code = RtcSettings.Username.ToString(),
											DisplayName = "User name",
											SettingType = SettingType.Text,
											SettingOwner = SettingOwner.User,
											IsAuthentication = true
										},
										new Infrastructure.Plugins.Common.Contracts.PluginSettingDefinition
										{
											Code = RtcSettings.Password.ToString(),
											DisplayName = "Password",
											SettingType = SettingType.Password,
											SettingOwner = SettingOwner.User,
											IsAuthentication = true
										},
										new Infrastructure.Plugins.Common.Contracts.PluginSettingDefinition
										{
											Code = RtcSettings.Project.ToString(),
											DisplayName = "Project name",
											SettingType = SettingType.Text,
											SettingOwner = SettingOwner.Project,
											IsAuthentication = true
										},
										new Infrastructure.Plugins.Common.Contracts.PluginSettingDefinition
										{
											Code = RtcSettings.Ccm.ToString(),
											DisplayName = "CCM name",
											SettingType = SettingType.Text,
											SettingOwner = SettingOwner.Project,
											IsAuthentication = true
										},
										new Infrastructure.Plugins.Common.Contracts.PluginSettingDefinition
										{
											Code = RtcSettings.Host.ToString(),
											DisplayName = "Host URL",
											SettingType = SettingType.Password,
											SettingOwner = SettingOwner.Project,
											IsAuthentication = true
										}
									}
			};

		private string GetSetting(RtcSettings setting) => _settingValues[setting.ToString()];

		private RtcClient GetClient()
		{
			var client = RtcClient.Connect(
				GetSetting(RtcSettings.Host),
				GetSetting(RtcSettings.Ccm),
				null);

			Auhenticate(client);

			return client;
		}

		private void Auhenticate(RtcClient rtcClient)
		{
			var result = rtcClient.Auth.Identity().Result;

			if (result.StatusCode == HttpStatusCode.OK)
			{
				var header = GetHeader(result);

				// ReSharper disable once InvertIf
				if (header != null && header.Value.Equals("authrequired"))
				{
					var authResult = rtcClient.Auth.SecurityCheck(
						GetSetting(RtcSettings.Username),
						GetSetting(RtcSettings.Password)).Result;

					var authHeader = GetHeader(authResult);

					if (authHeader != null && authHeader.Value.Equals("authfailed"))
					{
						throw new UnauthorizedAccessException();
					}
				}
			}
			else
			{
				throw new Exception("Incorrect Identity headers");
			}
		}

		private static Header GetHeader(RequestResult result) =>
			result.Headers.FirstOrDefault(_ => _.Key.Equals(
				"x-com-ibm-team-repository-web-auth-msg",
				StringComparison.InvariantCultureIgnoreCase));

		/// <summary>
		///     Gets the plugin title
		/// </summary>
		/// <returns>Plugin title</returns>
		public string Title => "IBM Rational Team Concert 6.x";

		/// <summary>
		///     Initialize plugin by settings
		/// </summary>
		/// <param name="values">The setting values</param>
		public void LoadSettingValues(IDictionary<string, string> values)
		{
			if (values == null) throw new ArgumentNullException(nameof(values));

			_settingValues = values;
		}

		/// <summary>
		///     Gets the current user.
		/// </summary>
		/// <returns>User information</returns>
		public User GetCurrentUser() => new User
		{
			DisplayName = GetSetting(RtcSettings.Username),
			InfoUrl = null,
			Login = GetSetting(RtcSettings.Username)
		};

		/// <summary>
		///     Creates the issue
		/// </summary>
		/// <param name="branchId">Target branch Id</param>
		/// <param name="createIssueRequest">The issue.</param>
		/// <returns></returns>
		public Issue CreateIssue(string branchId, CreateIssueRequest createIssueRequest)
		{
			var client = GetClient();

			var projectUuid = GetProjectUuid(client);

			var workItemTypes = client.Issues.GetTypes(projectUuid).Result;

			ValidateResponse(workItemTypes, HttpStatusCode.OK);

			var workItemType = workItemTypes.Data
				.SingleOrDefault(_ => _.Identifier.EndsWith(WorkItemTypes.Task, StringComparison.Ordinal))
				?.Resource;

			var caterories = client.Issues.GetCategories().Result;

			ValidateResponse(workItemTypes, HttpStatusCode.OK);

			var category = caterories.Data.Entities.SingleOrDefault(_ =>
				_.Archived == false &&
				_.Depth == 0 &&
				_.Title.Equals("Unassigned") &&
				_.ProjectArea.Resource.EndsWith(projectUuid, StringComparison.Ordinal))?.Resource;

			var issue = client.Issues.Create(
				projectUuid,
				new CreateWorkItem
				{
					Description = createIssueRequest.Description,
					FiledAgainstResource = category,
					Title = createIssueRequest.Title,
					TypeResource = workItemType
				})
				.Result;

			ValidateResponse(issue, HttpStatusCode.Created);

			var states = client.Issues.GetStates(projectUuid).Result;

			return issue.Data?.ToDto(states.Data);
		}

		private static void ValidateResponse(RequestResult requestResult, HttpStatusCode statusCode)
		{
			if (requestResult == null) throw new ArgumentNullException(nameof(requestResult));

			if (!Enum.IsDefined(typeof(HttpStatusCode), statusCode))
				throw new InvalidEnumArgumentException(nameof(statusCode), (int) statusCode, typeof(HttpStatusCode));

			if (requestResult.StatusCode != statusCode)
			{
				throw new RequestException(requestResult);
			}
		}

		/// <summary>
		///     Gets the issue.
		/// </summary>
		/// <param name="issueId">The issue identifier.</param>
		/// <returns></returns>
		public Issue GetIssue(string issueId)
		{
			var client = GetClient();

			var issue = client.Issues.Get(Convert.ToInt64(issueId)).Result;

			ValidateResponse(issue, HttpStatusCode.OK);

			var projectUuid = GetProjectUuid(client);

			var states = client.Issues.GetStates(projectUuid).Result;

			return issue.Data?.ToDto(states.Data);
		}

		/// <summary>
		///     Gets the issues.
		/// </summary>
		/// <returns>The issues.</returns>
		public IEnumerable<Issue> GetIssues()
		{
			var client = GetClient();

			var projectUuid = GetProjectUuid(client);

			var issues = client.Issues.GetByProject(projectUuid).Result;

			ValidateResponse(issues, HttpStatusCode.OK);

			var states = client.Issues.GetStates(projectUuid).Result;

			ValidateResponse(issues, HttpStatusCode.OK);

			return issues.Data.Entities.Select(_ => _.ToDto(states.Data));
		}

		private string GetProjectUuid(RtcClient client)
		{
			var project = GetProject(client);

			var lastIndex = project.Resource.LastIndexOf('/');

			if (lastIndex <= 0 || lastIndex == project.Resource.Length)
				throw new IncorrectProjectResourceException(project.Resource);

			var projectUuid = project.Resource.Substring(lastIndex + 1);

			return projectUuid;
		}

		private Project GetProject(RtcClient client)
		{
			var projects = client.Projects.Get().Result;

			ValidateResponse(projects, HttpStatusCode.OK);

			if (projects.Data.Total == 0)
				throw new UnauthorizedAccessException();

			var project = projects.Data.Entities.SingleOrDefault(_ =>
				_.Title.StartsWith(GetSetting(RtcSettings.Project), StringComparison.Ordinal));

			return project;
		}

		/// <summary>
		///     Updates the issue.
		/// </summary>
		/// <param name="updateIssue">The update issue.</param>
		/// <returns></returns>
		public Issue UpdateIssue(UpdateIssueRequest updateIssue)
		{
			var client = GetClient();

			var projectUuid = GetProjectUuid(client);

			var states = client.Issues.GetStates(projectUuid).Result;

			ValidateResponse(states, HttpStatusCode.OK);

			var issue = client.Issues.Update(
				Convert.ToInt64(updateIssue.Id),
				new ChangeWorkItem
				{
					Description = updateIssue.Description,
					Title = updateIssue.Title,
					StateResource = updateIssue.Status.ToDto(states.Data)
				}).Result;

			ValidateResponse(issue, HttpStatusCode.OK);

			return issue.Data.ToDto(states.Data);
		}
	}
}