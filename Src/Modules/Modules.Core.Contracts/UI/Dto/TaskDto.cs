namespace Modules.Core.Contracts.UI.Dto
{
	using System;
	using System.Runtime.Serialization;

	[DataContract(Name = "Task")]
	public sealed class TaskDto
	{
		[DataMember]
		public long? AnalyzedLinesCount { get; set; }

		[DataMember]
		public long? AnalyzedSizeKb { get; set; }

		[DataMember]
		public DateTime Created { get; set; }

		[DataMember]
		public string CreatedBy { get; set; }

		[DataMember]
		public DateTime? Finished { get; set; }

		[DataMember]
		public string FolderPath { get; set; }

		[DataMember]
		public long? FolderSizeKb { get; set; }

		[DataMember]
		public long Id { get; set; }

		[DataMember]
		public long? ItPluginId { get; set; }

		[DataMember]
		public DateTime Modified { get; set; }

		[DataMember]
		public string ModifiedBy { get; set; }

		[DataMember]
		public long ProjectId { get; set; }

		[DataMember]
		public string Repository { get; set; }

		[DataMember]
		public string ResolutionMessage { get; set; }

		[DataMember]
		public TaskResolutionStatusDto ResolutionStatus { get; set; }

		[DataMember]
		public long? ScanCoreWorkTimeSec { get; set; }

		[DataMember]
		public SdlPolicyStatusDto SdlPolicyStatus { get; set; }

		[DataMember]
		public int? SeverityHighCount { get; set; }

		[DataMember]
		public int? SeverityLowCount { get; set; }

		[DataMember]
		public int? SeverityMediumCount { get; set; }

		[DataMember]
		public TaskStatusDto Status { get; set; }

		[DataMember]
		public long? VcsPluginId { get; set; }
	}
}