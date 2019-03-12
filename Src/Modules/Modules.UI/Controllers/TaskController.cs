using System.Collections.Generic;

namespace Modules.UI.Controllers
{
	using System;
	using System.Linq;
	using System.Web.Mvc;

	using JetBrains.Annotations;

	using Microsoft.Practices.ObjectBuilder2;

	using Common.Logging;
	using Common.Security;
	using Modules.Core.Contracts.UI;
	using Modules.Core.Contracts.UI.Dto;
	using Modules.UI.Models.Data;
	using Modules.UI.Models.Entities;
	using Modules.UI.Models.Views;
	using Modules.UI.Models.Views.PersonalCabinet;
	using Modules.UI.Models.Views.Task;

	[Authorize]
	public sealed class TaskController : Controller
	{
		private readonly IApiService _apiService;

		public TaskController([NotNull] IApiService apiService)
		{
			_apiService = apiService;
		}

		[LogMethod(LogInputParameters = true)]
		public ActionResult GetByProject(long projectId)
		{
			var projectTasks = new ProjectTasksViewModel
			{
				IsCanCreateTask = _apiService.HaveAuthority(
					new AuthorityRequestDto
					{
						AuthorityNames = new[] {Authorities.UI.Project.Tasks.CreateNewTask},
						EntityId = projectId
					}),
				Project = _apiService.GetProject(projectId).ToModel(),
				Table = _apiService.GetTasksByProject(projectId).ToModel()
			};

			projectTasks.ScanCore = _apiService.GetScanCore(projectTasks.Project.Id).ToModel();

			if (projectTasks.Project.ItPluginId != null)
				projectTasks.ItPlugin = _apiService.GetPlugin(projectTasks.Project.ItPluginId.Value).ToModel();

			if (projectTasks.Project.VcsPluginId != null)
				projectTasks.VcsPlugin = _apiService.GetPlugin(projectTasks.Project.VcsPluginId.Value).ToModel();

			return View(projectTasks);
		}

		[LogMethod(LogInputParameters = true)]
		public ActionResult GetTaskResults(long taskId)
		{
			var taskResults = new TaskResultsViewModel
			{
				Table = _apiService.GetTaskResultsAsTable(taskId).ToModel(),
				Task = _apiService.GetTask(taskId).ToModel()
			};

			taskResults.ScanCore = _apiService.GetScanCore(taskResults.Task.ProjectId).ToModel();

			if (taskResults.Task.VcsPluginId != null)
				taskResults.VcsPlugin = _apiService.GetPlugin(taskResults.Task.VcsPluginId.Value).ToModel();

			if (taskResults.Task.ItPluginId != null)
				taskResults.ItPlugin = _apiService.GetPlugin(taskResults.Task.ItPluginId.Value).ToModel();

			taskResults.IsCanBeStopped = _apiService.HaveAuthority(
				new AuthorityRequestDto
				{
					AuthorityNames = new[] {Authorities.UI.Project.Tasks.StopTaskExecuting},
					EntityId = taskResults.Task.ProjectId
				})
				&& (taskResults.Task.Status != TaskStatusModel.Done)
				&& (taskResults.Task.Status != TaskStatusModel.PostProcessing);

			return View(taskResults);
		}

		[LogMethod(LogInputParameters = true)]
		public ActionResult NewTask(long projectId)
		{
			BranchDto[] branches;
			try
			{
				branches = _apiService.GetBranches(projectId);
			}
			catch(Exception)
			{
				branches = new BranchDto[0];
			}

			if(branches.Length == 0)
				return PartialView(new CreateTaskViewModel
				{
					ProjectId = projectId,
					Branches = new List<SelectListItem>()
				});

			var latestScannedId = branches
				.Where(_ => _.LastScanFinishedUtc != null)
				.OrderByDescending(_ => _.LastScanFinishedUtc)
				.Select(_ => _.Id)
				.FirstOrDefault();

			var model = new CreateTaskViewModel
			{
				ProjectId = projectId,
				Branches = branches.Select(_ =>
					new SelectListItem
					{
						Value = _.Id,
						Text = _.Name,
						Selected = _.Id == latestScannedId
					}).ToArray()
			};

			if(model.Branches.Any(_ => _.Selected)) return PartialView(model);

			var defaultBranchId = branches.FirstOrDefault(_ => _.IsDefault);

			if(defaultBranchId != null)
			{
				model.Branches.Where(_ => _.Value == defaultBranchId.Id).ForEach(_ => _.Selected = true);
			}

			return PartialView(model);
		}

		[HttpPost]
		[LogMethod]
		public ActionResult Create(CreateTaskViewModel model)
		{
			if(model == null)
				throw new ArgumentNullException(nameof(model));

			if(ModelState.IsValid)
			{
				_apiService.CreateTask(model.ToDto());
			}

			return RedirectToAction("GetByProject", new {projectId = model.ProjectId});
		}

		[LogMethod(LogInputParameters = true)]
		public ActionResult Stop(long taskId)
		{
			_apiService.StopTask(taskId);

			return RedirectToAction("GetTaskResults", new {taskId});
		}

		[LogMethod(LogInputParameters = true)]
		public ActionResult Rescan(string repository, long projectId)
		{
			if(string.IsNullOrEmpty(repository))
				throw new ArgumentNullException(nameof(repository));

			if(ModelState.IsValid)
			{
				_apiService.CreateTask(new CreateTaskDto
				{
					ProjectId = projectId,
					Repository = repository
				});
			}

			return RedirectToAction("GetByProject", new {projectId});
		}
	}
}