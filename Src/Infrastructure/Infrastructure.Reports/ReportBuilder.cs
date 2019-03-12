namespace Infrastructure.Reports
{
	using System;
	using System.Collections.Generic;

	using JetBrains.Annotations;

	using Common.Enums;
	using Infrastructure.Reports.Generation;
	using Infrastructure.Reports.Translation;
	using Infrastructure.Telemetry;
	using Repository.Context;

	[UsedImplicitly]
	internal sealed class ReportBuilder: IReportBuilder
	{
		private readonly IReportFileStorage _reportFileStorage;

		private readonly IReportGenerationPipelineManager _reportGenerationPipelineManager;

		private readonly IReportStorage _reportStorage;

		private readonly IReportTranslationManager _reportTranslationManager;

		private readonly ITelemetryScopeProvider _telemetryScopeProvider;

		public ReportBuilder(
			[NotNull] IReportGenerationPipelineManager reportGenerationPipelineManager,
			[NotNull] IReportFileStorage reportFileStorage,
			[NotNull] IReportStorage reportStorage,
			[NotNull] IReportTranslationManager reportTranslationManager,
			[NotNull] ITelemetryScopeProvider telemetryScopeProvider)
		{
			if(reportGenerationPipelineManager == null)
				throw new ArgumentNullException(nameof(reportGenerationPipelineManager));
			if(reportFileStorage == null) throw new ArgumentNullException(nameof(reportFileStorage));
			if(reportStorage == null) throw new ArgumentNullException(nameof(reportStorage));
			if(reportTranslationManager == null) throw new ArgumentNullException(nameof(reportTranslationManager));
			if(telemetryScopeProvider == null) throw new ArgumentNullException(nameof(telemetryScopeProvider));

			_reportGenerationPipelineManager = reportGenerationPipelineManager;
			_reportFileStorage = reportFileStorage;
			_reportStorage = reportStorage;
			_reportTranslationManager = reportTranslationManager;
			_telemetryScopeProvider = telemetryScopeProvider;
		}

		public ReportFile Build(
			long reportId,
			long userId,
			IReadOnlyDictionary<string, object> parameters,
			ReportFileType reportFileType)
		{
			using(var telemetryScope = _telemetryScopeProvider.Create<Reports>(TelemetryOperationNames.Report.Generate))
			{
				try
				{
					var report = GetReport(reportId);

					telemetryScope.SetEntity(report);

					var reportBundle = GetReportBundle(userId, parameters, report);

					var reportFile = TranslateReport(reportFileType, reportBundle);

					SaveReportFileInTempStorage(userId, reportBundle, reportFile);

					telemetryScope.WriteSuccess();

					return reportFile;
				}
				catch(Exception ex)
				{
					telemetryScope.WriteException(ex);

					throw;
				}

			}
		}

		private void SaveReportFileInTempStorage(long userId, ReportBundle reportBundle, ReportFile reportFile) =>
			_reportFileStorage.SaveReportFile(reportBundle, reportFile, userId);

		private ReportFile TranslateReport(ReportFileType reportFileType, ReportBundle reportBundle) =>
			_reportTranslationManager.Translate(reportBundle, reportFileType);

		private ReportBundle GetReportBundle(long userId, IReadOnlyDictionary<string, object> parameters, Reports report) =>
			_reportGenerationPipelineManager.Generate(report, userId, parameters);

		private Reports GetReport(long reportId) => _reportStorage.Get(reportId);
	}
}