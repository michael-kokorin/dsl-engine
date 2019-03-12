namespace Workflow.Event.Handlers
{
	using System;
	using System.Linq;

	using JetBrains.Annotations;

	using Microsoft.Practices.ObjectBuilder2;

	using Infrastructure;
	using Infrastructure.Events;
	using Infrastructure.Rules;
	using Infrastructure.Rules.Workflow;
	using Repository.Repositories;

	[UsedImplicitly]
	internal sealed class WorkflowEventHandler: IEventHandler
	{
		private readonly IRuleExecutorDirector _ruleExecutorDirector;

		private readonly IRuleParser _ruleParser;

		private readonly IWorkflowRuleRepository _workflowRuleRepository;

		public WorkflowEventHandler(
			[NotNull] IWorkflowRuleRepository workflowRuleRepository,
			[NotNull] IRuleParser ruleParser,
			[NotNull] IRuleExecutorDirector ruleExecutorDirector)
		{
			if(workflowRuleRepository == null) throw new ArgumentNullException(nameof(workflowRuleRepository));
			if(ruleParser == null) throw new ArgumentNullException(nameof(ruleParser));
			if(ruleExecutorDirector == null) throw new ArgumentNullException(nameof(ruleExecutorDirector));

			_workflowRuleRepository = workflowRuleRepository;
			_ruleParser = ruleParser;
			_ruleExecutorDirector = ruleExecutorDirector;
		}

		/// <summary>
		///   Determines whether this instance can handle the specified event to handle.
		/// </summary>
		/// <param name="eventToHandle">The event to handle.</param>
		/// <returns><see langword="true"/> if this instance can handle the event; otherwise, <see langword="false"/>.</returns>
		public bool CanHandle(Event eventToHandle) => true;

		/// <summary>
		///   Handles the specified event to handle.
		/// </summary>
		/// <param name="eventToHandle">The event to handle.</param>
		public void Handle(Event eventToHandle)
		{
			var data = eventToHandle.Data;

			if((data == null) || !data.ContainsKey(Variables.ProjectId))
				return;

			var projectId = long.Parse(data[Variables.ProjectId]);
			var rules = _workflowRuleRepository.GetByEventAndProject(eventToHandle.Key, projectId).ToArray();
			if(!rules.Any())
				return;

			var workflowRules = rules.Select(_ => _ruleParser.ParseWorkflowRule(_.Query)).ToArray();

			workflowRules.ForEach(_ => _ruleExecutorDirector.Execute<IWorkflowRule, WorkflowRuleResult>((dynamic)_, data));
		}
	}
}