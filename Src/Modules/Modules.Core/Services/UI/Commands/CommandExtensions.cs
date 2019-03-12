namespace Modules.Core.Services.UI.Commands
{
	using Modules.Core.Contracts.UI.Dto;

	internal static class CommandExtensions
	{
		public static CreateProjectCommand ToCommand(this NewProjectDto dto) => new CreateProjectCommand
		{
			Alias = dto.Alias,
			DefaultBranchName = dto.DefaultBranchName,
			Name = dto.Name
		};

		public static CreateTaskCommand ToCommand(this CreateTaskDto dto) => new CreateTaskCommand
		{
			ProjectId = dto.ProjectId,
			Repository = dto.Repository
		};
	}
}