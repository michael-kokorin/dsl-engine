namespace Infrastructure.Rules.Policy
{
	public sealed class PolicyRuleResult : IRuleResult<IPolicyRule>
	{
		public bool Success { get; set; }

		public string FailedRule { get; set; }

		public string FailedKey { get; set; }
	}
}