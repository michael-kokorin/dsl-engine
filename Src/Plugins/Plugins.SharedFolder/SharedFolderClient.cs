namespace Plugins.SharedFolder
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	using JetBrains.Annotations;

	using Plugins.SharedFolder.Extensions;

	internal sealed class SharedFolderClient: IDisposable
	{
		private readonly string _folderPath;

		public SharedFolderClient([NotNull] [PathReference] string folderPath)
		{
			if(folderPath == null)
			{
				throw new ArgumentNullException(nameof(folderPath));
			}

			var folderUri = new Uri(folderPath, UriKind.Absolute);

			_folderPath = folderUri.LocalPath;
		}

		public void Dispose()
		{
			// do nothing
		}

		private static void CopyDirectory(
			[NotNull] [PathReference] string sourcePath,
			[NotNull] [PathReference] string targetPath)
		{
			var sourceDirectories = Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories);

			if(!Directory.Exists(targetPath))
			{
				Directory.CreateDirectory(targetPath);
			}

			sourcePath = sourcePath.TrimEnd('\\', '/');

			foreach(var dirPath in sourceDirectories)
			{
				Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
			}

			var sourceFileNames = Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories);

			foreach(var newPath in sourceFileNames)
			{
				File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
			}
		}

		public void CopyLocal([NotNull] string sourceSubfolder, [NotNull] string destinationSubfolder)
		{
			if(sourceSubfolder == null)
			{
				throw new ArgumentNullException(nameof(sourceSubfolder));
			}
			if(destinationSubfolder == null)
			{
				throw new ArgumentNullException(nameof(destinationSubfolder));
			}

			var destinationPath = Path.Combine(
				_folderPath,
				sourceSubfolder.AsRelative(),
				destinationSubfolder.AsRelative());

			CopyTo(sourceSubfolder, destinationPath);
		}

		public void CopyTo([NotNull] string subfolder, [NotNull] [PathReference] string destinationPath)
		{
			if(subfolder == null)
			{
				throw new ArgumentNullException(nameof(subfolder));
			}
			if(destinationPath == null)
			{
				throw new ArgumentNullException(nameof(destinationPath));
			}

			var sourceFolderpath = Path.Combine(_folderPath, subfolder.AsRelative());

			CopyDirectory(sourceFolderpath, destinationPath);
		}

		public void CreateFile([NotNull] string subfolder, [NotNull] string fileName, [NotNull] byte[] fileContent)
		{
			if(subfolder == null)
			{
				throw new ArgumentNullException(nameof(subfolder));
			}
			if(fileName == null)
			{
				throw new ArgumentNullException(nameof(fileName));
			}

			var filePath = Path.Combine(
				_folderPath,
				subfolder.AsRelative(),
				fileName.AsRelative());

			File.WriteAllBytes(filePath, fileContent);
		}

		[NotNull]
		[ItemNotNull]
		public IEnumerable<string> GetSubfolders() =>
			Directory.GetDirectories(_folderPath, "*", SearchOption.AllDirectories)
					.Select(_ => _.Replace(_folderPath, string.Empty));
	}
}