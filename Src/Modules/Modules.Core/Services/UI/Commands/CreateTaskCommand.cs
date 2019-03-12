namespace Modules.Core.Services.UI.Commands
{
	using Common.Command;

	internal sealed class CreateTaskCommand: ICommand
	{
		public long ProjectId { get; set; }

		public string Repository { get; set; }

		public long CreatedTaskId { get; set; }
	}
}