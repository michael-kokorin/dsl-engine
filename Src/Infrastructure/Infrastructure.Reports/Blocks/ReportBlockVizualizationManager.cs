namespace Infrastructure.Reports.Blocks
{
	using System;
	using System.Collections.Generic;
	using System.Web.UI;

	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class ReportBlockVizualizationManager : IReportBlockVizualizationManager
	{
		private readonly IReportBlockVizualizerFabric _reportBlockVizualizerFabric;

		public ReportBlockVizualizationManager([NotNull] IReportBlockVizualizerFabric reportBlockVizualizerFabric)
		{
			if (reportBlockVizualizerFabric == null) throw new ArgumentNullException(nameof(reportBlockVizualizerFabric));

			_reportBlockVizualizerFabric = reportBlockVizualizerFabric;
		}

		public void Vizualize<T>(
			HtmlTextWriter textWriter,
			[NotNull] T reportBlock,
			IReadOnlyDictionary<string, object> parameterValues,
			IReadOnlyCollection<ReportQueryResult> queryResults,
			long userId)
			where T : class, IReportBlock
		{
			if (reportBlock == null) throw new ArgumentNullException(nameof(reportBlock));

			var vizualizer = _reportBlockVizualizerFabric.Get((dynamic) reportBlock);

			vizualizer.Vizualize(textWriter, reportBlock, parameterValues, queryResults, userId);
		}
	}
}