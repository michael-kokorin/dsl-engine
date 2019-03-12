namespace Workflow.GitHub
{
	using Infrastructure.Plugins.Agent.Contracts;

	/// <summary>
	///   Represents change in file.
	/// </summary>
	public sealed class FileChangeAnnotation
	{
		/// <summary>
		///   Gets or sets the annotation.
		/// </summary>
		/// <value>
		///   The annotation.
		/// </value>
		public IssueAnnotation Annotation { get; set; }

		/// <summary>
		///   Gets or sets the vulnerability.
		/// </summary>
		/// <value>
		///   The vulnerability.
		/// </value>
		public VulnerabilityInfo Vulnerability { get; set; }

		/// <summary>
		///   Gets or sets the type.
		/// </summary>
		/// <value>
		///   The type.
		/// </value>
		public FileChangeAnnotationType Type { get; set; }
	}
}