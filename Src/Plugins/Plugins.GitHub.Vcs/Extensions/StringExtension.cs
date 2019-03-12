namespace Plugins.GitHub.Vcs.Extensions
{
	using System.IO;

	using JetBrains.Annotations;

	internal static class StringExtension
	{
		private const string BranchPattern = "heads/{0}";

		public static void CopyTo(
			[NotNull] [PathReference] this string sourcePath,
			[NotNull] [PathReference] string targetPath)
		{
			if(!Directory.Exists(targetPath))
			{
				Directory.CreateDirectory(targetPath);
			}

			foreach(var file in Directory.GetFiles(sourcePath))
			{
				var fileName = Path.GetFileName(file);

				if(string.IsNullOrEmpty(fileName))
				{
					continue;
				}

				File.Copy(file, Path.Combine(targetPath, fileName), true);
			}

			foreach(var directory in Directory.GetDirectories(sourcePath))
			{
				var fileName = Path.GetFileName(directory);

				if(string.IsNullOrEmpty(fileName))
				{
					continue;
				}

				CopyTo(directory, Path.Combine(targetPath, fileName));
			}
		}

		[NotNull]
		public static string ToBranchName([NotNull] this string source) =>
			string.Format(BranchPattern, source);

		[NotNull]
		public static string ToGitPath([NotNull] this string source) =>
			source.Replace('\\', '/');
	}
}