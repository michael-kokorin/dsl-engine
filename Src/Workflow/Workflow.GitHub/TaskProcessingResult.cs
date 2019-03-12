namespace Workflow.GitHub
{
	using Infrastructure.Plugins.Agent.Contracts;

	/// <summary>
	///   Represents results of task processing.
	/// </summary>
	public sealed class TaskProcessingResult
	{
		/// <summary>
		///   Gets or sets the fp.
		/// </summary>
		/// <value>
		///   The fp.
		/// </value>
		public IssueVulnerabilityLink[] FalsePositivePairs { get; set; }

		/// <summary>
		///   Gets or sets the false positive annotations.
		/// </summary>
		/// <value>
		///   The false positive annotations.
		/// </value>
		public IssueAnnotation[] FalsePositiveAnnotations { get; set; }

		/// <summary>
		///   Gets or sets the fixed.
		/// </summary>
		/// <value>
		///   The fixed.
		/// </value>
		public IssueAnnotation[] Fixed { get; set; }

		/// <summary>
		///   Gets or sets the todo.
		/// </summary>
		/// <value>
		///   The todo.
		/// </value>
		public IssueVulnerabilityLink[] Todo { get; set; }

		/// <summary>
		///   Gets or sets the reopen.
		/// </summary>
		/// <value>
		///   The reopen.
		/// </value>
		public IssueVulnerabilityLink[] Reopen { get; set; }
	}

	public sealed class IssueVulnerabilityLink
	{
		public readonly IssueAnnotation IssueAnnotation;

		public readonly VulnerabilityInfo VulnerabilityInfo;

		public IssueVulnerabilityLink(IssueAnnotation issueAnnotation, VulnerabilityInfo vulnerabilityInfo)
		{
			IssueAnnotation = issueAnnotation;

			VulnerabilityInfo = vulnerabilityInfo;
		}
	}
}