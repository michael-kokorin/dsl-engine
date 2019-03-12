namespace Infrastructure.Rules
{
	using JetBrains.Annotations;

	using Infrastructure.Engines.Dsl;
	using Infrastructure.Rules.Notification;
	using Infrastructure.Rules.Policy;
	using Infrastructure.Rules.Workflow;
	using Repository.Context;

	[UsedImplicitly]
	internal sealed class RuleParser : IRuleParser
	{
		private readonly IDslParser _dslParser;

		public RuleParser(IDslParser dslParser)
		{
			_dslParser = dslParser;
		}

		public INotificationRule ParseNotificationRule(string query)
		{
			var expr = _dslParser.NotificationRuleExprParse(query);

			if (expr.Event != null)
			{
				return new EventNotificationRule(expr);
			}

			return new TimeNotificationRule(expr);
		}

		public IWorkflowRule ParseWorkflowRule(string query)
		{
			var expr = _dslParser.WorkflowRuleExprParse(query);

			if (expr.Event != null)
			{
				return new EventWorkflowRule(expr);
			}

			return new TimeWorkflowRule(expr);
		}

		public IPolicyRule ParsePolicyRule(PolicyRules rule)
		{
			var expr = _dslParser.PolicyRuleExprParse(rule.Query);

			return new PolicyRule(expr, rule);
		}
	}
}