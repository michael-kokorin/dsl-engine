namespace Infrastructure.Reports.Blocks.Html
{
	using System.Xml.Serialization;

	public sealed class HtmlReportBlock : IReportBlock
	{
		/// <summary>
		/// Gets or sets the report block identifier.
		/// </summary>
		/// <value>
		/// The report block identifier.
		/// </value>
		[XmlAttribute]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the HTML template.
		/// </summary>
		/// <value>
		/// The template.
		/// </value>
		[XmlAttribute]
		public string Template { get; set; }
	}
}