namespace Plugins.Rtc.It.Client.Entity
{
	using System;

	using RestSharp.Deserializers;

	public sealed class Issue
	{
		[DeserializeAs(Name = "dc:created")]
		public DateTime Created { get; set; }

		[DeserializeAs(Name = "dc:creator")]
		public ResourceLink Creator { get; set; }

		[DeserializeAs(Name = "dc:description")]
		public string Description { get; set; }

		[DeserializeAs(Name = "dc:identifier")]
		public long Id { get; set; }

		[DeserializeAs(Name = "rtc_cm:resolved")]
		public DateTime? Resolved { get; set; }

		[DeserializeAs(Name = "rtc_cm:state")]
		public ResourceLink State { get; set; }

		[DeserializeAs(Name = "dc:title")]
		public string Title { get; set; }
	}
}