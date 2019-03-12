namespace Infrastructure.Telemetry.Entities
{
	using Repository.Context;

	public sealed class VcsPluginInfo
	{
		public long? CommittedSourcesSize { get; set; }

		public string CreatedBranchName { get; set; }

		public long? DownloadedSourcesSize { get; set; }

		public Plugins Plugin { get; set; }

		public long? TaskId { get; set; }
	}
}