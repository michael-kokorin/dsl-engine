namespace Plugins.Rtc.It.Client.Entity
{
	using System.Collections.Generic;

	using RestSharp.Deserializers;

	public sealed class Identity
	{
		[DeserializeAs(Name = "userId")]
		public string UserId { get; set; }

		[DeserializeAs(Name = "roles")]
		public List<string> Roles { get; set; }
	}
}