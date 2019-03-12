namespace Infrastructure.Reports.Blocks.QueryScope
{
	using System.Xml.Serialization;

	public sealed class QueryScopeParameter
	{
		[XmlAttribute]
		public string Key { get; set; }

		[XmlAttribute]
		public string Template { get; set; }
	}

	public sealed class QueryScopeReportBlock: IReportBlock
	{
		[XmlElement]
		public IReportBlock Child { get; set; }

		[XmlAttribute]
		public string Id { get; set; }

		[XmlArray]
		public QueryScopeParameter[] Parameters { get; set; }

		[XmlElement]
		public IReportQuery Query { get; set; }
	}
}