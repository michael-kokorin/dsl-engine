namespace Infrastructure.Reports
{
	/// <summary>
	/// Gets report folder path
	/// </summary>
	internal interface IReportFolderPathProvider
	{
		/// <summary>
		/// Gets the report folder path.
		/// </summary>
		/// <param name="reportBundle">The report bundle.</param>
		/// <param name="userId">The user identifier.</param>
		/// <returns>Report folder full path</returns>
		string GetReportFolderPath(ReportBundle reportBundle, long userId);
	}
}