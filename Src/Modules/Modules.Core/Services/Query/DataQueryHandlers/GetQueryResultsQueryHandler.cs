namespace Modules.Core.Services.Query.DataQueryHandlers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Query;
	using Infrastructure.Query;
	using Modules.Core.Contracts.UI.Dto.Data;
	using Modules.Core.Services.Query.DataQueries;
	using Modules.Core.Services.UI.Renderers;

	[UsedImplicitly]
	internal sealed class GetQueryResultsQueryHandler : IDataQueryHandler<GetQueryResultsQuery, TableDto>
	{
		private readonly IQueryExecutor _queryExecutor;

		public GetQueryResultsQueryHandler([NotNull] IQueryExecutor queryExecutor)
		{
			if (queryExecutor == null) throw new ArgumentNullException(nameof(queryExecutor));

			_queryExecutor = queryExecutor;
		}

		public TableDto Execute(GetQueryResultsQuery dataQuery)
		{
			if (dataQuery == null)
				throw new ArgumentNullException(nameof(dataQuery));

			KeyValuePair<string, string>[] parameters = null;

			if (dataQuery.Parameters != null)
			{
				parameters = dataQuery.Parameters
					.Select(_ => new KeyValuePair<string, string>(_.Key, _.Value))
					.ToArray();
			}

			var queryResult = _queryExecutor.Execute(dataQuery.QueryId, dataQuery.UserId, parameters);

			var result = new TableRenderer().Render(queryResult);

			return result;
		}
	}
}