namespace Plugins.Rtc.It.Client.Entity
{
	using System.Collections.Generic;

	using RestSharp.Deserializers;

	public sealed class EntityList<T>
	{
		[DeserializeAs(Name = "oslc_cm:totalCount")]
		public int Total { get; set; }

		[DeserializeAs(Name = "oslc_cm:results")]
		public List<T> Entities { get; set; }
	}
}