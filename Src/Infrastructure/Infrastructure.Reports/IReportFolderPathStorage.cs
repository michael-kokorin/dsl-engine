namespace Infrastructure.Reports
{
	/// <summary>
	/// Report folder path storage
	/// </summary>
	internal interface IReportFolderPathStorage
	{
		/// <summary>
		/// Gets the report folder path.
		/// </summary>
		/// <returns>Report folder full path</returns>
		string GetReportFolderPath();
	}
}