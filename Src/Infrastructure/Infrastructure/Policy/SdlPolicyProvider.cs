namespace Infrastructure.Policy
{
	using System;
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Logging;
	using Common.Time;
	using Repository.Context;
	using Repository.Repositories;

	[UsedImplicitly]
	internal sealed class SdlPolicyProvider : ISdlPolicyProvider
	{
		private readonly IPolicyRuleRepository _policyRuleRepository;

		private readonly ITimeService _timeService;

		public SdlPolicyProvider(
			[NotNull] IPolicyRuleRepository policyRuleRepository,
			[NotNull] ITimeService timeService)
		{
			if (policyRuleRepository == null) throw new ArgumentNullException(nameof(policyRuleRepository));
			if (timeService == null) throw new ArgumentNullException(nameof(timeService));

			_policyRuleRepository = policyRuleRepository;
			_timeService = timeService;
		}

		[LogMethod(LogInputParameters = true)]
		public void Add(long projectId, string name, string query)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException(nameof(name));

			if (string.IsNullOrEmpty(query))
				throw new ArgumentNullException(nameof(query));

			if (_policyRuleRepository.Get(projectId, name).Any())
				return;

			var policy = new PolicyRules
			{
				Added = _timeService.GetUtc(),
				Name = name,
				ProjectId = projectId,
				Query = query
			};

			_policyRuleRepository.Insert(policy);

			_policyRuleRepository.Save();
		}
	}
}