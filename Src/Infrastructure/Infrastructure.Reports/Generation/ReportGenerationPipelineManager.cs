namespace Infrastructure.Reports.Generation
{
	using System;
	using System.Collections.Generic;

	using JetBrains.Annotations;

	using Common.Extensions;
	using Infrastructure.Reports.Generation.Stages;
	using Repository.Context;

	[UsedImplicitly]
	internal sealed class ReportGenerationPipelineManager : IReportGenerationPipelineManager
	{
		private readonly IReportGenerationStageDispatcher _reportGenerationStageDispatcher;

		public ReportGenerationPipelineManager(
			[NotNull] IReportGenerationStageDispatcher reportGenerationStageDispatcher)
		{
			if (reportGenerationStageDispatcher == null)
				throw new ArgumentNullException(nameof(reportGenerationStageDispatcher));

			_reportGenerationStageDispatcher = reportGenerationStageDispatcher;
		}

		public ReportBundle Generate([NotNull] Reports report,
			long userId,
			IReadOnlyDictionary<string, object> parameters)
		{
			if (report == null) throw new ArgumentNullException(nameof(report));

			if (string.IsNullOrEmpty(report.Rule))
				throw new ReportRuleIsEmptyException(report.Id);

			var reportBundle = new ReportBundle
			{
				ParameterValues = parameters,
				Report = report,
				TargetUserId = userId
			};

			try
			{
				reportBundle.Rule = report.Rule.FromJson<ReportRule>();
			}
			catch (Exception ex)
			{
				throw new IncorrectReportRuleException(ex);
			}

			ProcessReport(reportBundle);

			return reportBundle;
		}

		private void ProcessReport(ReportBundle reportBundle)
		{
			var validateParametersStage = _reportGenerationStageDispatcher.Get<ValidateParametersStage>();
			var executeQueriesStage = _reportGenerationStageDispatcher.Get<ExecuteQueriesStage>();
			var renderTitleStage = _reportGenerationStageDispatcher.Get<RenderTitleStage>();
			var addDefaultParametersStage = _reportGenerationStageDispatcher.Get<AddDefaultParametersStage>();
			var vizualizationStage = _reportGenerationStageDispatcher.Get<ReportVizualizationStage>();

			validateParametersStage.SetSuccessor(executeQueriesStage);
			executeQueriesStage.SetSuccessor(renderTitleStage);
			renderTitleStage.SetSuccessor(addDefaultParametersStage);
			addDefaultParametersStage.SetSuccessor(vizualizationStage);

			validateParametersStage.Process(reportBundle);
		}
	}
}