namespace Workflow.GitHub
{
	/// <summary>
	///   Provides methods to get annotation issues from the specified source code folder.
	/// </summary>
	internal interface IAnnotationIssuesProvider
	{
		/// <summary>
		///   Gets the issues.
		/// </summary>
		/// <param name="repoPath">The repo path.</param>
		/// <param name="sourceCodeFiles">The source code files.</param>
		/// <returns>
		///   Issues.
		/// </returns>
		IssueAnnotation[] GetIssues(string repoPath, string[] sourceCodeFiles);
	}
}