namespace Common.FileSystem
{
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	internal sealed class FileSystemInfoProvider: IFileSystemInfoProvider
	{
		/// <summary>
		///   Enumerates the files in directory.
		/// </summary>
		/// <param name="directoryPath">The directory path.</param>
		/// <returns>A collection of paths to all files</returns>
		public IEnumerable<string> EnumerateFilesInDirectory(string directoryPath)
			=> Directory.EnumerateFiles(directoryPath, "*.*", SearchOption.AllDirectories);

		/// <summary>
		///   Calculates the size of the directory.
		/// </summary>
		/// <param name="directoryPath">The directory path.</param>
		/// <returns>The directory size in bytes.</returns>
		public long CalculateDirectorySize(string directoryPath) =>
			EnumerateFilesInDirectory(directoryPath)
				.Select(file => new FileInfo(file))
				.Select(info => info.Length)
				.Sum();
	}
}