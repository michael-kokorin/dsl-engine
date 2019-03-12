namespace Infrastructure.Rules.Workflow
{
	using System;
	using System.Collections.Generic;

	using JetBrains.Annotations;

	using Infrastructure.Engines;

	[UsedImplicitly]
	internal sealed class WorkflowRuleExecutor : IRuleExecutor<IWorkflowRule, WorkflowRuleResult>
	{
		private readonly IWorkflowActionProvider _workflowActionProvider;

		public WorkflowRuleExecutor([NotNull] IWorkflowActionProvider workflowActionProvider)
		{
			if (workflowActionProvider == null) throw new ArgumentNullException(nameof(workflowActionProvider));

			_workflowActionProvider = workflowActionProvider;
		}

		public WorkflowRuleResult Execute(IWorkflowRule rule, IDictionary<string, string> parameters)
		{
			var action = _workflowActionProvider.Get(rule.Expression.Action.Value);

			if (action == null)
			{
				throw new Exception();
			}

			action.Execute(parameters);

			return new WorkflowRuleResult();
		}
	}
}