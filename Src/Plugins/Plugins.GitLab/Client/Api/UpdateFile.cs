namespace Plugins.GitLab.Client.Api
{
	using System;

	public sealed class UpdateFile
	{
		public readonly string BranchName;

		public readonly string FilePath;

		public readonly string CommitMessage;

		public readonly byte[] Content;

		public UpdateFile(string branchName, string filePath, string commitMessage, byte[] content)
		{
			if (branchName == null) throw new ArgumentNullException(nameof(branchName));
			if (filePath == null) throw new ArgumentNullException(nameof(filePath));
			if (commitMessage == null) throw new ArgumentNullException(nameof(commitMessage));
			if (content == null) throw new ArgumentNullException(nameof(content));

			BranchName = branchName;
			FilePath = filePath;
			CommitMessage = commitMessage;
			Content = content;
		}
	}
}