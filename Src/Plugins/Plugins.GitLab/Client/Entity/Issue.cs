namespace Plugins.GitLab.Client.Entity
{
	using System;

	public sealed class Issue
	{
		public User Assignee { get; set; }

		public User Author { get; set; }

		public DateTime CreatedAt { get; set; }

		public string Description { get; set; }

		public long Id { get; set; }

		public long Iid { get; set; }

		public long ProjectId { get; set; }

		public string State { get; set; }

		public string Title { get; set; }
	}
}