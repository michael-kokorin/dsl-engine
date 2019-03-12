namespace Infrastructure.Reports
{
	using System.Xml.Serialization;

	/// <summary>
	/// Report query definition
	/// </summary>
	/// <seealso cref="IReportQuery" />
	public sealed class ReportQuery: IReportQuery
	{
		/// <summary>
		/// Gets the report query key.
		/// </summary>
		/// <value>
		/// The report query key.
		/// </value>
		[XmlAttribute]
		public string Key { get; set; }

		/// <summary>
		/// Gets or sets the plain query DSL text.
		/// </summary>
		/// <value>
		/// The query DSL text.
		/// </value>
		[XmlAttribute]
		public string Text { get; set; }
	}
}