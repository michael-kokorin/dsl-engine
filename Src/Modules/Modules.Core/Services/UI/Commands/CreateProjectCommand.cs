namespace Modules.Core.Services.UI.Commands
{
	using Common.Command;

	internal sealed class CreateProjectCommand : ICommand
	{
		public string Alias { get; set; }

		public string DefaultBranchName { get; set; }

		public string Description { get; set; }

		public string Name { get; set; }

		public long CreatedProjectId { get; set; }
	}
}