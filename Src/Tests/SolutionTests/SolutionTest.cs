namespace SolutionTests
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	using NUnit.Framework;

	[TestFixture]
	public sealed class SolutionTest
	{
		[Test]
		public void TestAllTranslationStringsAreUsed()
		{
			var solutionPath = new DirectoryInfo(TestUtils.GetSolutionPath());
			var allResourcesFiles = solutionPath.GetFiles("Resources.resx", SearchOption.AllDirectories);
			var allFiles =
				Directory.GetFiles(solutionPath.FullName, "*.*", SearchOption.AllDirectories)
								.Where(
									s =>
									s.EndsWith(".xaml", StringComparison.Ordinal) || s.EndsWith(".cs", StringComparison.Ordinal) ||
									s.EndsWith(".cshtml", StringComparison.Ordinal))
								.Where(o => !o.Contains("Designer"));

			var excludedList =
				new[]
				{
					".git\\",
					"packages\\",
					"obj\\",
					"bin\\",
					"$tf\\",
					"_ReSharper.Caches\\"
				};

			var resourcesNotInExludedList =
				allResourcesFiles.Select(allTestProject => new {allTestProject, project = allTestProject})
												.Select(
													testProject =>
													new {project = testProject, didcontain = excludedList.Any(s => testProject.project.FullName.Contains(s))})
												.Where(testProject => !testProject.didcontain)
												.Select(testProject => testProject.project.allTestProject.FullName);

			var allSourceFiles =
				allFiles.Select(Path.GetFullPath).Where(file => !excludedList.Where(file.Contains).Any()).ToList();

			var badlist = new List<string>();

			foreach(var resFile in resourcesNotInExludedList)
			{
				var entries = TestUtils.ParseXmlAndReturnTranslationPropertyValues(resFile);
				foreach(var entry in entries)
				{
					var wasUsed = allSourceFiles.Select(File.ReadAllText).Any(alltext => alltext.Contains(entry.Key));

					// TODO this is for static localization of tables - remove in future
					if(entry.Key.StartsWith("Property_", StringComparison.Ordinal))
						continue;

					if(entry.Key.StartsWith("Enum_", StringComparison.Ordinal))
						continue;

					if(!wasUsed)
						badlist.Add(resFile + " -  " + entry.Key);
				}
			}

			if(!badlist.Any()) return;

			var badEntryList = badlist.Aggregate(string.Empty, (current, bad) => current + bad + Environment.NewLine);
			Assert.True(false, "Some translations were not used: " + badEntryList);
		}

		/// <summary>
		///   Tests that L10N tables are not in database context.
		/// </summary>
		[Test]
		public void TestThatForbiddenTablesAreNotInContext()
		{
			var forbiddenTables =
				new[]
				{
					"l10n"
				};

			var solutionPath = new DirectoryInfo(TestUtils.GetSolutionPath());
			var contextPath = Path.Combine(solutionPath.FullName, "Data\\Repository\\Context");
			var forbiddenFiles = Directory.EnumerateFiles(contextPath, "*.cs").Where(_ =>
			{
				var name = Path.GetFileNameWithoutExtension(_) ?? string.Empty;
				return forbiddenTables.Any(table => name.Contains(table));
			}).ToArray();

			Assert.True(
				forbiddenFiles.Length == 0,
				"The following tables are imported but shouldn't: " +
				string.Join(", ", forbiddenFiles.Select(Path.GetFileNameWithoutExtension)));
		}

		/// <summary>
		///   Tests that forbidden entities are not in database context.
		/// </summary>
		[Test]
		public void TestThatForbiddenEntitiesAreNotInContext()
		{
			var forbiddenEntities =
				new[]
				{
					"_Result"
				};

			var solutionPath = new DirectoryInfo(TestUtils.GetSolutionPath());
			var contextPath = Path.Combine(solutionPath.FullName, "Data\\Repository\\Context");
			var forbiddenFiles = Directory.EnumerateFiles(contextPath, "*.cs").Where(_ =>
			{
				var name = Path.GetFileNameWithoutExtension(_) ?? string.Empty;
				return forbiddenEntities.Any(entity => name.Contains(entity));
			}).ToArray();

			Assert.True(
				forbiddenFiles.Length == 0,
				"The following entities are imported but shouldn't: " +
				string.Join(", ", forbiddenFiles.Select(Path.GetFileNameWithoutExtension)));
		}

		/// <summary>
		/// Tests that SLN file is under version control.
		/// </summary>
		[Test]
		public void TestThatSlnFileIsUnderVersionControl()
		{
			var solutionPath = new DirectoryInfo(TestUtils.GetSolutionPath());
			var solutionFile = Path.Combine(solutionPath.FullName, "sln");

			string content;
			using(var streamReader = new StreamReader(solutionFile))
			{
				content = streamReader.ReadToEnd();
			}

			Assert.True(content.Contains("GlobalSection(TeamFoundationVersionControl)"), "Solution file is not under source control");
		}
	}
}