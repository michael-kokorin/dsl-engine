namespace Modules.Core.Services.Query.Commands
{
	using Common.Command;

	internal sealed class CreateQueryCommand : ICommand
	{
		public readonly string Name;

		public readonly long ProjectId;

		public CreateQueryCommand(string name, long projectId)
		{
			Name = name;

			ProjectId = projectId;
		}
	}
}