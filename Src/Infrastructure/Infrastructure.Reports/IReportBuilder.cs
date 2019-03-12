namespace Infrastructure.Reports
{
	using System.Collections.Generic;

	using Common.Enums;

	/// <summary>
	/// Report builder
	/// </summary>
	public interface IReportBuilder
	{
		/// <summary>
		/// Builds the specified report.
		/// </summary>
		/// <param name="reportId">The report identifier.</param>
		/// <param name="userId">The user identifier.</param>
		/// <param name="parameters">The report parameters.</param>
		/// <param name="reportFileType">Type of the report output file.</param>
		/// <returns>Report file</returns>
		ReportFile Build(long reportId, long userId, IReadOnlyDictionary<string, object> parameters, ReportFileType reportFileType);
	}
}