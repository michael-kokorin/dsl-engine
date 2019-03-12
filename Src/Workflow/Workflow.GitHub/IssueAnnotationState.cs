namespace Workflow.GitHub
{
	using JetBrains.Annotations;

	/// <summary>
	///   Indicates state of annotation issue.
	/// </summary>
	[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
	public enum IssueAnnotationState
	{
		/// <summary>
		///   The issue that is not fixed yet.
		/// </summary>
		Todo = 0,

		/// <summary>
		///   The issue that should be verified that it is fixed.
		/// </summary>
		Verify = 1,

		/// <summary>
		///   The issue is fixed.
		/// </summary>
		Fixed = 2,

		/// <summary>
		///   The issue is reopened.
		/// </summary>
		Reopen = 3,

		/// <summary>
		///   The issue is found but marked as nonactive.
		/// </summary>
		FalsePositive = 4
	}
}