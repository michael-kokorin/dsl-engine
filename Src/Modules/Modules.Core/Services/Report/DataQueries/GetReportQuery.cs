namespace Modules.Core.Services.Report.DataQueries
{
	using Common.Query;

	public sealed class GetReportQuery : IDataQuery
	{
		public readonly long ReportId;

		public GetReportQuery(long reportId)
		{
			ReportId = reportId;
		}
	}
}