namespace Infrastructure.Reports.Blocks
{
	using System.Xml.Serialization;

	public abstract class ReportBlock : IReportBlock
	{
		/// <summary>
		/// Gets or sets the color of the background.
		/// </summary>
		/// <value>
		/// The color of the background.
		/// </value>
		[XmlElement]
		public ReportItemColor BackgroundColor { get; set; }

		/// <summary>
		/// Gets or sets the margin bottom px.
		/// </summary>
		/// <value>
		/// The margin bottom px.
		/// </value>
		[XmlAttribute]
		public int MarginBottomPx { get; set; }

		/// <summary>
		/// Gets or sets the margin left px.
		/// </summary>
		/// <value>
		/// The margin left px.
		/// </value>
		[XmlAttribute]
		public int MarginLeftPx { get; set; }

		/// <summary>
		/// Gets or sets the margin right px.
		/// </summary>
		/// <value>
		/// The margin right px.
		/// </value>
		[XmlAttribute]
		public int MarginRightPx { get; set; }

		/// <summary>
		/// Gets or sets the margin top px.
		/// </summary>
		/// <value>
		/// The margin top px.
		/// </value>
		[XmlAttribute]
		public int MarginTopPx { get; set; }

		/// <summary>
		/// Gets or sets the padding bottom px.
		/// </summary>
		/// <value>
		/// The padding bottom px.
		/// </value>
		[XmlAttribute]
		public int PaddingBottomPx { get; set; }

		/// <summary>
		/// Gets or sets the padding left px.
		/// </summary>
		/// <value>
		/// The padding left px.
		/// </value>
		[XmlAttribute]
		public int PaddingLeftPx { get; set; }

		/// <summary>
		/// Gets or sets the padding right px.
		/// </summary>
		/// <value>
		/// The padding right px.
		/// </value>
		[XmlAttribute]
		public int PaddingRightPx { get; set; }

		/// <summary>
		/// Gets or sets the padding top px.
		/// </summary>
		/// <value>
		/// The padding top px.
		/// </value>
		[XmlAttribute]
		public int PaddingTopPx { get; set; }

		/// <summary>
		/// Gets or sets the report block identifier.
		/// </summary>
		/// <value>
		/// The report block identifier.
		/// </value>
		[XmlAttribute]
		public virtual string Id { get; set; }

		protected ReportBlock()
		{
			BackgroundColor = new ReportItemColor(255, 255, 255);
		}
	}
}