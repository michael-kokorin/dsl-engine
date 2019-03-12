namespace Infrastructure.Query
{
	using Infrastructure.Engines.Dsl.Query;

	public interface IQueryModelAccessValidator
	{
		void Validate(DslDataQuery query, long? projectId, bool isSystem);
	}
}