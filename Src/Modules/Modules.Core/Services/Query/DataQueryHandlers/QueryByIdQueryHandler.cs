namespace Modules.Core.Services.Query.DataQueryHandlers
{
	using System;

	using JetBrains.Annotations;

	using Common.Query;
	using Infrastructure.Query;
	using Modules.Core.Contracts.Query.Dto;
	using Modules.Core.Services.Query.DataQueries;

	[UsedImplicitly]
	internal sealed class QueryByIdQueryHandler : IDataQueryHandler<QueryByIdQuery, QueryDto>
	{
		private readonly IQueryStorage _queryStorage;

		public QueryByIdQueryHandler([NotNull] IQueryStorage queryStorage)
		{
			if (queryStorage == null) throw new ArgumentNullException(nameof(queryStorage));

			_queryStorage = queryStorage;
		}

		public QueryDto Execute(QueryByIdQuery dataQuery)
		{
			if (dataQuery == null)
				throw new ArgumentNullException(nameof(dataQuery));

			var query = _queryStorage.Get(dataQuery.QueryId);

			return query?.ToDto();
		}
	}
}