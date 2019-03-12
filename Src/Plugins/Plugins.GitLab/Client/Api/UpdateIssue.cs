namespace Plugins.GitLab.Client.Api
{
	public sealed class UpdateIssue
	{
		public string Description { get; set; }

		public string State { get; set; }

		public string Title { get; set; }
	}
}