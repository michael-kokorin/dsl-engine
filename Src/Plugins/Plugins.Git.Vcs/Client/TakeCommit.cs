namespace Plugins.Git.Vcs.Client
{
	using System;

	using JetBrains.Annotations;

	internal sealed class TakeCommit
	{
		public readonly string Branch;

		public readonly string Email;

		public readonly string LocalFilePath;

		public string Message;

		public string Password;

		public string Username;

		public TakeCommit([NotNull] string branch, [NotNull] string localfilePath, [NotNull] string email)
		{
			if (branch == null) throw new ArgumentNullException(nameof(branch));
			if (localfilePath == null) throw new ArgumentNullException(nameof(localfilePath));
			if (email == null) throw new ArgumentNullException(nameof(email));

			Branch = branch;
			Email = email;
			LocalFilePath = localfilePath;
		}
	}
}