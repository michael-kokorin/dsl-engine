namespace Infrastructure.Reports.Blocks
{
	using System.Collections.Generic;
	using System.Web.UI;

	public interface IReportBlockVizualizationManager
	{
		void Vizualize<T>(
			HtmlTextWriter textWriter,
			T reportBlock,
			IReadOnlyDictionary<string, object> parameterValues,
			IReadOnlyCollection<ReportQueryResult> queryResults,
			long userId)
			where T : class, IReportBlock;
	}
}