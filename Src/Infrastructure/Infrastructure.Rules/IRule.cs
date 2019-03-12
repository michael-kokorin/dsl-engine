namespace Infrastructure.Rules
{
	/// <summary>
	///     Represents contract for rule.
	/// </summary>
	public interface IRule
	{
		/// <summary>
		///     Gets the type of the rule.
		/// </summary>
		/// <value>
		///     The type of the rule.
		/// </value>
		RuleType RuleType { get; }
	}
}