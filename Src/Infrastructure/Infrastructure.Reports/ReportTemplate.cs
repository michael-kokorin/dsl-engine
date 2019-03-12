namespace Infrastructure.Reports
{
	using System.Xml.Serialization;

	using Infrastructure.Reports.Blocks;

	/// <summary>
	/// Report template definition
	/// </summary>
	public sealed class ReportTemplate
	{
		/// <summary>
		/// Gets or sets the report template root block.
		/// </summary>
		/// <value>
		/// The root report block.
		/// </value>
		[XmlAttribute]
		public IReportBlock Root { get; set; }
	}
}