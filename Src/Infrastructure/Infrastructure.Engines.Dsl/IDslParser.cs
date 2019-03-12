namespace Infrastructure.Engines.Dsl
{
	using Infrastructure.Engines.Dsl.Query;

	/// <summary>
	///     Provides contract for DSL parser.
	/// </summary>
	public interface IDslParser
	{
		/// <summary>
		///     Parser for notification rule.
		/// </summary>
		/// <returns>Parser.</returns>
		NotificationRuleExpr NotificationRuleExprParse(string query);

		/// <summary>
		///     Parser for data query.
		/// </summary>
		/// <returns>Parser.</returns>
		DslDataQuery DataQueryParse(string query);

		/// <summary>
		///     Parser for policy rule expression.
		/// </summary>
		/// <returns>Parser.</returns>
		PolicyRuleExpr PolicyRuleExprParse(string query);

		/// <summary>
		///     Parser for workflow rule expression.
		/// </summary>
		/// <returns>Parser.</returns>
		WorkflowRuleExpr WorkflowRuleExprParse(string query);
	}
}