namespace Infrastructure.Reports.Blocks.Iterator
{
	using System.Xml.Serialization;

	public sealed class IteratorReportBlock : IQuaryableReportBlock
	{
		[XmlElement]
		public IReportBlock Child { get; set; }

		[XmlAttribute]
		public string Id { get; set; }

		[XmlAttribute]
		public string QueryKey { get; set; }
	}
}