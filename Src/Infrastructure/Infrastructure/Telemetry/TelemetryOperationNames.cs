namespace Infrastructure.Telemetry
{
	/// <summary>
	/// Available operations name for telemetry
	/// </summary>
	public static class TelemetryOperationNames
	{
		/// <summary>
		/// Project telemetry operations
		/// </summary>
		public static class Prroject
		{
			/// <summary>
			/// The create project operation
			/// </summary>
			public const string Create = "create";

			/// <summary>
			/// The update project operation
			/// </summary>
			public const string Update = "update";
		}

		/// <summary>
		/// Query operations for telemetry
		/// </summary>
		public static class Query
		{
			/// <summary>
			/// Query creation operation name
			/// </summary>
			public const string Create = "create";

			/// <summary>
			/// Query execution operation name
			/// </summary>
			public const string Execute = "execute";

			/// <summary>
			/// Query changing operation name
			/// </summary>
			public const string Update = "update";
		}

		public static class Report
		{
			/// <summary>
			/// The execute
			/// </summary>
			public const string Generate = "generate";

			/// <summary>
			/// Report creation operation
			/// </summary>
			public const string Create = "create";

			/// <summary>
			/// Report deletion operation
			/// </summary>
			public const string Delete = "delete";

			/// <summary>
			/// Report changing operation
			/// </summary>
			public const string Update = "update";
		}

		public static class Task
		{
			/// <summary>
			/// Task creation operations
			/// </summary>
			public const string Create = "create";

			/// <summary>
			/// Stop task execution operation
			/// </summary>
			public const string Stop = "stop";

			/// <summary>
			/// Task updateion operation
			/// </summary>
			public const string Update = "update";
		}

		/// <summary>
		/// VCS plugin operations
		/// </summary>
		public static class VcsPlugin
		{
			/// <summary>
			/// Commit sources to VCS
			/// </summary>
			public const string Commit = "commit";

			/// <summary>
			/// Create branch in VCS
			/// </summary>
			public const string CreateBranch = "create-branch";

			/// <summary>
			/// Download sources from VCS
			/// </summary>
			public const string Checkout = "checkout";
		}

		/// <summary>
		/// Issue tracker plugin operations
		/// </summary>
		public static class ItPlugin
		{
			/// <summary>
			/// Create issue operation
			/// </summary>
			public const string Create = "create";

			/// <summary>
			/// Update issue operation
			/// </summary>
			public const string Update = "update";
		}
	}
}