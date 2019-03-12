namespace PackageVersionsHelper
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Xml.Linq;

	using Common.Extensions;
	using PackageVersionsHelper.Properties;

	internal static class Program
	{
		private static void Main()
		{
			var mainFolder = Path.IsPathRooted(Settings.Default.RootFolderPath)
				? Settings.Default.RootFolderPath
				: Path.Combine(Environment.CurrentDirectory, Settings.Default.RootFolderPath);

			var dictionary = new Dictionary<string, Tuple<string, string>>();

			var files = Directory.EnumerateFiles(mainFolder,
				Settings.Default.PackageConfigurationFileName,
				SearchOption.AllDirectories);

			foreach (var file in files)
			{
				var doc = XDocument.Load(file);

				if (doc.Root == null) continue;

				var rootElements = doc.Root.Elements();

				foreach (var element in rootElements)
				{
					var id = element.Attribute(Settings.Default.IdAttributeName);

					var version = element.Attribute(Settings.Default.VersionAttributeName);

					if (dictionary.ContainsKey(id.Value))
					{
						var existingValue = dictionary[id.Value];

						if (existingValue.Item1 == version.Value) continue;

						Console.WriteLine(
							Resources.PackagesCollisionInfo.FormatWith(
								id,
								existingValue.Item1,
								existingValue.Item2.Replace(mainFolder, string.Empty),
								version.Value,
								file.Replace(mainFolder, string.Empty)));

						dictionary[id.Value] = string.Compare(existingValue.Item1, version.Value, StringComparison.Ordinal) == 1
							? existingValue
							: new Tuple<string, string>(version.Value, file);
					}
					else
						dictionary.Add(id.Value, new Tuple<string, string>(version.Value, file));
				}
			}

			var outputFile = Path.Combine(mainFolder, Settings.Default.OutputFileName);

			using (var writer = new StreamWriter(outputFile, false))
			{
				var messages = dictionary
					.OrderBy(x => x.Key)
					.Select(item => Resources.RowTemplate.FormatWith(item.Key, item.Value.Item1));

				foreach (var message in messages)
					writer.WriteLine(message);

				writer.Flush();
			}

			Console.WriteLine(Resources.EndMessage);
			Console.ReadLine();
		}
	}
}