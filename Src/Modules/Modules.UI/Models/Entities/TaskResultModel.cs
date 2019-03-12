namespace Modules.UI.Models.Entities
{
	using System.ComponentModel.DataAnnotations;

	using Modules.UI.Resources;

	public sealed class TaskResultModel
	{
		[Display(ResourceType = typeof(Resources), Name = "TaskResultModel_AdditionalExploitConditions_Additional_exploit_conditions")]
		public string AdditionalExploitConditions { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "TaskResultModel_ExploitGraph_Exploit_graph")]
		public string ExploitGraph { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "TaskResultModel_Description_Description")]
		public string Description { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "TaskResultModel_File_File_name")]
		public string File { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "TaskResultModel_Function_Function_name")]
		public string Function { get; set; }

		// ReSharper disable once LocalizableElement
		[Display(Name = "Id")]
		public long Id { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "TaskResultModel_IssueNumber_Issue_number")]
		public string IssueNumber { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "TaskResultModel_IssueUrl_Issue_link")]
		public string IssueUrl { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "TaskResultModel_LineNumber_Line_number")]
		public int LineNumber { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "TaskResultModel_LinePosition_Position_number")]
		public int LinePosition { get; set; }

		public string Place { get; set; }

		public string SourceFile { get; set; }

		public string Rawline { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "TaskResultModel_Type_Vulnerability_type")]
		public string Type { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "TaskResultModel_TypeShort_Vulnerability_type_short")]
		public string TypeShort { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "TaskResultModel_Message_Message")]
		public string Message { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "TaskResultModel_TaskId_Task_Id")]
		public long TaskId { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "TaskResultModel_SeverityType_Severity_type")]
		public int SeverityType { get; set; }
	}
}