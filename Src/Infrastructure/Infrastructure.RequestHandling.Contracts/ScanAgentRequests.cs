namespace Infrastructure.RequestHandling.Contracts
{
	public static class ScanAgentRequests
	{
		public const string CheckVersion = "CheckVersion";

		public const string GetNextTask = "GetNextTask";

		public const string IsTaskCancelled = "IsTaskCancelled";

		public const string SendResults = "SendResults";
	}
}