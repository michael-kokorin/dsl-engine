namespace Infrastructure.Rules
{
	using System.Collections.Generic;

	public interface IRuleExecutor<in TRule, out TRuleResult>
		where TRule : IRule
		where TRuleResult : IRuleResult<TRule>
	{
		TRuleResult Execute(TRule rule, IDictionary<string, string> parameters);
	}
}