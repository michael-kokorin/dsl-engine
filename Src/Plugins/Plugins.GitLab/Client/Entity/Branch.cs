namespace Plugins.GitLab.Client.Entity
{
	public sealed class Branch
	{
		public Commit Commit { get; set; }

		public string Name { get; set; }
	}
}