namespace PT.Sdl.Infrastructure.Plugins.Agent.Contracts
{
	using System;
	using System.Runtime.Serialization;

	/// <summary>
	///   Represents DTO for task scanning result.
	/// </summary>
	[DataContract(Name = "ScanTaskResult")]
	public sealed class ScanTaskResultDto
	{
		/// <summary>
		///   Gets or sets the identifier.
		/// </summary>
		/// <value>
		///   The identifier.
		/// </value>
		[DataMember(IsRequired = true)]
		public long Id { get; set; }

		/// <summary>
		///   Gets or sets the status.
		/// </summary>
		/// <value>
		///   The status.
		/// </value>
		[DataMember(IsRequired = true)]
		public ScanStatus Status { get; set; }

		/// <summary>
		///   Gets or sets the result path.
		/// </summary>
		/// <value>
		///   The result path.
		/// </value>
		[DataMember(IsRequired = true)]
		public string ResultPath { get; set; }

		/// <summary>
		///   Gets or sets the log path.
		/// </summary>
		/// <value>
		///   The log path.
		/// </value>
		[DataMember(IsRequired = true)]
		public string LogPath { get; set; }

		/// <summary>
		///   Gets or sets the analyzed files.
		/// </summary>
		/// <value>
		///   The analyzed files.
		/// </value>
		[DataMember(IsRequired = true)]
		public string AnalyzedFiles { get; set; }

		/// <summary>
		///   Gets or sets the size of the analyzed source code.
		/// </summary>
		/// <value>
		///   The size of the analyzed source code.
		/// </value>
		[DataMember(IsRequired = true)]
		public long AnalyzedSizeInBytes { get; set; }

		/// <summary>
		///   Gets or sets the count of analyzed lines.
		/// </summary>
		/// <value>
		///   The count of analyzed lines.
		/// </value>
		[DataMember(IsRequired = true)]
		public long AnalyzedLinesCount { get; set; }

		/// <summary>
		///   Gets or sets the core run time.
		/// </summary>
		/// <value>
		///   The core run time.
		/// </value>
		[DataMember(IsRequired = true)]
		public TimeSpan CoreRunTime { get; set; }
	}
}