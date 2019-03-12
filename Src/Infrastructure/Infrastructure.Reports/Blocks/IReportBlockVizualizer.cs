namespace Infrastructure.Reports.Blocks
{
	using System.Collections.Generic;
	using System.Web.UI;

	public interface IReportBlockVizualizer<in T>
		where T : class, IReportBlock
	{
		void Vizualize(
			HtmlTextWriter htmlTextWriter,
			T block,
			IReadOnlyDictionary<string, object> parameterValues,
			IReadOnlyCollection<ReportQueryResult> queryResults,
			long userId);
	}
}