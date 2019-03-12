namespace Infrastructure.Reports.Blocks.Chart
{
	using System.Xml.Serialization;

	/// <summary>
	/// Chart column source definition
	/// </summary>
	public sealed class ChartColumn
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
		/// Gets or sets the width of the border.
		/// </summary>
		/// <value>
		/// The width of the border.
		/// </value>
		[XmlAttribute]
		public int BorderWidth { get; set; }

		/// <summary>
		/// Gets or sets the column key in source query result.
		/// </summary>
		/// <value>
		/// The query result column key.
		/// </value>
		[XmlAttribute]
		public string ColumnKey { get; set; }

		/// <summary>
		/// Gets or sets the display name.
		/// </summary>
		/// <value>
		/// The display name.
		/// </value>
		[XmlAttribute]
		public string DisplayName { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="ChartColumn"/> is fill.
		/// 
		/// Default: true
		/// </summary>
		/// <value>
		///   <c>true</c> if fill; otherwise, <c>false</c>.
		/// </value>
		[XmlAttribute]
		public bool Fill { get; set; }

		/// <summary>
		/// Gets or sets the color of the line.
		/// </summary>
		/// <value>
		/// The color of the line.
		/// </value>
		[XmlElement]
		public ReportItemColor LineColor { get; set; }

		public ChartColumn()
		{
			BackgroundColor = new ReportItemColor(0.3f);

			BorderWidth = 2;

			Fill = true;

			LineColor = new ReportItemColor(BackgroundColor.Red, BackgroundColor.Green, BackgroundColor.Blue);
		}
	}
}