namespace Plugins.Tfs.It
{
	using System;

	public sealed class TeamFoundationIssueDoesNotExistsException : Exception
	{
		public TeamFoundationIssueDoesNotExistsException(string host, string project, string isueId)
			: base($"Team Foundation Service issue does not exists. Host='{host}', Project='{project}', Id='{isueId}'")
		{

		}
	}
}