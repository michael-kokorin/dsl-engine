namespace Plugins.Rtc.It.Client.Entity
{
	using RestSharp.Deserializers;

	public sealed class WorkItemType
	{
		[DeserializeAs(Name = "rtc_cm:category")]
		public string Category { get; set; }

		[DeserializeAs(Name = "rtc_cm:projectArea")]
		public ResourceLink ProjectArea { get; set; }

		[DeserializeAs(Name = "dc:identifier")]
		public string Identifier { get; set; }

		[DeserializeAs(Name = "rdf:resource")]
		public string Resource { get; set; }

		[DeserializeAs(Name = "dc:title")]
		public string Title { get; set; }
	}
}