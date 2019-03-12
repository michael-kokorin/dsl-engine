namespace Infrastructure.Rules.Workflow
{
	using System;

	using JetBrains.Annotations;

	using Infrastructure.Engines.Dsl;

	public abstract class WorkflowRule : IWorkflowRule
	{
		public WorkflowRuleExpr Expression { get; }

		public RuleType RuleType => RuleType.Workflow;

		public bool IsEventTriggered => Expression.Event != null;

		public bool IsTimeTriggered => Expression.Trigger != null;

		protected WorkflowRule([NotNull] WorkflowRuleExpr expression)
		{
			if (expression == null) throw new ArgumentNullException(nameof(expression));

			Expression = expression;
		}

	}
}