namespace Infrastructure.Query
{
	using System;

	using JetBrains.Annotations;

	using Infrastructure.Engines.Dsl.Query;
	using Infrastructure.Engines.Query;

	[UsedImplicitly]
	internal sealed class QueryModelProcessor : IQueryModelProcessor
	{
		private readonly IQueryTranslator _queryTranslator;

		private readonly IQueryModelAccessValidator _queryModelAccessValidator;

		public QueryModelProcessor([NotNull] IQueryTranslator queryTranslator,
			[NotNull] IQueryModelAccessValidator queryModelAccessValidator)
		{
			if (queryTranslator == null) throw new ArgumentNullException(nameof(queryTranslator));
			if (queryModelAccessValidator == null) throw new ArgumentNullException(nameof(queryModelAccessValidator));

			_queryTranslator = queryTranslator;
			_queryModelAccessValidator = queryModelAccessValidator;
		}

		public DslDataQuery FromText(string queryText, long? projectId, bool isSystem)
		{
			if (string.IsNullOrEmpty(queryText))
				throw new ArgumentNullException(nameof(queryText));

			var queryModel = _queryTranslator.ToQuery(queryText);

			_queryModelAccessValidator.Validate(queryModel, projectId, isSystem);

			return queryModel;
		}

		public string ToText(DslDataQuery query, long? projectId, bool isSystem)
		{
			_queryModelAccessValidator.Validate(query, projectId, isSystem);

			return _queryTranslator.ToDsl(query);
		}
	}
}