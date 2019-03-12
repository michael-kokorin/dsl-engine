namespace Modules.UI.Controllers
{
	using System;
	using System.Linq;
	using System.Web.Mvc;

	using JetBrains.Annotations;

	using Common.Logging;
	using Common.Security;
	using Modules.Core.Contracts.UI;
	using Modules.Core.Contracts.UI.Dto;
	using Modules.Core.Contracts.UI.Dto.ProjectSettings;
	using Modules.Core.Contracts.UI.Dto.UserSettings;
	using Modules.UI.Models;
	using Modules.UI.Models.Entities;
	using Modules.UI.Models.Views.PersonalCabinet;
	using Modules.UI.Models.Views.User;
	using Modules.UI.Services;

	[Authorize]
	public sealed class UserController : BaseController
	{
		private readonly IApiService _apiService;

		private readonly IAuthorityProvider _authorityProvider;

		public UserController(
			[NotNull] IApiService apiService,
			[NotNull] IAuthorityProvider authorityProvider)
		{
			if (apiService == null) throw new ArgumentNullException(nameof(apiService));
			if (authorityProvider == null) throw new ArgumentNullException(nameof(authorityProvider));

			_apiService = apiService;
			_authorityProvider = authorityProvider;
		}

		[ChildActionOnly]
		public ActionResult HeaderActions()
		{
			var model = new UserHeaderActionsViewModel
			{
				IsCanViewAdminPanel = _authorityProvider.GetProjects(new[] {Authorities.UI.Administration.Edit}).Any(),
				IsCanViewProjects = _authorityProvider.GetProjects(new[] {Authorities.UI.Project.ProjectsList.View}).Any(),
				IsCanViewReports = _authorityProvider.GetProjects(new[] {Authorities.UI.Reports.Run}).Any(),
				IsCanViewQueries = _authorityProvider.GetProjects(new[] {Authorities.UI.Queries.ViewQuery}).Any()
			};

			return PartialView(model);
		}

		[ChildActionOnly]
		public ActionResult Info()
		{
			using (Impersonate())
			{
				var currentUser = new UserInfoViewModel
				{
					User = _apiService.GetCurrentUser()
				};

				return PartialView(currentUser);
			}
		}

		[HttpGet]
		[LogMethod]
		public ActionResult PersonalCabinet()
		{
			var userSettings = new UserSettingsViewModel
			{
				UserInfo = _apiService.GetCurrentUser().ToModel()
			};

			var userProjects = _apiService.GetProjectsByUser();

			if ((userProjects == null) ||
			    (userProjects.Length == 0))
				return View(userSettings);

			foreach (var project in userProjects.OrderBy(_ => _.Name))
			{
				var projectPlugins = _apiService.GetPluginsByProject(project.Id);

				foreach (var plugin in projectPlugins)
				{
					var projectPlugin =
						new ProjectPluginViewModel
						{
							Project = project.ToModel(),
							PluginSettingsView = new PluginSettingsViewModel
							{
								Plugin = plugin.ToModel(),
								Settings = _apiService
									.GetPluginSettingsForUserInProject(plugin.Id, project.Id)
									.Select(_ => _.ToModel())
									.ToList()
							}
						};

					switch (plugin.Type)
					{
						case PluginTypeDto.IssueTracker:
							userSettings.ItPluginSettings.Add(projectPlugin);
							break;
						case PluginTypeDto.VersionControl:
							userSettings.VcsPluginSettings.Add(projectPlugin);
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
				}
			}

			return View(userSettings);
		}

		[HttpPost]
		[LogMethod]
		public ActionResult PersonalCabinet(UserSettingsViewModel settingsView)
		{
			if (settingsView == null)
				throw new ArgumentNullException(nameof(settingsView));

			if (!ModelState.IsValid) return View(settingsView);

			UpdateUserInfo(settingsView);

			UpdatePluginSettings(settingsView);

			return View(settingsView);
		}

		private void UpdateUserInfo(UserSettingsViewModel settingsView) => _apiService.UpdateUserInfo(new UserUpdatedDto
		{
			DisplayName = settingsView.UserInfo.DisplayName,
			Email = settingsView.UserInfo.Email,
			Id = settingsView.UserInfo.Id
		});

		private void UpdatePluginSettings(UserSettingsViewModel settingsView)
		{
			var vcsPluginSettings = settingsView.VcsPluginSettings
				.Where(_ => _.PluginSettingsView.Settings != null)
				.SelectMany(_ => _.PluginSettingsView.Settings
					.Select(s => new UpdateProjectPluginSettingDto
					{
						ProjectId = _.Project.Id,
						SettingId = s.SettingId,
						Value = s.Value
					})).ToArray();

			var itPluginSettings = settingsView.ItPluginSettings
				.Where(_ => _.PluginSettingsView.Settings != null)
				.SelectMany(_ =>
					_.PluginSettingsView.Settings.Select(s => new UpdateProjectPluginSettingDto
					{
						ProjectId = _.Project.Id,
						SettingId = s.SettingId,
						Value = s.Value
					})).ToArray();

			var pluginSettings = vcsPluginSettings.Union(itPluginSettings).ToArray();

			_apiService.UpdateUserPluginSetting(pluginSettings);
		}
	}
}