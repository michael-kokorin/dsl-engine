namespace Infrastructure.Engines.Query
{
	using System;

	using JetBrains.Annotations;

	using Infrastructure.Engines.Dsl.Query;

	[UsedImplicitly]
	internal sealed class QueryBlockTranslationManager : IQueryBlockTranslationManager
	{
		private readonly IQueryBlockTranslatorResolver _queryBlockTranslatorResolver;

		public QueryBlockTranslationManager([NotNull] IQueryBlockTranslatorResolver queryBlockTranslatorResolver)
		{
			if (queryBlockTranslatorResolver == null) throw new ArgumentNullException(nameof(queryBlockTranslatorResolver));

			_queryBlockTranslatorResolver = queryBlockTranslatorResolver;
		}

		public string Translate<T>(T queryBlock) where T : class, IDslQueryBlock
		{
			var translator = _queryBlockTranslatorResolver.Resolve<T>();

			return translator.Translate(queryBlock);
		}

		public string ToDsl<T>(T queryBlock) where T : class, IDslQueryBlock
		{
			var translator = _queryBlockTranslatorResolver.Resolve<T>();

			return translator.ToDsl(queryBlock);
		}
	}
}