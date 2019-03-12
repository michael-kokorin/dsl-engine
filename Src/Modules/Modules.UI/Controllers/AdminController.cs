namespace Modules.UI.Controllers
{
	using System;
	using System.Web.Mvc;

	using JetBrains.Annotations;

	using Common.Logging;
	using Modules.Core.Contracts.UI;
	using Modules.UI.Models.Views.Admin;

	[Authorize]
	public sealed class AdminController : Controller
	{
		private readonly IApiService _apiService;

		[UsedImplicitly]
		public AdminController([NotNull] IApiService apiService)
		{
			if (apiService == null) throw new ArgumentNullException(nameof(apiService));

			_apiService = apiService;
		}

		[HttpGet]
		[LogMethod(LogInputParameters = true)]
		public ActionResult Index()
		{
			var options = _apiService.GetSettings().ToModel();

			var licence = _apiService.GetLicenceInfo();

			options.SummarySettings.LicenceId = licence.Id;
			options.SummarySettings.LicenceDescription = licence.Description;

			return View(options);
		}

		[HttpPost]
		[LogMethod]
		public ActionResult Index([NotNull] SettingsModel settings)
		{
			if (settings == null) throw new ArgumentNullException(nameof(settings));

			if (ModelState.IsValid)
			{
				_apiService.SetSettings(settings.ToDto());
			}

			return Index();
		}
	}
}