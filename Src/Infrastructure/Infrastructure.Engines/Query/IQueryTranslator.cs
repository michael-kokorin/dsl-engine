namespace Infrastructure.Engines.Query
{
	using Infrastructure.Engines.Dsl.Query;

	public interface IQueryTranslator
	{
		DslDataQuery ToQuery(string dslQuery);

		string ToDsl(DslDataQuery query);
	}
}