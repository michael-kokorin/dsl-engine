namespace Modules.Core.Services.Query.CommandHandlers
{
	using System;

	using JetBrains.Annotations;

	using Common.Command;
	using Common.Enums;
	using Common.Extensions;
	using Infrastructure.Engines.Dsl.Query;
	using Infrastructure.Query;
	using Modules.Core.Services.Query.Commands;

	[UsedImplicitly]
	internal sealed class UpdateQueryCommandHandler : ICommandHandler<UpdateQueryCommand>
	{
		private readonly IQueryStorage _queryStorage;

		public UpdateQueryCommandHandler([NotNull] IQueryStorage queryStorage)
		{
			if (queryStorage == null) throw new ArgumentNullException(nameof(queryStorage));

			_queryStorage = queryStorage;
		}

		public void Process(UpdateQueryCommand command)
		{
			if (command == null)
				throw new ArgumentNullException(nameof(command));

			if (command.Query == null)
				throw new ArgumentNullException(nameof(command.Query));

			var query = command.Query;

			if (query.Model != null)
			{
				var model = query.Model.FromJson<DslDataQuery>();

				_queryStorage.Update(query.Id,
					model,
					query.Name,
					query.Comment,
					(QueryPrivacyType) query.Privacy,
					(QueryVisibilityType) query.Visibility);

				return;
			}

			_queryStorage.Update(query.Id,
				query.Query,
				query.Name,
				query.Comment,
				(QueryPrivacyType) query.Privacy,
				(QueryVisibilityType) query.Visibility);
		}
	}
}