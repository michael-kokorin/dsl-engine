namespace Infrastructure.Reports.Blocks.Image
{
	using System.Xml.Serialization;

	public sealed class ImageReportBlock : IReportBlock
	{
		/// <summary>
		/// Gets or sets the image alternative text
		/// </summary>
		/// <value>
		/// The image alternative text
		/// </value>
		[XmlAttribute]
		public string Alt { get; set; }

		/// <summary>
		/// Gets or sets the image height. May be represented in px of %
		/// </summary>
		/// <value>
		/// The image height.
		/// </value>
		[XmlAttribute]
		public string Height { get; set; }

		/// <summary>
		/// Gets or sets the report block identifier.
		/// </summary>
		/// <value>
		/// The report block identifier.
		/// </value>
		[XmlAttribute]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the image source.
		/// </summary>
		/// <value>
		/// The image source.
		/// </value>
		[XmlElement]
		public string Source { get; set; }

		/// <summary>
		/// Gets or sets the image width. May be represented in px or %
		/// </summary>
		/// <value>
		/// The image width.
		/// </value>
		[XmlAttribute]
		public string Width { get; set; }

		public ImageReportBlock()
		{
			Alt = "Image";

			Height = "100%";
			Width = "100%";
		}
	}
}