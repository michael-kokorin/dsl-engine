namespace Plugins.Git.Vcs.Client
{
	using System;

	using JetBrains.Annotations;

	internal sealed class CreateBranch
	{
		public readonly string NewBranchName;

		public readonly string Username;

		public readonly string Password;

		public CreateBranch([NotNull] string newBranchName, string username, string password)
		{
			if (newBranchName == null) throw new ArgumentNullException(nameof(newBranchName));

			NewBranchName = newBranchName;
			Username = username;
			Password = password;
		}
	}
}