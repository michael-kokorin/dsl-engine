namespace Infrastructure.Engines.Query
{
	using System;
	using System.Text;

	using JetBrains.Annotations;

	using Infrastructure.Engines.Dsl;
	using Infrastructure.Engines.Dsl.Query;

	[UsedImplicitly]
	internal sealed class QueryTranslator : IQueryTranslator
	{
		private readonly IDslParser _dslParser;

		private readonly IQueryBlockTranslationManager _queryBlockTranslationManager;

		public QueryTranslator(
			[NotNull] IDslParser dslParser,
			[NotNull] IQueryBlockTranslationManager queryBlockTranslationManager)
		{
			if (dslParser == null) throw new ArgumentNullException(nameof(dslParser));
			if (queryBlockTranslationManager == null) throw new ArgumentNullException(nameof(queryBlockTranslationManager));

			_dslParser = dslParser;
			_queryBlockTranslationManager = queryBlockTranslationManager;
		}

		public DslDataQuery ToQuery([NotNull] string dslQuery)
		{
			if (string.IsNullOrEmpty(dslQuery))
				throw new ArgumentNullException(nameof(dslQuery));

			return _dslParser.DataQueryParse(dslQuery);
		}

		public string ToDsl([NotNull] DslDataQuery query)
		{
			if (query == null) throw new ArgumentNullException(nameof(query));

			var stringBuilder = new StringBuilder();

			TranslateEntityName(query, stringBuilder);

			TranslateBlocks(query, stringBuilder);

			if (query.TakeFirst)
				stringBuilder.AppendLine(DslKeywords.First);

			if (query.TakeFirstOrDefault)
				stringBuilder.AppendLine(DslKeywords.FirstOrDefault);

			if (!string.IsNullOrEmpty(query.TableKey))
				stringBuilder.AppendLine($"{DslKeywords.Table} {query.TableKey}");

			return stringBuilder.ToString();
		}

		private static void TranslateEntityName(DslDataQuery query, StringBuilder stringBuilder)
			=> stringBuilder.AppendLine(query.QueryEntityName);

		private void TranslateBlocks(DslDataQuery query, StringBuilder stringBuilder)
		{
			if (query.Blocks == null)
				return;

			// ReSharper disable once LoopCanBePartlyConvertedToQuery
			foreach (var queryBlock in query.Blocks)
			{
				var translatedBlock = _queryBlockTranslationManager.ToDsl((dynamic) queryBlock);

				stringBuilder.AppendLine(translatedBlock);
			}
		}
	}
}