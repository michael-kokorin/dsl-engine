namespace Infrastructure.Rules.Policy
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Logging;
	using Common.Security;
	using Infrastructure.Engines.Query.Result;
	using Infrastructure.Query;

	[UsedImplicitly]
	internal sealed class PolicyRuleExecutor : IRuleExecutor<IPolicyRule, PolicyRuleResult>
	{
		private readonly ILog _log;

		private readonly IQueryStorage _queryStorage;

		private readonly IQueryExecutor _queryExecutor;

		private readonly IUserPrincipal _userPrincipal;

		public PolicyRuleExecutor(
			[NotNull] ILog log,
			[NotNull] IQueryStorage queryStorage,
			[NotNull] IQueryExecutor queryExecutor,
			[NotNull] IUserPrincipal userPrincipal)
		{
			if (log == null) throw new ArgumentNullException(nameof(log));
			if (queryStorage == null) throw new ArgumentNullException(nameof(queryStorage));
			if (queryExecutor == null) throw new ArgumentNullException(nameof(queryExecutor));
			if (userPrincipal == null) throw new ArgumentNullException(nameof(userPrincipal));

			_queryStorage = queryStorage;
			_queryExecutor = queryExecutor;
			_userPrincipal = userPrincipal;
			_log = log;
		}

		public PolicyRuleResult Execute([NotNull] IPolicyRule rule, IDictionary<string, string> parameters)
		{
			if (rule == null) throw new ArgumentNullException(nameof(rule));

			var queriedData = new Dictionary<string, QueryResult>();

			foreach (var dataParameterExpr in rule.Expression.Data)
			{
				var query = _queryStorage.Get(rule.Rule.ProjectId, dataParameterExpr.Value);

				var result = _queryExecutor.Execute(query.Id, _userPrincipal.Info.Id, parameters.ToArray());

				queriedData.Add(dataParameterExpr.Name, result);
			}

			// ReSharper disable once LoopCanBePartlyConvertedToQuery
			foreach (var queryResult in queriedData)
			{
				if (queryResult.Value.Exceptions != null &&
				    queryResult.Value.Exceptions.Any())
				{
					throw new PolicyRuleQueryException(queryResult.Value);
				}

				if (queryResult.Value.Items == null ||
				    !queryResult.Value.Items.Any()) continue;

				_log.Info($"SDL policy failed. Policy name='{rule.Rule.Name}', Query key='{queryResult.Key}'");

				return new PolicyRuleResult
				{
					Success = false,
					FailedRule = rule.Rule.Name,
					FailedKey = queryResult.Key
				};
			}

			_log.Info($"SDL policy succeed. Policy name='{rule.Rule.Name}'");

			return new PolicyRuleResult
			{
				Success = true
			};
		}
	}
}