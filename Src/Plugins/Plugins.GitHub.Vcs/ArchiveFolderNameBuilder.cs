namespace Plugins.GitHub.Vcs
{
	using JetBrains.Annotations;

	internal static class ArchiveFolderNameBuilder
	{
		private const string FolderNamePattern = "{0}-{1}-{2}";

		[NotNull]
		public static string Build([NotNull] string ownerName, [NotNull] string repositoryName, [NotNull] string commitSha)
		{
			var shortSha = SplitSha(commitSha);

			return string.Format(FolderNamePattern, ownerName, repositoryName, shortSha);
		}

		[NotNull]
		private static string SplitSha([NotNull] string commitSha) => commitSha.Substring(0, 7);
	}
}