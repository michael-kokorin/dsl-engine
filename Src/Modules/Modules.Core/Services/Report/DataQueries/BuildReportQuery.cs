namespace Modules.Core.Services.Report.DataQueries
{
	using System.Collections.Generic;

	using Common.Enums;
	using Common.Query;

	internal sealed class BuildReportQuery : IDataQuery
	{
		public readonly long ReportId;

		public IReadOnlyDictionary<string, string> Parameters;

		public ReportFileType ReportFileType;

		public BuildReportQuery(long reportId, IReadOnlyDictionary<string, string> parameters, ReportFileType reportFileType)
		{
			ReportId = reportId;

			Parameters = parameters;

			ReportFileType = reportFileType;
		}
	}
}