namespace Infrastructure.Plugins.Contracts
{
	using System.Collections.Generic;

	/// <summary>
	///     Issue tracker plugin interface
	/// </summary>
	public interface IIssueTrackerPlugin: ICorePlugin
	{
		/// <summary>
		///     Creates the issue
		/// </summary>
		/// <param name="branchId">Target branch Id</param>
		/// <param name="createIssueRequest">The issue.</param>
		/// <returns></returns>
		Issue CreateIssue(string branchId, CreateIssueRequest createIssueRequest);

		/// <summary>
		///     Gets the issue.
		/// </summary>
		/// <param name="issueId">The issue identifier.</param>
		/// <returns></returns>
		Issue GetIssue(string issueId);

		/// <summary>
		///     Gets the issues.
		/// </summary>
		/// <returns>The issues.</returns>
		IEnumerable<Issue> GetIssues();

		/// <summary>
		///     Updates the issue.
		/// </summary>
		/// <param name="updateIssueRequest">The update issue.</param>
		/// <returns></returns>
		Issue UpdateIssue(UpdateIssueRequest updateIssueRequest);
	}
}