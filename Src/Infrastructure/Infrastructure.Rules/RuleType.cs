namespace Infrastructure.Rules
{
	/// <summary>
	/// Indicates rule type.
	/// </summary>
	public enum RuleType
	{
		/// <summary>
		/// The notification rule.
		/// </summary>
		Notification = 1,

		/// <summary>
		/// The policy rule.
		/// </summary>
		Policy = 2,

		/// <summary>
		/// The workflow rule.
		/// </summary>
		Workflow = 3
	}
}