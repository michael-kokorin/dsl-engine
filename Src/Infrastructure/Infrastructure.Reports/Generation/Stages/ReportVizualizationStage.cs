namespace Infrastructure.Reports.Generation.Stages
{
	using System;
	using System.IO;
	using System.Web.UI;

	using JetBrains.Annotations;

	using Infrastructure.Reports.Blocks;

	[UsedImplicitly]
	internal sealed class ReportVizualizationStage: ReportGenerationStage
	{
		private readonly IReportBlockVizualizationManager _reportBlockVizualizationManager;

		public ReportVizualizationStage([NotNull] IReportBlockVizualizationManager reportBlockVizualizationManager)
		{
			if(reportBlockVizualizationManager == null)
			{
				throw new ArgumentNullException(nameof(reportBlockVizualizationManager));
			}

			_reportBlockVizualizationManager = reportBlockVizualizationManager;
		}

		protected override void ExecuteStage(ReportBundle reportBundle)
		{
			if(reportBundle.Rule.Template.Root == null)
			{
				throw new EmptyReportRuleTemplateException(reportBundle);
			}

			using(var stringWriter = new StringWriter())
			{
				using(var htmlTextWriter = new HtmlTextWriter(stringWriter))
				{
					_reportBlockVizualizationManager.Vizualize(
						htmlTextWriter,
						(dynamic)reportBundle.Rule.Template.Root,
						reportBundle.ParameterValues,
						reportBundle.QueryResults,
						reportBundle.TargetUserId);
				}

				reportBundle.BodyHtml = stringWriter.ToString();
			}
		}
	}
}