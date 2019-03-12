namespace Common.FileSystem
{
	using System;
	using System.IO;

	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class FileWriter : IFileWriter
	{
		public void Write([NotNull] string folderPath, [NotNull] string fileName, byte[] content)
		{
			if (folderPath == null) throw new ArgumentNullException(nameof(folderPath));
			if (fileName == null) throw new ArgumentNullException(nameof(fileName));

			var filePath = Path.Combine(folderPath, fileName);

			Write(filePath, content);
		}

		public void Write([NotNull] string filePath, [NotNull] byte[] content)
		{
			if (filePath == null) throw new ArgumentNullException(nameof(filePath));
			if (content == null) throw new ArgumentNullException(nameof(content));

			CreateFolderIfNotExists(filePath);

			File.WriteAllBytes(filePath, content);
		}

		private static void CreateFolderIfNotExists(string filePath)
		{
			var fileInfo = new FileInfo(filePath);

			var folderPath = fileInfo.DirectoryName;

			if (string.IsNullOrEmpty(folderPath))
				throw new ArgumentException(nameof(filePath));

			if (!Directory.Exists(folderPath))
				Directory.CreateDirectory(folderPath);
		}
	}
}