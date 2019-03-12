namespace Modules.SA.Config
{
	using System;
	using System.IO;

	using JetBrains.Annotations;

	internal sealed class TextFile
	{
		public static bool TryRead([NotNull] string filePath, out string content)
		{
			if (filePath == null) throw new ArgumentNullException(nameof(filePath));

			content = null;

			try
			{
				content = File.ReadAllText(filePath);

				return true;
			}
			catch
			{
				return false;
			}
		}

		public static void Write(string filePath, string content) => File.WriteAllText(filePath, content);
	}
}