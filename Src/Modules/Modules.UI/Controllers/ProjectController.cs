namespace Modules.UI.Controllers
{
	using System;
	using System.Linq;
	using System.Web;
	using System.Web.Mvc;

	using JetBrains.Annotations;

	using Common.Extensions;
	using Common.Licencing;
	using Common.Logging;
	using Common.Security;
	using Modules.Core.Contracts.UI;
	using Modules.Core.Contracts.UI.Dto;
	using Modules.Core.Contracts.UI.Dto.ProjectSettings;
	using Modules.Core.Contracts.UI.Dto.UserSettings;
	using Modules.UI.Extensions;
	using Modules.UI.Models.Data;
	using Modules.UI.Models.Entities;
	using Modules.UI.Models.Views.PersonalCabinet;
	using Modules.UI.Models.Views.Plugin;
	using Modules.UI.Models.Views.Project;
	using Modules.UI.Services;

	[Authorize]
	public sealed class ProjectController : Controller
	{
		private readonly IApiService _apiService;

		private readonly IAuthorityProvider _authorityProvider;

		public ProjectController(
			[NotNull] IApiService apiService,
			[NotNull] IAuthorityProvider authorityProvider)
		{
			_apiService = apiService;
			_authorityProvider = authorityProvider;
		}

		[LogMethod]
		public ActionResult Index()
		{
			var projects = _apiService.GetProjectsByUser().Select(_ => _.ToModel()).ToArray();

			var model = new ProjectsViewModel
			{
				IsCanCreateProject = _authorityProvider.IsCan(Authorities.UI.Project.ProjectsList.Create, null)
			};

			if (projects.Length == 0)
				return View(model);

			var currentUser = _apiService.GetCurrentUser();

			var roles = _apiService.GetRoles(currentUser.Id);

			var projectViewModels = projects
				.Select(p => new ProjectViewModel
				{
					IsCanEdit = _authorityProvider.IsCan(new Authorities.UI.Project.Settings().All(), p.Id),
					IsCanViewHealthStat =
						_authorityProvider.IsCan(Authorities.UI.Project.ProjectsList.Stat.Health, p.Id),
					IsCanViewMetricsStat =
						_authorityProvider.IsCan(Authorities.UI.Project.ProjectsList.Stat.Metrics, p.Id),
					IsCanViewVulnerabilitiesStat =
						_authorityProvider.IsCan(Authorities.UI.Project.ProjectsList.Stat.Vulnerabilities, p.Id),
					Project = p,
					Roles = roles
						.Where(r => (r.ProjectId == p.Id) || (r.ProjectId == null))
						.Select(_ => _.ToModel())
						.ToArray(),
					ScanCore = _apiService.GetScanCore(p.Id).ToModel(),
					HealthTable = _apiService.GetProjectHealthStat(p.Id).ToModel(),
					MetricsTable = _apiService.GetProjectMetricStat(p.Id).ToModel(),
					VulnerabilityTable = _apiService.GetProjectVulnerabilitiesStat(p.Id).ToModel()
				});

			model.Projects = projectViewModels.ToList();

			return View(model);
		}

		[LogMethod(LogInputParameters = true)]
		public ActionResult Edit(long projectId)
		{
			var model = new EditProjectViewModel
			{
				ProjectId = projectId,
				ProjectSettings =
				{
					CanRead = _authorityProvider.IsCan(Authorities.UI.Project.Settings.View, projectId),
					CanWrite = _authorityProvider.IsCan(Authorities.UI.Project.Settings.Edit, projectId)
				},
				VcsSettings =
				{
					CanRead = _authorityProvider.IsCan(Authorities.UI.Project.Settings.ViewVersionControl, projectId),
					CanWrite = _authorityProvider.IsCan(Authorities.UI.Project.Settings.EditVersionControl, projectId)
				},
				ItSettings =
				{
					CanRead = _authorityProvider.IsCan(Authorities.UI.Project.Settings.ViewIssueTracker, projectId),
					CanWrite = _authorityProvider.IsCan(Authorities.UI.Project.Settings.EditIssueTracker, projectId)
				},
				RbacSettings =
				{
					CanRead = _authorityProvider.IsCan(Authorities.UI.Project.Settings.ViewAccessControl, projectId),
					CanWrite = _authorityProvider.IsCan(Authorities.UI.Project.Settings.EditAccessControl, projectId)
				},
				SdlSettings =
				{
					CanRead = _authorityProvider.IsCan(Authorities.UI.Project.Settings.ViewSdlPolicy, projectId),
					CanWrite = _authorityProvider.IsCan(Authorities.UI.Project.Settings.EditSdlPolicy, projectId)
				},
				Notifications =
				{
					CanRead = _authorityProvider.IsCan(Authorities.UI.Project.Settings.ViewNotifications, projectId),
					CanWrite = _authorityProvider.IsCan(Authorities.UI.Project.Settings.EditNotifications, projectId)
				}
			};

			var project = _apiService.GetProject(projectId).ToModel();

			if (model.ProjectSettings.CanRead)
			{
				model.ProjectSettings.Alias = project.Alias;
				model.ProjectSettings.CommitToIt = project.CommitToIt;
				model.ProjectSettings.CommitToItEnabled = bool.Parse(_apiService.GetCapability(UserCapabilityKey.EnableCommitToIt));
				model.ProjectSettings.CommitToVcs = project.CommitToVcs;
				model.ProjectSettings.CommitToVcsEnabled = bool.Parse(_apiService.GetCapability(UserCapabilityKey.EnableCommitToVcs));
				model.ProjectSettings.DefaultBranchName = project.DefaultBranchName;
				model.ProjectSettings.Description = project.Description;
				model.ProjectSettings.DisplayName = project.Name;
				model.ProjectSettings.VcsSyncEnabled = project.VcsSyncEnabled;
				model.ProjectSettings.EnablePoll = project.EnablePoll;
				model.ProjectSettings.PollTimeout = project.PollTimeout.ToString();
			}

			if (model.VcsSettings.CanRead)
			{
				model.SelectedVcsId = project.VcsPluginId;

				model.VcsPlugins = _apiService.GetPlugins(PluginTypeDto.VersionControl)
					.Select(_ => new SelectListItem
					{
						Text = _.DisplayName,
						Value = _.Id.ToString()
					});

				if (model.VcsPlugins == null || !model.VcsPlugins.Any())
				{
					model.VcsSettings.CanRead = false;

					model.VcsSettings.CanWrite = false;
				}
			}

			if (model.ItSettings.CanRead)
			{
				model.SelectedItId = project.ItPluginId;

				model.ItPlugins = _apiService.GetPlugins(PluginTypeDto.IssueTracker)
					.Select(_ => new SelectListItem
					{
						Text = _.DisplayName,
						Value = _.Id.ToString()
					});

				if (model.ItPlugins == null || !model.ItPlugins.Any())
				{
					model.ItSettings.CanRead = false;

					model.ItSettings.CanWrite = false;
				}
			}

			if (model.RbacSettings.CanRead)
			{
				model.ProjectRoles = _apiService.GetProjectRoles(projectId)
					.Select(_ => new SelectListItem
					{
						Text = $"{_.DisplayName} ({_.GroupName})",
						Value = _.Id.ToString()
					})
					.ToList();
			}

			if (model.SdlSettings.CanRead)
			{
				model.SdlSettings.SdlRules = _apiService.GetProjectSdlRules(projectId)
					.Select(_ => new SelectListItem
					{
						Text = _.DisplayName,
						Value = _.Id.ToString()
					})
					.ToList();
			}

			if (model.Notifications.CanRead)
			{
				model.Notifications.NotificationRules = _apiService.GetProjectNotificationRules(projectId)
					.Select(_ => new SelectListItem
					{
						Text = _.DisplayName,
						Value = _.Id.ToString()
					})
					.ToList();
			}

			var settings = _apiService.GetEntitySettings(SettingOwnerDto.Project, projectId);

			model.SettingTabs = settings;

			return View(model);
		}

		[HttpGet]
		public ActionResult GetAuthorities(long roleId, long projectId)
		{
			var authorities = _apiService.GetProjectAuthoritiesForRole(projectId, roleId)
				.Select(_ => new SelectListItem
				{
					Text = _.Name,
					Value = _.Id.ToString()
				});

			return Json(authorities, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public ActionResult GetNotificationRule(long ruleId)
		{
			var notificationRule = _apiService.GetNotificationRule(ruleId);

			return Json(notificationRule, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		[LogMethod]
		[ValidateInput(false)]
		public ActionResult SaveNotificationRule(EditNotificationRuleViewModel model)
		{
			_apiService.SaveNotificationRule(new NotificationRuleDto
			{
				Id = model.RuleId,
				Query = model.Query
			});

			return new JsonResult();
		}

		[HttpGet]
		public ActionResult GetPolicyRule(long ruleId)
		{
			var policyRule = _apiService.GetSdlRule(ruleId);

			return Json(policyRule, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		[LogMethod]
		public ActionResult UpdateSettings(EditProjectViewModel model)
		{
			if ((model.ProjectSettings.Alias == null) || (model.ProjectSettings.DisplayName == null) ||
			    (model.ProjectSettings.DefaultBranchName == null))
			{
				return RedirectToAction("Edit", new {projectId = model.ProjectId});
			}

			// ReSharper disable once InvertIf
			if ((model.ProjectSettings != null) &&
			    model.ProjectSettings.CanWrite)
			{
				int pollTimeout;
				int? timeout;
				if (int.TryParse(model.ProjectSettings.PollTimeout, out pollTimeout))
				{
					timeout = pollTimeout;
				}
				else
				{
					timeout = null;
				}

				_apiService.UpdateProjectSettings(
					model.ProjectId,
					new ProjectSettingsDto
					{
						Alias = model.ProjectSettings.Alias,
						CommitToIt = model.ProjectSettings.CommitToIt,
						CommitToVcs = model.ProjectSettings.CommitToVcs,
						DefaultBranchName = model.ProjectSettings.DefaultBranchName,
						Description = HttpUtility.HtmlEncode(model.ProjectSettings.Description),
						DisplayName = HttpUtility.HtmlEncode(model.ProjectSettings.DisplayName),
						VcsSyncEnabled = model.ProjectSettings.VcsSyncEnabled,
						EnablePoll = model.ProjectSettings.EnablePoll,
						PollTimeout = timeout
					});
			}

			return RedirectToAction("Edit", new {projectId = model.ProjectId});
		}

		[LogMethod]
		public ActionResult NewProject()
		{
			var model = new NewProjectViewModel();

			return PartialView(model);
		}

		public ActionResult SaveSettings(string values)
		{
			try
			{
				var items = values.FromJson<SettingValueDto[]>();

				var errors = _apiService.SaveEntitySettings(items);

				return Json(new
				{
					Errors = errors,
					Success = true
				});
			}
			catch (Exception)
			{
				return Json(new
				{
					success = false
				});
			}
		}

		[HttpPost]
		[LogMethod]
		public ActionResult Create(NewProjectViewModel model)
		{
			if (ModelState.IsValid)
			{
				_apiService.CreateProject(new NewProjectDto
				{
					Alias = model.Alias,
					DefaultBranchName = model.DefaultBranchName,
					Name = model.DisplayName
				});
			}

			return RedirectToAction("Index");
		}

		[HttpPost]
		[LogMethod(LogInputParameters = true)]
		public PartialViewResult GetPluginSettings(long projectId, long pluginId)
		{
			var settings = _apiService
				.GetPluginSettingsForProject(pluginId, projectId)
				.Select(_ => _.ToModel())
				.ToList();

			var model = new PluginSettingsModel
			{
				PluginId = pluginId,
				ProjectId = projectId,
				Settings = settings
			};

			return PartialView(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult SavePluginSettings(PluginSettingsModel model)
		{
			_apiService.UpdatePluginSettings(
				model.ProjectId,
				model.PluginId,
				new UpdateProjectPluginSettingsDto
				{
					Settings = model.Settings
						.Select(s => new UpdateProjectPluginSettingDto
						{
							SettingId = s.SettingId,
							Value = s.Value
						})
						.ToArray()
				});

			return RedirectToAction("Edit", new {projectId = model.ProjectId});
		}
	}
}