namespace Plugins.Git.Vcs.Client
{
	using System;

	using JetBrains.Annotations;

	internal sealed class CloneRepository
	{
		public readonly string Branch;

		public readonly string Uri;

		public readonly string Username;

		public readonly string Password;

		public CloneRepository([NotNull] string uri,string branch, string username, string password)
		{
			if (uri == null) throw new ArgumentNullException(nameof(uri));

			Uri = uri;
			Branch = branch;
			Username = username;
			Password = password;
		}
	}
}