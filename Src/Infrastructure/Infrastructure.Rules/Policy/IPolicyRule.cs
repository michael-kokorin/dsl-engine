namespace Infrastructure.Rules.Policy
{
	using Infrastructure.Engines.Dsl;
	using Repository.Context;

	public interface IPolicyRule : IRule
	{
		PolicyRuleExpr Expression { get; }

		PolicyRules Rule { get; }
	}
}