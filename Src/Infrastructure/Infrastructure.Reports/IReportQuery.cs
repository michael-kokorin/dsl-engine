namespace Infrastructure.Reports
{
	/// <summary>
	/// Default interface for Report query definition
	/// </summary>
	public interface IReportQuery
	{
		/// <summary>
		/// Gets the report query key.
		/// </summary>
		/// <value>
		/// The report query key.
		/// </value>
		string Key { get; }
	}
}