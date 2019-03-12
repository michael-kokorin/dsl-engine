namespace Infrastructure.Reports
{
	using System.Collections.Generic;
	using System.Xml.Serialization;

	using Repository.Context;

	[XmlRoot]
	public sealed class ReportBundle
	{
		[XmlElement]
		public string BodyHtml { get; set; }

		[XmlAttribute]
		public string Title { get; set; }

		[XmlElement]
		public Reports Report { get; set; }

		[XmlArray]
		public IReadOnlyDictionary<string, object> ParameterValues { get; set; }

		[XmlArray]
		public IReadOnlyCollection<ReportQueryResult> QueryResults { get; set; }

		[XmlElement]
		public ReportRule Rule { get; set; }

		[XmlAttribute]
		public long TargetUserId { get; set; }
	}

	public sealed class ReportQueryResult
	{
		[XmlAttribute]
		public string Key { get; set; }

		[XmlArray]
		public Engines.Query.Result.QueryResult Result { get; set; }
	}
}