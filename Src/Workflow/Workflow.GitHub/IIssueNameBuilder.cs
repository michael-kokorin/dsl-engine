namespace Workflow.GitHub
{
	/// <summary>
	///   Provides methods to build issue name.
	/// </summary>
	internal interface IIssueNameBuilder
	{
		/// <summary>
		///   Builds the issue name by the specified information.
		/// </summary>
		/// <param name="info">The information.</param>
		/// <returns>The issue name.</returns>
		string Build(IssueNameBuilderInfo info);
	}
}