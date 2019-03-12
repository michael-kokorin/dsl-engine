namespace Infrastructure.Reports
{
	using System.Xml.Serialization;

	/// <summary>
	/// AI SSDL report definition rule
	/// </summary>
	public sealed class ReportRule
	{
		/// <summary>
		/// Gets or sets the report display title.
		/// </summary>
		/// <value>
		/// The report title.
		/// </value>
		[XmlAttribute]
		public string ReportTitle { get; set; }

		/// <summary>
		/// Gets or sets the report parameters array.
		/// </summary>
		/// <value>
		/// The report parameters array.
		/// </value>
		[XmlElement]
		public ReportParameter[] Parameters { get; set; }

		/// <summary>
		/// Gets or sets the report query links.
		/// </summary>
		/// <value>
		/// The query links array.
		/// </value>
		[XmlElement]
		public IReportQuery[] QueryLinks { get; set; }

		/// <summary>
		/// Gets or sets the report template.
		/// </summary>
		/// <value>
		/// The report template.
		/// </value>
		[XmlElement]
		public ReportTemplate Template { get; set; }
	}
}