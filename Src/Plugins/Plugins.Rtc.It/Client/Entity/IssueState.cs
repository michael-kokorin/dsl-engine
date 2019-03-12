namespace Plugins.Rtc.It.Client.Entity
{
	using RestSharp.Deserializers;

	public sealed class IssueState
	{
		[DeserializeAs(Name = "rtc_cm:group")]
		public string Group { get; set; }

		[DeserializeAs(Name = "dc:identifier")]
		public string Identifier { get; set; }

		[DeserializeAs(Name = "rdf:resource")]
		public string Resource { get; set; }

		[DeserializeAs(Name = "dc:title")]
		public string Title { get; set; }
	}
}