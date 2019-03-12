namespace Modules.Core.Services.Query.DataQueryHandlers
{
	using System;

	using JetBrains.Annotations;

	using Common.Extensions;
	using Common.Query;
	using Infrastructure.Engines.Query;
	using Modules.Core.Services.Query.DataQueries;

	[UsedImplicitly]
	public sealed class GetQueryModelQueryHandler : IDataQueryHandler<GetQueryModelQuery, string>
	{
		private readonly IQueryTranslator _queryTranslator;

		public GetQueryModelQueryHandler([NotNull] IQueryTranslator queryTranslator)
		{
			if (queryTranslator == null) throw new ArgumentNullException(nameof(queryTranslator));

			_queryTranslator = queryTranslator;
		}

		public string Execute([NotNull] GetQueryModelQuery dataQuery)
		{
			if (dataQuery == null) throw new ArgumentNullException(nameof(dataQuery));

			var queryModel = _queryTranslator.ToQuery(dataQuery.Query);

			var serializedQueryModel = queryModel.ToJson();

			return serializedQueryModel;
		}
	}
}