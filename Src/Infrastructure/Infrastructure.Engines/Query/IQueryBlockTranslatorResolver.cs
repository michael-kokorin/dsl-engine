namespace Infrastructure.Engines.Query
{
	using Infrastructure.Engines.Dsl.Query;

	internal interface IQueryBlockTranslatorResolver
	{
		IQueryBlockTranslator<T> Resolve<T>() where T : class, IDslQueryBlock;
	}
}