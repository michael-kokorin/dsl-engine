namespace Infrastructure.Reports.Blocks.BoolReportBlock
{
	using System;
	using System.Collections.Generic;
	using System.Web.UI;

	using JetBrains.Annotations;

	using Infrastructure.Templates;

	[UsedImplicitly]
	internal sealed class BoolReportBlockVizualizer : IReportBlockVizualizer<BoolReportBlock>
	{
		private readonly IReportBlockVizualizationManager _reportBlockVizualizationManager;

		private readonly ITemplateBuilder _templateBuilder;

		public BoolReportBlockVizualizer([NotNull] IReportBlockVizualizationManager reportBlockVizualizationManager,
			[NotNull] ITemplateBuilder templateBuilder)
		{
			if (reportBlockVizualizationManager == null)
				throw new ArgumentNullException(nameof(reportBlockVizualizationManager));
			if (templateBuilder == null) throw new ArgumentNullException(nameof(templateBuilder));

			_reportBlockVizualizationManager = reportBlockVizualizationManager;
			_templateBuilder = templateBuilder;
		}

		public void Vizualize([NotNull] HtmlTextWriter htmlTextWriter,
			[NotNull] BoolReportBlock block,
			IReadOnlyDictionary<string, object> parameterValues,
			IReadOnlyCollection<ReportQueryResult> queryResults,
			long userId)
		{
			if (htmlTextWriter == null) throw new ArgumentNullException(nameof(htmlTextWriter));
			if (block == null) throw new ArgumentNullException(nameof(block));

			var templateSource = $"$if({block.Variable})$1$endif$";

			var template = _templateBuilder.Build(templateSource);

			if (parameterValues != null)
			{
				foreach (var parameterValue in parameterValues)
				{
					template.Add(parameterValue.Key, parameterValue.Value);
				}
			}

			var result = template.Render();

			if (result.Equals("1"))
			{
				if (block.Positive != null)
					_reportBlockVizualizationManager.Vizualize(
						htmlTextWriter,
						(dynamic) block.Positive,
						parameterValues,
						queryResults,
						userId);
			}
			else
			{
				if (block.Negative != null)
					_reportBlockVizualizationManager.Vizualize(
						htmlTextWriter,
						(dynamic) block.Negative,
						parameterValues,
						queryResults,
						userId);
			}
		}
	}
}