namespace SolutionTests
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Xml;

	internal static class TestUtils
	{
		public static string GetSolutionPath(string subDirectory = "") =>
			Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..")) +
			Path.DirectorySeparatorChar + subDirectory;

		public static Dictionary<string, string> ParseXmlAndReturnTranslationPropertyValues(string mainResource)
		{
			var values = new Dictionary<string, string>();
			var doc = new XmlDocument();
			doc.Load(mainResource);
			var rootNode = doc.SelectNodes("root")[0];
			var inrootNodes = rootNode.ChildNodes;
			foreach (var inrootNode in inrootNodes)
			{
				var node = inrootNode as XmlNode;
				if(node.Name != "data") continue;

				var propertyName = node.Attributes[0].Value;
				var propertyValue = node.InnerText.Trim();
				values.Add(propertyName, propertyValue);
			}

			return values;
		}
	}
}