namespace Plugins.Ftp.Tests
{
	using System.Collections.Generic;
	using System.IO;

	using FluentAssertions;

	using NUnit.Framework;

	using Infrastructure.Plugins.Contracts;

	[TestFixture]
	[Ignore("IntegrationTests")]
	public sealed class FtpVcsPluginTests
	{
		private IVersionControlPlugin _ftpPlugin;

		[SetUp]
		public void SetUp()
		{
			_ftpPlugin = new FtpVcsPlugin();

			_ftpPlugin.LoadSettingValues(new Dictionary<string, string>
			{
				{
					FtpSettings.UserName.ToString(),
					"ftpuser"
				},
				{
					FtpSettings.UserPassword.ToString(),
					"P@ssw0rd"
				},
				{
					FtpSettings.HostUri.ToString(),
					"localhost:21"
				},
				{
					FtpSettings.SslEnabled.ToString(),
					false.ToString()
				}
			});
		}

		[Test]
		public void ShouldGetFtpDirectoriesList()
		{
			var list = _ftpPlugin.GetBranches();

			list.Should().NotBeNullOrEmpty();
		}

		[Test]
		public void ShouldCreateFtpFolder()
		{
			const string rootFolder = "Folder 2";
			const string newFolder = "DelMeNow";

			string newFolderPath = $"{rootFolder}/{newFolder}";

			var folderInfo = _ftpPlugin.CreateBranch("e:\\temp\\download\\", newFolder, rootFolder);

			folderInfo.Should().NotBeNull();
			folderInfo.Id.ShouldBeEquivalentTo(newFolderPath);
			folderInfo.IsDefault = false;
			folderInfo.Name.ShouldBeEquivalentTo(newFolderPath);
		}

		[Test]
		public void ShouldUploadFileToTheFtp()
		{
			const string folderPath = @"E:\Temp\download";

			const string fileName = @"Test.txt";

			var filePath = Path.Combine(folderPath, fileName);

			var fileContent = File.ReadAllBytes(filePath);

			_ftpPlugin.Commit(folderPath, "Folder 2", "halo", fileName, fileContent);
		}

		[Test]
		public void ShouldDownloadSources() => _ftpPlugin.GetSources("Folder 2", "e:\\temp\\download");
	}
}