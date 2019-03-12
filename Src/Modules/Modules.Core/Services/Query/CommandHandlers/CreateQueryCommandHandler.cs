namespace Modules.Core.Services.Query.CommandHandlers
{
	using System;

	using JetBrains.Annotations;

	using Common.Command;
	using Infrastructure.Query;
	using Modules.Core.Services.Query.Commands;

	[UsedImplicitly]
	internal sealed class CreateQueryCommandHandler : ICommandHandler<CreateQueryCommand>
	{
		private readonly IQueryStorage _queryStorage;

		public CreateQueryCommandHandler([NotNull] IQueryStorage queryStorage)
		{
			if (queryStorage == null) throw new ArgumentNullException(nameof(queryStorage));

			_queryStorage = queryStorage;
		}

		public void Process(CreateQueryCommand command)
		{
			if (command == null)
				throw new ArgumentNullException(nameof(command));

			_queryStorage.Create(command.ProjectId, command.Name);
		}
	}
}