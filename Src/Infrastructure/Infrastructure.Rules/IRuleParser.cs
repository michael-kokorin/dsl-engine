namespace Infrastructure.Rules
{
	using Infrastructure.Rules.Notification;
	using Infrastructure.Rules.Policy;
	using Infrastructure.Rules.Workflow;
	using Repository.Context;

	/// <summary>
	///     Provides methods to parse rules.
	/// </summary>
	public interface IRuleParser
	{
		/// <summary>
		///     Parses the notification rule.
		/// </summary>
		/// <param name="query">The query.</param>
		/// <returns>Parsed rule.</returns>
		INotificationRule ParseNotificationRule(string query);

		/// <summary>
		///     Parses the workflow rule.
		/// </summary>
		/// <param name="query">The query.</param>
		/// <returns>Parsed rule.</returns>
		IWorkflowRule ParseWorkflowRule(string query);

		/// <summary>
		///     Parses the policy rule.
		/// </summary>
		/// <param name="rule">The rule.</param>
		/// <returns>Parsed rule.</returns>
		IPolicyRule ParsePolicyRule(PolicyRules rule);
	}
}