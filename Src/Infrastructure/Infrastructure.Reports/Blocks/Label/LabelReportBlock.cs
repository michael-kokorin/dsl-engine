namespace Infrastructure.Reports.Blocks.Label
{
	using System.Xml.Serialization;

	/// <summary>
	/// Label report block
	/// </summary>
	public sealed class LabelReportBlock : IReportBlock
	{
		/// <summary>
		/// Gets or sets the label text align.
		/// 
		/// Default: Left
		/// </summary>
		/// <value>
		/// The label text align.
		/// </value>
		[XmlAttribute]
		public LabelHorizontalAlign HorizontalAlign { get; set; }

		/// <summary>
		/// Gets or sets the font style.
		/// </summary>
		/// <value>
		/// The font style.
		/// </value>
		[XmlElement(IsNullable = true)]
		public LabelFontStyle FontStyle { get; set; }

		/// <summary>
		/// Gets or sets the report block identifier.
		/// </summary>
		/// <value>
		/// The report block identifier.
		/// </value>
		[XmlAttribute]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the label text.
		/// </summary>
		/// <value>
		/// The label text.
		/// </value>
		[XmlAttribute]
		public string Text { get; set; }

		/// <summary>
		/// Gets or sets the vertical align of label.
		/// 
		/// Default: Top.
		/// </summary>
		/// <value>
		/// The vertical align of label.
		/// </value>
		[XmlAttribute]
		public LabelVerticalalign VerticalAlign { get; set; }

		/// <summary>
		/// Gets or sets the label color.
		/// </summary>
		/// <value>
		/// The color.
		/// </value>
		[XmlElement]
		public ReportItemColor Color { get; set; }

		public LabelReportBlock()
		{
			Color = new ReportItemColor(0, 0, 0); // black

			FontStyle = new LabelFontStyle(14);

			HorizontalAlign = LabelHorizontalAlign.Left;

			VerticalAlign = LabelVerticalalign.Top;
		}
	}
}