namespace Plugins.Rtc.It.Client.Entity
{
	using Newtonsoft.Json;

	using RestSharp.Deserializers;

	public sealed class ResourceLink
	{
		[DeserializeAs(Name = "rdf:resource")]
		[JsonProperty("rdf:resource")]
		public string Resource { get; set; }
	}
}