namespace Plugins.Rtc.It.Client.Entity
{
	using RestSharp.Deserializers;

	public sealed class Project
	{
		[DeserializeAs(Name = "dc:description")]
		public string Description { get; set; }

		[DeserializeAs(Name = "rdf:resource")]
		public string Resource { get; set; }

		[DeserializeAs(Name = "dc:title")]
		public string Title { get; set; }
	}
}