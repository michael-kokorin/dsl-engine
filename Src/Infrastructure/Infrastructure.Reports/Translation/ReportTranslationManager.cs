namespace Infrastructure.Reports.Translation
{
	using System;

	using JetBrains.Annotations;

	using Common.Enums;
	using Common.Extensions;
	using Common.Time;

	[UsedImplicitly]
	internal sealed class ReportTranslationManager : IReportTranslationManager
	{
		private readonly IReportFileExtensionProvider _reportFileExtensionProvider;

		private readonly IReportTranslatorFabric _reportTranslatorFabric;

		private readonly ITimeService _timeService;

		public ReportTranslationManager([NotNull] IReportFileExtensionProvider reportFileExtensionProvider,
			[NotNull] IReportTranslatorFabric reportTranslatorFabric,
			[NotNull] ITimeService timeService)
		{
			if (reportFileExtensionProvider == null) throw new ArgumentNullException(nameof(reportFileExtensionProvider));
			if (reportTranslatorFabric == null) throw new ArgumentNullException(nameof(reportTranslatorFabric));
			if (timeService == null) throw new ArgumentNullException(nameof(timeService));

			_reportFileExtensionProvider = reportFileExtensionProvider;
			_reportTranslatorFabric = reportTranslatorFabric;
			_timeService = timeService;
		}

		public ReportFile Translate([NotNull] ReportBundle reportBundle, ReportFileType reportFileType)
		{
			if (reportBundle == null) throw new ArgumentNullException(nameof(reportBundle));

			var translator = _reportTranslatorFabric.GetTranslator(reportFileType);

			var reportFileContent = translator.Translate(reportBundle.BodyHtml);

			var reportTitle = GetReportFileName(reportBundle, reportFileType);

			return new ReportFile(reportBundle.Title, reportTitle, reportBundle.BodyHtml, reportFileContent, reportFileType);
		}

		private string GetReportFileName(ReportBundle reportBundle, ReportFileType reportFileType)
		{
			var currentDateUtc = _timeService.GetUtc();

			const string dateMask = "yyyy-MM-dd hh-mm-ss-ffff";

			var reportFileExtension = _reportFileExtensionProvider.Get(reportFileType);

			var validFileName = reportBundle.Title.ToValidPath();

			var fileName = $"{validFileName} {currentDateUtc.ToString(dateMask)}.{reportFileExtension}";

			return fileName;
		}
	}
}