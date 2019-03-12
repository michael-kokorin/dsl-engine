namespace Infrastructure.Engines.Query
{
	using Infrastructure.Engines.Dsl.Query;

	public interface IQueryBlockTranslator<in T> where T : IDslQueryBlock
	{
		string Translate(T queryBlock);

		string ToDsl(T queryBlock);
	}
}