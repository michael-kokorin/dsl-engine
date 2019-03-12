namespace Infrastructure.Engines
{
	using System;
	using System.Linq;
	using System.Text;

	using JetBrains.Annotations;

	using Infrastructure.Engines.Dsl.Query;
	using Infrastructure.Engines.Query;

	[UsedImplicitly]
	public sealed class DataQueryExpressionTranslator : IDataQueryExpressionTranslator
	{
		private readonly IQueryBlockTranslationManager _queryBlockTranslationManager;

		private readonly IQueryToTableRenderer _queryToTableRenderer;

		public DataQueryExpressionTranslator(
			[NotNull] IQueryBlockTranslationManager queryBlockTranslationManager,
			[NotNull] IQueryToTableRenderer queryToTableRenderer)
		{
			if (queryBlockTranslationManager == null) throw new ArgumentNullException(nameof(queryBlockTranslationManager));
			if (queryToTableRenderer == null) throw new ArgumentNullException(nameof(queryToTableRenderer));

			_queryBlockTranslationManager = queryBlockTranslationManager;
			_queryToTableRenderer = queryToTableRenderer;
		}

		public string Translate(DslDataQuery queryExpression)
		{
			if (queryExpression == null)
				throw new ArgumentNullException(nameof(queryExpression));

			var builder = new StringBuilder("source.Query()\n");

			RenderBlocks(queryExpression, builder);

			builder.AppendLine(_queryToTableRenderer.RenderToTable(queryExpression));

			ApplyTakeFirst(queryExpression, builder);

			return builder.ToString();
		}

		private static void ApplyTakeFirst(DslDataQuery queryExpression, StringBuilder builder)
		{
			if (queryExpression.TakeFirst)
			{
				builder.AppendLine(".First()");
			}
			else if (queryExpression.TakeFirstOrDefault)
			{
				builder.AppendLine(".FirstOrDefault()");
			}
		}

		private void RenderBlocks(DslDataQuery queryExpression, StringBuilder builder)
		{
			if (queryExpression.Blocks == null || !queryExpression.Blocks.Any()) return;

			// ReSharper disable once LoopCanBePartlyConvertedToQuery
			foreach (var queryBlock in queryExpression.Blocks)
			{
				var translatedBlock = _queryBlockTranslationManager.Translate((dynamic) queryBlock);

				builder.AppendLine(translatedBlock);
			}
		}
	}
}