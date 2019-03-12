namespace Infrastructure.Rules.Extensions
{
	using Microsoft.Practices.Unity;

	using Common.Container;
	using Common.Extensions;

	public static class ContainerExtension
	{
		public static IUnityContainer RegisterRuleExecutor<TRule, TRuleResult, TRuleExecutor>(this IUnityContainer container, ReuseScope reuseScope)
			where TRule : IRule
			where TRuleResult : IRuleResult<TRule>
			where TRuleExecutor : IRuleExecutor<TRule, TRuleResult> =>
			container.RegisterType<IRuleExecutor<TRule, TRuleResult>, TRuleExecutor>(reuseScope);
	}
}