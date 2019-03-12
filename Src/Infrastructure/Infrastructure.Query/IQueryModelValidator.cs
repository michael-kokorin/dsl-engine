namespace Infrastructure.Query
{
	using Infrastructure.Engines.Dsl.Query;

	public interface IQueryModelValidator
	{
		void Validate(DslDataQuery dslDataQuery);
	}
}