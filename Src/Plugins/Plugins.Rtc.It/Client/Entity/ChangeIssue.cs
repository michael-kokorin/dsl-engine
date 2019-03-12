namespace Plugins.Rtc.It.Client.Entity
{
	using Newtonsoft.Json;

	public sealed class ChangeIssue
	{
		[JsonProperty("dc:description")]
		public string Description { get; set; }

		[JsonProperty("rtc_cm:filedAgainst")]
		public ResourceLink FiledAgainst { get; set; }

		[JsonProperty("rtc_cm:state")]
		public ResourceLink State { get; set; }

		[JsonProperty("dc:title")]
		public string Title { get; set; }

		[JsonProperty("dc:type")]
		public ResourceLink Type { get; set; }
	}
}