namespace Infrastructure.Rules.Policy
{
	using System;

	using JetBrains.Annotations;

	using Infrastructure.Engines.Dsl;
	using Repository.Context;

	/// <summary>
	///     Represents policy rule.
	/// </summary>
	public sealed class PolicyRule : IPolicyRule
	{
		public PolicyRuleExpr Expression { get; }

		public  PolicyRules Rule { get; }

		public PolicyRule([NotNull] PolicyRuleExpr expression, [NotNull] PolicyRules rule)
		{
			if (expression == null) throw new ArgumentNullException(nameof(expression));
			if (rule == null) throw new ArgumentNullException(nameof(rule));

			Expression = expression;
			Rule = rule;
		}

		public RuleType RuleType => RuleType.Policy;
	}
}