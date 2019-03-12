namespace Modules.Core.Services.Query.Commands
{
	using System;

	using JetBrains.Annotations;

	using Common.Command;
	using Modules.Core.Contracts.Query.Dto;

	[UsedImplicitly]
	internal sealed class UpdateQueryCommand : ICommand
	{
		public readonly QueryDto Query;

		public UpdateQueryCommand([NotNull] QueryDto query)
		{
			if (query == null) throw new ArgumentNullException(nameof(query));

			Query = query;
		}
	}
}