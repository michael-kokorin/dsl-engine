using System.Web.Mvc;

namespace Modules.UI.Controllers
{
	using System;
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Logging;
	using Common.Security;
	using Modules.Core.Contracts.Query;
	using Modules.Core.Contracts.UI;
	using Modules.UI.Models.Data;
	using Modules.UI.Models.Entities;
	using Modules.UI.Models.Views;
	using Modules.UI.Models.Views.Query;
	using Modules.UI.Services;

	[Authorize]
	public sealed class QueryController : Controller
	{
		private readonly IApiService _apiService;

		private readonly IAuthorityProvider _authorityProvider;

		private readonly IQueryService _queryService;

		public QueryController(
			[NotNull] IApiService apiService,
			[NotNull] IAuthorityProvider authorityProvider,
			[NotNull] IQueryService queryService)
		{
			if (apiService == null) throw new ArgumentNullException(nameof(apiService));
			if (authorityProvider == null) throw new ArgumentNullException(nameof(authorityProvider));
			if (queryService == null) throw new ArgumentNullException(nameof(queryService));

			_authorityProvider = authorityProvider;
			_queryService = queryService;
			_apiService = apiService;
		}

		[LogMethod]
		[HttpGet]
		public ActionResult List()
		{
			var userProjects = _authorityProvider.GetProjects(new[] {Authorities.UI.Queries.CreateQuery});

			var model = new QueryListViewModel
			{
				IsCanCreateNewQuery = userProjects.Any(),
				Table = _queryService.GetList().ToModel()
			};

			return View(model);
		}

		[LogMethod]
		public ActionResult CreateQuery()
		{
			var projects = _authorityProvider.GetProjects(new[] {Authorities.UI.Queries.CreateQuery});

			ViewBag.Projects = projects;

			var model = new CreateQueryViewModel();

			return PartialView(model);
		}

		[HttpPost]
		[LogMethod]
		public ActionResult CreateQuery([NotNull] CreateQueryViewModel model)
		{
			if (model == null) throw new ArgumentNullException(nameof(model));

			var creqtedQuery = _queryService.Create(model.ProjectId, model.Name);

			return RedirectToAction("Edit", new {queryId = creqtedQuery.Id});
		}

		[HttpGet]
		[LogMethod(LogInputParameters = true)]
		public ActionResult Edit(long queryId) => View(queryId);

		[HttpGet]
		[LogMethod(LogInputParameters = true)]
		public JsonResult Get(long queryId)
		{
			var query = _queryService.Get(queryId);

			var queryViewModel = new QueryViewModel
			{
				AccessReference = _queryService.GetPrivacyReference().Select(_ => _.ToModel()).ToArray(),
				CurrentUserId = _apiService.GetCurrentUser().Id,
				IsCanEdit = _queryService.IsCanEdit(queryId),
				Query = query.ToModel(),
				SortOrderDirections = _queryService.GetSortDirections().Select(_ => _.ToModel()).ToArray(),
				Users = GetUsersForProject(query.ProjectId)
			};

			return Json(queryViewModel, JsonRequestBehavior.AllowGet);
		}

		private UserModel[] GetUsersForProject(long projectId)
		{
			try
			{
				return _apiService.GetUsers(projectId).Select(_ => _.ToModel()).ToArray();
			}
			catch
			{
				return null;
			}
		}

		[HttpGet]
		[LogMethod(LogInputParameters = true)]
		public JsonResult Execute(long queryId, long userId, string parameters)
		{
			var table = _queryService.Execute(queryId, userId, parameters);

			return Json(table, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public JsonResult GetDataSources(string projectId)
		{
			var dataSources = _queryService.GetDataSources(projectId);

			return Json(dataSources, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		[LogMethod(LogInputParameters = true)]
		public JsonResult GetDataSourceFields([NotNull] string dataSourceKey, string projectId)
		{
			if (dataSourceKey == null) throw new ArgumentNullException(nameof(dataSourceKey));

			var dataSources = _queryService.GetDataSourceFields(dataSourceKey, projectId);

			return Json(dataSources, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		[LogMethod]
		public JsonResult GetFilterConditions()
		{
			var dataSources = _queryService.GetFilterConditions();

			return Json(dataSources, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		[LogMethod]
		public JsonResult GetFilterOperations()
		{
			var dataSources = _queryService.GetFilterOperations();

			return Json(dataSources, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		[LogMethod]
		public void Update(QueryModel query)
		{
			if (query == null)
				throw new ArgumentNullException(nameof(query));

			_queryService.Update(query.ToDto());
		}

		[HttpPost]
		[LogMethod]
		public string GetText([NotNull] string queryModel)
		{
			if (queryModel == null) throw new ArgumentNullException(nameof(queryModel));

			var result = _queryService.ConvertToText(queryModel);

			return result;
		}

		[HttpPost]
		[LogMethod(LogInputParameters = true, LogOutputValue = true)]
		public string GetModel([NotNull] string query)
		{
			if (query == null) throw new ArgumentNullException(nameof(query));

			var result = _queryService.ConvertToModel(query);

			return result;
		}
	}
}