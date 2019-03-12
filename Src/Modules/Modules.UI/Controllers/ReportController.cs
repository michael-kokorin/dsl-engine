namespace Modules.UI.Controllers
{
	using System;
	using System.Net.Mime;
	using System.Text;
	using System.Web.Mvc;

	using JetBrains.Annotations;

	using Common.Logging;
	using Modules.Core.Contracts.Report;
	using Modules.UI.Models.Data;
	using Modules.UI.Models.Views.Report;

	[Authorize]
	public sealed class ReportController : Controller
	{
		private readonly IReportService _reportService;

		public ReportController([NotNull] IReportService reportService)
		{
			if (reportService == null) throw new ArgumentNullException(nameof(reportService));

			_reportService = reportService;
		}

		[LogMethod]
		public ActionResult Index()
		{
			var reportsTable = _reportService.GetList();

			var reports = new ReportsViewModel
			{
				Table = reportsTable.ToModel()
			};

			return View(reports);
		}

		[LogMethod(LogInputParameters = true)]
		public ActionResult Show(long reportId) => View(reportId);

		[LogMethod(LogInputParameters = true)]
		public JsonResult Get(long reportId)
		{
			var report = _reportService.Get(reportId);

			return Json(report, JsonRequestBehavior.AllowGet);
		}

		[LogMethod(LogInputParameters = true)]
		public FileContentResult Execute(long reportId, string parameters, int type)
		{
			var result = _reportService.Execute(reportId, parameters, type);

			string contentType;

			switch (type)
			{
				case 0: // HTML
					contentType = MediaTypeNames.Text.Html;
					break;
				case 1: //PDF
					contentType = MediaTypeNames.Application.Pdf;
					break;
				default:
					contentType = MediaTypeNames.Application.Octet;
					break;
			}

			return File(result.Content, contentType, result.Title);
		}

		[LogMethod(LogInputParameters = true)]
		public string Build(long reportId, string parameters)
		{
			var result = _reportService.Execute(reportId, parameters, 0);

			return Encoding.UTF8.GetString(result.Content);
		}
	}
}