namespace Infrastructure.Query
{
	using Infrastructure.Engines.Dsl.Query;

	// ReSharper disable once MemberCanBeInternal
	public interface IQueryModelProcessor
	{
		DslDataQuery FromText(string queryText, long? projectId, bool isSystem);

		string ToText(DslDataQuery query, long? projectId, bool isSystem);
	}
}