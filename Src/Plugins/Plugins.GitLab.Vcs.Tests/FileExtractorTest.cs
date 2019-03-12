namespace Plugins.GitLab.Vcs.Tests
{
	using NUnit.Framework;

	[TestFixture]
	[Ignore]
	public sealed class FileExtractorTest
	{
		[Test]
		public void Extract()
		{
			const string targetPath = "e:\\temp\\gitlab";

			const string filePath = "e:\\temp\\gitlab\\temp.zip";

			FileExtractor.Extract(filePath, targetPath);
		}
	}
}