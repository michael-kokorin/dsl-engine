namespace Infrastructure.Events
{
	/// <summary>
	///     Event keys.
	/// </summary>
	public static class EventKeys
	{
		public const string ExternalSystemAction = "external-system-action";

		public const string VcsCommitted = "vcs-committed";

		/// <summary>
		///     Event keys for policy.
		/// </summary>
		public static class Policy
		{
			public const string CheckFinished = "policy-check-finished";

			public const string CheckRequired = "policy-check-required";

			public const string CheckStarted = "policy-check-started";

			public const string Error = "policy-error";

			public const string Successful = "policy-successful";

			public const string Violation = "policy-violation";
		}

		/// <summary>
		///     Event keys for scan task.
		/// </summary>
		public static class ScanTask
		{
			public const string Cancelled = "scan-task-cancelled";

			public const string Created = "scan-task-created";

			public const string Failed = "scan-task-failed";

			public const string Finished = "scan-task-finished";

			public const string PostprocessingFinished = "scan-task-postprocessing-finished";

			public const string PostprocessingStarted = "scan-task-postprocessing-started";

			public const string PreprocessingFinished = "scan-task-preprocessing-finished";

			public const string PreprocessingStarted = "scan-task-preprocessing-started";

			public const string ScanningFinished = "scan-task-scanning-finished";

			public const string ScanningStarted = "scan-task-scanning-started";
		}
	}
}