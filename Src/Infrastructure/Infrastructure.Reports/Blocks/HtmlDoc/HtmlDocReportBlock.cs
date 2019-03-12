namespace Infrastructure.Reports.Blocks.HtmlDoc
{
	using System.Xml.Serialization;

	public sealed class HtmlDocReportBlock : IReportBlock
	{
		/// <summary>
		/// Gets or sets the width.
		/// </summary>
		/// <value>
		/// The width.
		/// </value>
		[XmlAttribute]
		public string Width { get; set; }

		/// <summary>
		/// Gets or sets the child.
		/// </summary>
		/// <value>
		/// The child.
		/// </value>
		[XmlElement]
		public IReportBlock Child { get; set; }

		[XmlAttribute]
		public string Id { get; set; }

		[XmlAttribute]
		public bool WithHeader { get; set; }

		public HtmlDocReportBlock(bool withHtmlHeader = false)
		{
			Width = "100%";

			WithHeader = withHtmlHeader;
		}
	}
}