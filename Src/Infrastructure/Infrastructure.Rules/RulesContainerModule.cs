namespace Infrastructure.Rules
{
	using Microsoft.Practices.Unity;

	using Common.Container;
	using Common.Extensions;
	using Infrastructure.Rules.Extensions;
	using Infrastructure.Rules.Notification;
	using Infrastructure.Rules.Policy;
	using Infrastructure.Rules.Workflow;

	public sealed class RulesContainerModule : IContainerModule
	{
		/// <summary>
		///   Registers binding to the specified container.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <param name="reuseScope">The reuse scope.</param>
		/// <returns>The container.</returns>
		public IUnityContainer Register(IUnityContainer container, ReuseScope reuseScope) =>
			container
				.RegisterType<IRuleParser, RuleParser>(reuseScope)
				.RegisterType<IRuleExecutorDirector, RuleExecutorDirector>(reuseScope)

				.RegisterRuleExecutor<INotificationRule, NotificationRuleResult, NotificationRuleExecutor>(reuseScope)
				.RegisterRuleExecutor<IPolicyRule, PolicyRuleResult, PolicyRuleExecutor>(reuseScope)
				.RegisterRuleExecutor<IWorkflowRule, WorkflowRuleResult, WorkflowRuleExecutor>(reuseScope);
	}
}