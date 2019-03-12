namespace Plugins.Rtc.It.Client.Entity
{
	using RestSharp.Deserializers;

	public sealed class Category
	{
		[DeserializeAs(Name = "rtc_cm:archived")]
		public bool Archived { get; set; }

		[DeserializeAs(Name = "rtc_cm:depth")]
		public int Depth { get; set; }

		[DeserializeAs(Name = "dc:description")]
		public string Description { get; set; }

		[DeserializeAs(Name = "rtc_cm:hierarchicalName")]
		public string HierarchicalName { get; set; }

		[DeserializeAs(Name = "rtc_cm:projectArea")]
		public ResourceLink ProjectArea { get; set; }

		[DeserializeAs(Name = "rdf:resource")]
		public string Resource { get; set; }

		[DeserializeAs(Name = "dc:title")]
		public string Title { get; set; }
	}
}