namespace Plugins.GitLab.Client.Entity
{
	public sealed class User
	{
		public long Id { get; set; }

		public string Name { get; set; }

		public string UserName { get; set; }

		public string WebUrl { get; set; }
	}
}