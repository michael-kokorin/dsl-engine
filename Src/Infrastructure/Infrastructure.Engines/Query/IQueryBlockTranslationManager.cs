namespace Infrastructure.Engines.Query
{
	using Infrastructure.Engines.Dsl.Query;

	public interface IQueryBlockTranslationManager
	{
		string Translate<T>(T queryBlock) where T : class, IDslQueryBlock;

		string ToDsl<T>(T queryBlock) where T : class, IDslQueryBlock;
	}
}