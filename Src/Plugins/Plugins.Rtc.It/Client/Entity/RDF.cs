namespace Plugins.Rtc.It.Client.Entity
{
	using RestSharp.Deserializers;

	[DeserializeAs(Name = "rdf:RDF")]
	public sealed class Rdf<T>
	{
		[DeserializeAs(Name = "rdf:Description")]
		public T Description { get; set; }
	}
}