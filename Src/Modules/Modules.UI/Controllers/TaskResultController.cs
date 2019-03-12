namespace Modules.UI.Controllers
{
    using System.Web.Mvc;

    using JetBrains.Annotations;

    using Common.Logging;
    using Modules.Core.Contracts.UI;
    using Modules.UI.Models.Entities;
    using Modules.UI.Models.Views.Result;

    [Authorize]
    public sealed class TaskResultController : Controller
    {
        private readonly IApiService _apiService;

        public TaskResultController([NotNull] IApiService apiService)
        {
            _apiService = apiService;
        }

        [LogMethod(LogInputParameters = true)]
        public ActionResult Get(long resultId)
        {
            var result = new TaskResultViewModel
            {
                TaskResult = _apiService.GetResult(resultId).ToModel()
            };

            return View(result);
        }
    }
}