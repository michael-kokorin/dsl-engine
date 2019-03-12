namespace Plugins.GitLab.Client.Entity
{
	using System;

	public sealed class Commit
	{
		public DateTime CreatedAt { get; set; }

		public string Id { get; set; }

		public string Title { get; set; }

		public string Message { get; set; }
	}
}