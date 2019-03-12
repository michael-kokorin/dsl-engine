namespace Infrastructure.Reports.Blocks.Chart
{
	using System.Xml.Serialization;

	/// <summary>
	/// Chart report block
	/// 
	/// Supported next types of chart: lines, bar
	/// </summary>
	public sealed class ChartReportBlock : IQuaryableReportBlock
	{
		/// <summary>
		/// Gets or sets the array of column sources.
		/// </summary>
		/// <value>
		/// The chart columns.
		/// </value>
		[XmlElement]
		public ChartColumn[] Columns { get; set; }

		[XmlAttribute]
		public string Id { get; set; }


		/// <summary>
		/// Gets or sets the chart labels source.
		/// </summary>
		/// <value>
		/// The label.
		/// </value>
		[XmlElement]
		public ChartLabel Label { get; set; }


		/// <summary>
		/// Gets or sets the type of chart.
		/// 
		/// Default: line
		/// </summary>
		/// <value>
		/// The type of chart.
		/// </value>
		[XmlAttribute]
		public ChartType Type { get; set; }


		/// <summary>
		/// Gets or sets the source query key.
		/// </summary>
		/// <value>
		/// The query key.
		/// </value>
		[XmlAttribute]
		public string QueryKey { get; set; }


		/// <summary>
		/// Gets or sets the chart height, mm.
		/// 
		/// Default: 50 mm
		/// </summary>
		/// <value>
		/// The height, mm.
		/// </value>
		[XmlAttribute]
		public int HeightPx { get; set; }

		public ChartReportBlock()
		{
			HeightPx = 50;

			Type = ChartType.Line;
		}
	}
}