namespace Common.FileSystem
{
	using System;
	using System.IO;
	using System.Reflection;

	using Common.Extensions;
	using Common.Properties;

	/// <summary>
	///   Provides methods to load files.
	/// </summary>
	public static class FileLoader
	{
		/// <summary>
		///   Loads file from the resource.
		/// </summary>
		/// <param name="fileName">Name of the file.</param>
		/// <returns>The content of the file.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="fileName"/> is <see langword="null"/> or empty.</exception>
		/// <exception cref="ArgumentException">File with <paramref name="fileName"/> name is not found in resources.</exception>
		public static string FromResource(string fileName)
		{
			if(string.IsNullOrEmpty(fileName))
				throw new ArgumentNullException(nameof(fileName));

			var assembly = Assembly.GetCallingAssembly();

			var manifestStream = assembly.GetManifestResourceStream(fileName);

			if(manifestStream == null)
				throw new ArgumentException(Resources.ResourceFileNotFound.FormatWith(fileName));

			var textStreamReader = new StreamReader(manifestStream);

			var result = textStreamReader.ReadToEnd();

			return result;
		}
	}
}