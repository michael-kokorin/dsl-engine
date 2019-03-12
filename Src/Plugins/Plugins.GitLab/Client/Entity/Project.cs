namespace Plugins.GitLab.Client.Entity
{
	public sealed class Project
	{
		public string DefaultBranch { get; set; }

		public long Id { get; set; }

		public string Name { get; set; }

		public User Owner { get; set; }
	}
}