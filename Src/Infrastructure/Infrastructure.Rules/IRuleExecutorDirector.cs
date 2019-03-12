namespace Infrastructure.Rules
{
	using System.Collections.Generic;

	public interface IRuleExecutorDirector
	{
		TRuleResult Execute<TRule, TRuleResult>(TRule rule, IDictionary<string, string> parameters)
			where TRule : IRule
			where TRuleResult : IRuleResult<TRule>;
	}
}