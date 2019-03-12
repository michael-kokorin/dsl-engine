namespace Infrastructure.Query.Evaluation
{
	using System.Collections.Generic;

	using Infrastructure.Engines.Dsl.Query;
	using Infrastructure.Engines.Query.Result;

	// ReSharper disable once MemberCanBeInternal
	public interface IDslDataQueryEvaluator
	{
		IEnumerable<QueryResultColumn> Evaluate(DslDataQuery query, long userId);
	}
}