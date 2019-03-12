namespace Infrastructure.Rules.Workflow
{
	using Infrastructure.Engines.Dsl;

	public sealed class EventWorkflowRule : WorkflowRule, IEventWorkflowRule
	{
		public EventWorkflowRule(WorkflowRuleExpr expression) : base(expression)
		{
		}

		public string[] GetDependentEvents => Expression.Event.Dependent;

		public bool HasDependentEvents => Expression.Event.HasDependent;

		public GroupActionType EventGroupAction => Expression.Event.GroupAction;

		public bool IsForAllEvents
			=> !Expression.Event.HasDependent || (Expression.Event.GroupAction == GroupActionType.Exclude);

		public bool DoesEventMatch(string eventName) => Expression.Event.IsMatch(eventName);
	}
}