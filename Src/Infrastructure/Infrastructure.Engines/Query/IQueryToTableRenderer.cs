namespace Infrastructure.Engines.Query
{
	using Infrastructure.Engines.Dsl.Query;

	public interface IQueryToTableRenderer
	{
		string RenderToTable(DslDataQuery query);
	}
}