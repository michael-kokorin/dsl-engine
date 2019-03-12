namespace Modules.Core.Services.Query.DataQueryHandlers
{
	using System;

	using JetBrains.Annotations;

	using Common.Extensions;
	using Common.Query;
	using Infrastructure.Engines.Dsl.Query;
	using Infrastructure.Engines.Query;
	using Modules.Core.Services.Query.DataQueries;

	[UsedImplicitly]
	public sealed class GetQueryTextQueryHandler : IDataQueryHandler<GetQueryTextQuery, string>
	{
		private readonly IQueryTranslator _queryTranslator;

		public GetQueryTextQueryHandler([NotNull] IQueryTranslator queryTranslator)
		{
			if (queryTranslator == null) throw new ArgumentNullException(nameof(queryTranslator));

			_queryTranslator = queryTranslator;
		}

		public string Execute([NotNull] GetQueryTextQuery dataQuery)
		{
			if (dataQuery == null) throw new ArgumentNullException(nameof(dataQuery));

			var queryModel = dataQuery.QueryModel.FromJson<DslDataQuery>();

			var queryText = _queryTranslator.ToDsl(queryModel);

			return queryText;
		}
	}
}