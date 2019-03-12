namespace Plugins.GitLab.Vcs
{
	using System;

	using System.IO;
	using System.IO.Compression;

	using JetBrains.Annotations;

	public sealed class FileExtractor
	{
		public static void Extract([NotNull] string archiveFileName, [NotNull] string targetPath)
		{
			if (archiveFileName == null) throw new ArgumentNullException(nameof(archiveFileName));
			if (targetPath == null) throw new ArgumentNullException(nameof(targetPath));

			// by default, sources downloads in arvice with inner root folder with name:
			// digital_dept-0e79ff112daeb3104b49ecd5b8d528175b187aaf-0e79ff112daeb3104b49ecd5b8d528175b187aaf
			//
			// so, we have to extract sources from that inner folder

			using (var archive = ZipFile.OpenRead(archiveFileName))
			{
				foreach (var entry in archive.Entries)
				{
					var index = entry.FullName.IndexOf('/');

					if ((index > 0) && (index < entry.FullName.Length - 1))
					{
						var entryName = entry.FullName.Substring(index + 1);

						var entryPath = Path.Combine(targetPath, entryName);

						entryPath = entryPath.Replace('/', Path.DirectorySeparatorChar);

						if (entryPath.EndsWith("\\", StringComparison.Ordinal))
						{
							Directory.CreateDirectory(entryPath);
						}
						else
						{
							entry.ExtractToFile(entryPath, true);
						}
					}
				}
			}
		}
	}
}