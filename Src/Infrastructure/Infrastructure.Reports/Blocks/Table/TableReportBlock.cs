namespace Infrastructure.Reports.Blocks.Table
{
	using System.Xml.Serialization;

	using Infrastructure.Reports.Blocks.Label;

	/// <summary>
	/// Table report block
	/// </summary>
	public sealed class TableReportBlock : IQuaryableReportBlock
	{
		/// <summary>
		/// Gets or sets the border thickness (px).
		/// 
		/// By default, equals 1px.
		/// </summary>
		/// <value>
		/// The border thickness.
		/// </value>
		[XmlAttribute]
		public int BorderPx { get; set; }

		/// <summary>
		/// Gets or sets the report block identifier.
		/// </summary>
		/// <value>
		/// The report block identifier.
		/// </value>
		[XmlAttribute]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the query key.
		/// </summary>
		/// <value>
		/// The query key.
		/// </value>
		[XmlAttribute]
		public string QueryKey { get; set; }

		/// <summary>
		/// Gets or sets the header label.
		/// </summary>
		/// <value>
		/// The header label.
		/// </value>
		[XmlElement]
		public LabelReportBlock HeaderLabel { get; set; }

		/// <summary>
		/// Gets or sets the body label.
		/// </summary>
		/// <value>
		/// The body label.
		/// </value>
		[XmlElement]
		public LabelReportBlock BodyLabel { get; set; }

		public TableReportBlock()
		{
			BorderPx = 1;

			HeaderLabel = new LabelReportBlock
			{
				FontStyle = new LabelFontStyle("Tahoma", 14, true, false),
				HorizontalAlign = LabelHorizontalAlign.Center,
				Text = $"${TableReportBlockParameters.ColumnHeader}$",
				VerticalAlign = LabelVerticalalign.Middle
			};

			BodyLabel = new LabelReportBlock
			{
				FontStyle = new LabelFontStyle("Tahoma", 14, false, false),
				HorizontalAlign = LabelHorizontalAlign.Left,
				Text = $"${TableReportBlockParameters.RowColumnItem}$",
				VerticalAlign = LabelVerticalalign.Middle
			};
		}
	}
}