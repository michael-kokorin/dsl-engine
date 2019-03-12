namespace Modules.UI.Models.Entities
{
	using System.ComponentModel.DataAnnotations;

	using Modules.UI.Resources;

	public enum TaskStatusModel
	{
		[Display(ResourceType = typeof(Resources), Name = "TaskStatusModel_New_New_task")]
		New = 0,

		[Display(ResourceType = typeof(Resources), Name = "TaskStatusModel_PreProcessing_Pre_processing")]
		PreProcessing = 1,

		[Display(ResourceType = typeof(Resources), Name = "TaskStatusModel_ReadyToStan_Ready_to_scan")]
		ReadyToStan = 2,

		[Display(ResourceType = typeof(Resources), Name = "TaskStatusModel_Scanning_Scanning")]
		Scanning = 3,

		[Display(ResourceType = typeof(Resources), Name = "TaskStatusModel_ReadyToPostProcessing")]
		ReadyToPostProcessing = 5,

		[Display(ResourceType = typeof(Resources), Name = "TaskStatusModel_PostProcessing_Post_processing")]
		PostProcessing = 6,

		[Display(ResourceType = typeof(Resources), Name = "TaskStatusModel_Done_Task_done")]
		Done = 7
	}
}