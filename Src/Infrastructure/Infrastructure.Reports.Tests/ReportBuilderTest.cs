namespace Infrastructure.Reports.Tests
{
	using System;
	using System.Collections.Generic;

	using FluentAssertions;

	using Moq;

	using NUnit.Framework;

	using Common.Enums;
	using Infrastructure.Reports.Generation;
	using Infrastructure.Reports.Translation;
	using Infrastructure.Telemetry;
	using Repository.Context;

	[TestFixture]
	public sealed class ReportBuilderTest
	{
		private IReportBuilder _target;

		private Mock<IReportGenerationPipelineManager> _reportGenerationPipelineManager;

		private Mock<IReportFileStorage> _repositoryLogger;

		private Mock<IReportStorage> _reportStorage;

		private Mock<IReportTranslationManager> _reportTranslationManager;

		private Mock<ITelemetryScopeProvider> _telemetryScopeProvider;

		[SetUp]
		public void SetUp()
		{
			_reportGenerationPipelineManager = new Mock<IReportGenerationPipelineManager>();
			_repositoryLogger = new Mock<IReportFileStorage>();
			_reportStorage = new Mock<IReportStorage>();
			_reportTranslationManager = new Mock<IReportTranslationManager>();
			_telemetryScopeProvider = new Mock<ITelemetryScopeProvider>();

			_telemetryScopeProvider
				.Setup(_ => _.Create<Reports>(It.IsAny<string>()))
				.Returns(Mock.Of<ITelemetryScope<Reports>>());

			_target = new ReportBuilder(
				_reportGenerationPipelineManager.Object,
				_repositoryLogger.Object,
				_reportStorage.Object,
				_reportTranslationManager.Object,
				_telemetryScopeProvider.Object);
		}

		[Test]
		public void ShouldGenerateReport()
		{
			const int reportId = 234234;

			var report = new Reports
			{
				Id = reportId
			};

			_reportStorage
				.Setup(_ => _.Get(reportId))
				.Returns(report);

			const int userId = 2342342;

			var parameters = new Dictionary<string, object>
			{
				{"Halo", "No"}
			};

			var reportBundle = new ReportBundle();

			_reportGenerationPipelineManager
				.Setup(_ => _.Generate(report, userId, parameters))
				.Returns(reportBundle);

			const ReportFileType reportFileType = ReportFileType.Html;

			var reportFile = new ReportFile("Title", "Repo", "<div></div>",  Guid.NewGuid().ToByteArray(), reportFileType);

			_reportTranslationManager
				.Setup(_ => _.Translate(reportBundle, reportFileType))
				.Returns(reportFile);

			var result = _target.Build(reportId, userId, parameters, reportFileType);

			_repositoryLogger.Verify(_ => _.SaveReportFile(reportBundle, reportFile, userId), Times.Once);

			result.ShouldBeEquivalentTo(reportFile);
		}
	}
}