namespace Infrastructure.Rules.Workflow
{
	using System;

	using Infrastructure.Engines.Dsl;

	internal sealed class TimeWorkflowRule : WorkflowRule, ITimeWorkflowRule
	{
		public TimeWorkflowRule(WorkflowRuleExpr expression) : base(expression)
		{
		}

		public DateTime Start => Expression.Trigger.Start;

		public bool IsRepeatable => Expression.Trigger.Repeat.HasValue;

		public TimeSpan? Repeat => Expression.Trigger.Repeat;
	}
}