namespace Infrastructure.Rules.Policy
{
	using System;
	using System.Linq;

	using Infrastructure.Engines.Query.Result;

	internal sealed class PolicyRuleQueryException : Exception
	{
		public PolicyRuleQueryException(QueryResult queryResult)
			: base($"Policy rule query execution failed. Messages='{string.Join(", ", queryResult.Exceptions.Select(_ => _.Message))}'")
		{

		}
	}
}