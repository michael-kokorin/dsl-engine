namespace Plugins.GitLab.Vcs.Extensions
{
	using System.IO;

	internal static class StringExtension
	{
		private const string BranchPattern = "heads/{0}";

		public static void CopyTo(this string sourcePath, string targetPath)
		{
			if (!Directory.Exists(targetPath))
			{
				Directory.CreateDirectory(targetPath);
			}

			foreach (var file in Directory.GetFiles(sourcePath))
			{
				var fileName = Path.GetFileName(file);

				if (string.IsNullOrEmpty(fileName))
				{
					continue;
				}

				File.Copy(file, Path.Combine(targetPath, fileName), true);
			}

			foreach (var directory in Directory.GetDirectories(sourcePath))
			{
				var fileName = Path.GetFileName(directory);

				if (string.IsNullOrEmpty(fileName))
				{
					continue;
				}

				CopyTo(directory, Path.Combine(targetPath, fileName));
			}
		}

		public static string ToBranchName(this string source) =>
			string.Format(BranchPattern, source);

		public static string ToGitPath(this string source) =>
			source.Replace('\\', '/');
	}
}