namespace Infrastructure.Reports.Blocks.Link
{
	using System.Xml.Serialization;

	public sealed class LinkReportBlock : IReportBlock
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
		/// Gets or sets the target.
		/// </summary>
		/// <value>
		/// The target.
		/// </value>
		[XmlAttribute]
		public string Target { get; set; }

		/// <summary>
		/// Gets or sets the i report block.
		/// </summary>
		/// <value>
		/// The i report block.
		/// </value>
		[XmlElement]
		public IReportBlock Child { get; set; }

		public LinkReportBlock(string id)
		{
			Id = id;
		}
	}
}