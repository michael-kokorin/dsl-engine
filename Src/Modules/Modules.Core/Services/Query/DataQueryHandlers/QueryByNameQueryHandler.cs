namespace Modules.Core.Services.Query.DataQueryHandlers
{
	using System;

	using JetBrains.Annotations;

	using Common.Query;
	using Infrastructure.Query;
	using Modules.Core.Contracts.Query.Dto;
	using Modules.Core.Services.Query.DataQueries;

	[UsedImplicitly]
	internal sealed class QueryByNameQueryHandler : IDataQueryHandler<QueryByNameQuery, QueryDto>
	{
		private readonly IQueryStorage _queryStorage;

		public QueryByNameQueryHandler([NotNull] IQueryStorage queryStorage)
		{
			if (queryStorage == null) throw new ArgumentNullException(nameof(queryStorage));

			_queryStorage = queryStorage;
		}

		public QueryDto Execute(QueryByNameQuery dataQuery)
		{
			if (dataQuery == null)
				throw new ArgumentNullException(nameof(dataQuery));

			var query = _queryStorage.Get(dataQuery.ProjectId, dataQuery.Name);

			return query?.ToDto();
		}
	}
}