namespace Modules.UI.Models.Views
{
	using Modules.Core.Contracts.Dto;
	using Modules.Core.Contracts.UI.Dto;
	using Modules.UI.Models.Views.Task;

	internal static class ViewModelExtensions
	{
		public static CreateTaskDto ToDto(this CreateTaskViewModel model) =>
			new CreateTaskDto
			{
				ProjectId = model.ProjectId,
				Repository = model.Repository
			};

		public static ReferenceItemModel ToModel(this ReferenceItemDto referenceItemDto) =>
			new ReferenceItemModel(referenceItemDto.Value, referenceItemDto.Text);
	}
}