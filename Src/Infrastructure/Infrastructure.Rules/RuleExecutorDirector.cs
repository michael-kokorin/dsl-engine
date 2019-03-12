namespace Infrastructure.Rules
{
	using System;
	using System.Collections.Generic;

	using JetBrains.Annotations;

	using Microsoft.Practices.Unity;

	[UsedImplicitly]
	internal sealed class RuleExecutorDirector : IRuleExecutorDirector
	{
		private readonly IUnityContainer _container;

		public RuleExecutorDirector([NotNull] IUnityContainer container)
		{
			if (container == null) throw new ArgumentNullException(nameof(container));

			_container = container;
		}

		public TRuleResult Execute<TRule, TRuleResult>(TRule rule, IDictionary<string, string> parameters)
			where TRule : IRule
			where TRuleResult : IRuleResult<TRule>
		{
			var ruleExecutor = _container.Resolve<IRuleExecutor<TRule, TRuleResult>>();

			return ruleExecutor.Execute((dynamic) rule, parameters);
		}
	}
}