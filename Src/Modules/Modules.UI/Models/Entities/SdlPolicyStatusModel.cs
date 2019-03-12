namespace Modules.UI.Models.Entities
{
	using System.ComponentModel.DataAnnotations;

	using Modules.UI.Resources;

	public enum SdlPolicyStatusModel
	{
		[Display(ResourceType = typeof(Resources), Name = "SdlPolicyStatusModel_Unknown_Unknown")]
		Unknown = 0,

		[Display(ResourceType = typeof(Resources), Name = "SdlPolicyStatusModel_Success_Success")]
		Success = 1,

		[Display(ResourceType = typeof(Resources), Name = "SdlPolicyStatusModel_Failed_Failed")]
		Failed = 2,

		[Display(ResourceType = typeof(Resources), Name = "SdlPolicyStatusModel_Error")]
		Error = 3
	}
}