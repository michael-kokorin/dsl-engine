namespace Common.FileSystem
{
	using System.Collections.Generic;

	using JetBrains.Annotations;

	/// <summary>
	///   Provides information from file system.
	/// </summary>
	public interface IFileSystemInfoProvider
	{
		/// <summary>
		///   Calculates the size of the directory.
		/// </summary>
		/// <param name="directoryPath">The directory path.</param>
		/// <returns>The directory size in bytes.</returns>
		long CalculateDirectorySize([NotNull] [PathReference] string directoryPath);

		/// <summary>
		///   Enumerates the files in directory.
		/// </summary>
		/// <param name="directoryPath">The directory path.</param>
		/// <returns>A collection of paths to all files</returns>
		IEnumerable<string> EnumerateFilesInDirectory([NotNull] [PathReference] string directoryPath);
	}
}