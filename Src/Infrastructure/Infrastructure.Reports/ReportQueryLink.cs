namespace Infrastructure.Reports
{
	using System.Xml.Serialization;

	/// <summary>
	/// Report query link
	/// </summary>
	/// <seealso cref="IReportQuery" />
	public sealed class ReportQueryLink : IReportQuery
	{
		/// <summary>
		/// Gets the report query key.
		/// </summary>
		/// <value>
		/// The report query key.
		/// </value>
		[XmlAttribute]
		public string Key { get; set; }

		/// <summary>
		/// Gets or sets the query identifier.
		/// </summary>
		/// <value>
		/// The query identifier.
		/// </value>
		[XmlAttribute]
		public long QueryId { get; set; }

		/// <summary>
		/// Gets or sets the query parameters.
		/// </summary>
		/// <value>
		/// The query parameters.
		/// </value>
		[XmlArray]
		public ReportQueryParameter[] Parameters { get; set; }

		public ReportQueryLink(string key, long queryId)
		{
			Key = key;

			QueryId = queryId;

			Parameters = new ReportQueryParameter[0];
		}
	}
}