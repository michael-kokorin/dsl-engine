namespace Modules.Core.Contracts.UI.Dto
{
	using System.Runtime.Serialization;

	[DataContract(Name = "TaskResult")]
	public sealed class TaskResultDto
	{
		[DataMember]
		public string AdditionalExploitConditions { get; set; }

		[DataMember]
		public string Description { get; set; }

		[DataMember]
		public string ExploitGraph { get; set; }

		[DataMember]
		public string File { get; set; }

		[DataMember]
		public string Function { get; set; }

		[DataMember]
		public long Id { get; set; }

		[DataMember]
		public string IssueNumber { get; set; }

		[DataMember]
		public string IssueUrl { get; set; }

		[DataMember]
		public int LineNumber { get; set; }

		[DataMember]
		public int LinePosition { get; set; }

		[DataMember]
		public string Message { get; set; }

		[DataMember]
		public string Place { get; set; }

		[DataMember]
		public long ProjectId { get; set; }

		[DataMember]
		public string Rawline { get; set; }

		[DataMember]
		public int SeverityType { get; set; }

		[DataMember]
		public string SourceFile { get; set; }

		[DataMember]
		public long TaskId { get; set; }

		[DataMember]
		public string Type { get; set; }

		[DataMember]
		public string TypeShort { get; set; }
	}
}