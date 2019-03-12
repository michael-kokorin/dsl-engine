namespace Infrastructure.Rules.Workflow
{
	using System;

	public interface ITimeWorkflowRule : IWorkflowRule
	{
		/// <summary>
		///     Gets the start.
		/// </summary>
		/// <value>
		///     The start.
		/// </value>
		DateTime Start { get; }

		/// <summary>
		///     Gets a value indicating whether this rule is repeatable.
		/// </summary>
		/// <value>
		///     <see langword="true" /> if this rule is repeatable; otherwise, <see langword="false" />.
		/// </value>
		bool IsRepeatable { get; }

		/// <summary>
		///     Gets the repeat time.
		/// </summary>
		/// <value>
		///     The repeat time.
		/// </value>
		TimeSpan? Repeat { get; }
	}
}