namespace Plugins.Rtc.It.Client.Entity
{
	using RestSharp.Deserializers;

	public sealed class User
	{
		[DeserializeAs(Name = "foaf:nick")]
		public string Nick { get; set; }

		[DeserializeAs(Name = "foaf:name")]
		public string Name { get; set; }
	}
}