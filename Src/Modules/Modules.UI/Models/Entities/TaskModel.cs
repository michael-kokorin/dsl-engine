namespace Modules.UI.Models.Entities
{
	using System;
	using System.ComponentModel.DataAnnotations;

	using Modules.UI.Resources;

	public sealed class TaskModel
	{
		[Display(ResourceType = typeof(Resources), Name = "TaskModel_AnalyzedLinesCount_Analyzed_Lines")]
		public long? AnalyzedLinesCount { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "TaskModel_AnalyzedSizeKb_Analyzed_Size__Kb_")]
		public long? AnalyzedSizeKb { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "TaskModel_Created_Created")]
		public DateTime Created { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "TaskModel_CreatedBy_Created_by")]
		public string CreatedBy { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "TaskModel_Finished_Finished")]
		public DateTime? Finished { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "TaskModel_FolderPath_Folder_path")]
		public string FolderPath { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "TaskModel_FolderSizeKb_Folder_size__KB_")]
		public long? FolderSizeKb { get; set; }

		public long Id { get; set; }

		public long? ItPluginId { get; set; }

		public long ProjectId { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "TaskModel_Repository_Repository")]
		public string Repository { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "TaskModel_ScanCoreWorkingTimeSec_Scan_core_working_time__s_")]
		public long? ScanCoreWorkingTimeSec { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "ProjectModel_SdlPolicyStatus_SDL_Policy")]
		public SdlPolicyStatusModel SdlPolicyStatus { get; set; }

		public int? SeverityHighCount { get; set; }

		public int? SeverityLowCount { get; set; }

		public int? SeverityMediumCount { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "TaskModel_Status_Ststus")]
		public TaskStatusModel Status { get; set; }

		public long? VcsPluginId { get; set; }
	}
}