namespace Infrastructure.Rules.Workflow
{
	using Infrastructure.Engines.Dsl;

	/// <summary>
	/// Represents contract for workflow rule.
	/// </summary>
	public interface IWorkflowRule : IRule
	{
		WorkflowRuleExpr Expression { get; }

		/// <summary>
		///     Gets a value indicating whether this instance is event triggered rule.
		/// </summary>
		/// <value>
		///     <see langword="true" /> if this instance is event triggered rule; otherwise, <see langword="false" />.
		/// </value>
		bool IsEventTriggered { get; }

		/// <summary>
		///     Gets a value indicating whether this instance is time triggered rule.
		/// </summary>
		/// <value>
		///     <see langword="true" /> if this instance is time triggered rule; otherwise, <see langword="false" />.
		/// </value>
		bool IsTimeTriggered { get; }
	}
}