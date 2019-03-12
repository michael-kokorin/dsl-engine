namespace Infrastructure.Telemetry.Entities
{
	using Repository.Context;

	public sealed class ItPluginInfo
	{
		public Plugins Plugins { get; set; }

		public long? TaskId { get; set; }
	}
}