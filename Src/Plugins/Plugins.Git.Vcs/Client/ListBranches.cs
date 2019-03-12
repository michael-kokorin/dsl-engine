namespace Plugins.Git.Vcs.Client
{
	using System;

	using JetBrains.Annotations;

	internal sealed class ListBranches
	{
		public readonly string Password;

		public readonly string Url;

		public readonly string Username;

		public ListBranches([NotNull] string url, string username, string password)
		{
			if (url == null) throw new ArgumentNullException(nameof(url));

			Url = url;
			Username = username;
			Password = password;
		}
	}
}