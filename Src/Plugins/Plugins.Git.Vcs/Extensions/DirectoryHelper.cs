namespace Plugins.Git.Vcs.Extensions
{
	using System.IO;

	public static class DirectoryHelper
	{
		public static void SafeDelete(string foldetPath)
		{
			var files = Directory.GetFiles(foldetPath);
			var dirs = Directory.GetDirectories(foldetPath);

			foreach (var file in files)
			{
				File.SetAttributes(file, FileAttributes.Normal);
				File.Delete(file);
			}

			foreach (var dir in dirs)
			{
				SafeDelete(dir);
			}

			Directory.Delete(foldetPath, false);
		}
	}
}