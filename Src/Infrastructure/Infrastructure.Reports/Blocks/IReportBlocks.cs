namespace Infrastructure.Reports.Blocks
{
	public interface IReportBlock
	{
		/// <summary>
		/// Gets or sets the report block identifier.
		/// </summary>
		/// <value>
		/// The report block identifier.
		/// </value>
		string Id { get; set; }
	}
}