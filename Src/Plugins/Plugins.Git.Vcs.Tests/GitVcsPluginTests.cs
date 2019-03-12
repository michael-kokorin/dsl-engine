namespace Plugins.Git.Vcs.Tests
{
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	using FluentAssertions;
	using NUnit.Framework;

	using Infrastructure.Plugins.Contracts;

	[TestFixture]
	[Ignore("Integration tests")]
	public sealed class GitVcsPluginTests
	{
		private IVersionControlPlugin _target;

		[SetUp]
		public void SetUp()
		{
			_target = new GitVcsPlugin();

			LoadGitLabSettings();
		}

		private void LoadGitLabSettings() =>
			_target.LoadSettingValues(new Dictionary<string, string>
			{
				{GitSettingKeys.Url.ToString(), "https://gitlab.com/msharonov/Test.git"},
				{GitSettingKeys.Username.ToString(), "msharonov"},
				{GitSettingKeys.Password.ToString(), "Adm1nR!VCN"},
				{GitSettingKeys.Email.ToString(), "mail@mail.mail"}
			});

		[Test]
		public void ShouldResolveBranchesFromGitLab()
		{
			var branches = _target.GetBranches();

			branches.Count().ShouldBeEquivalentTo(1);
		}

		private const string TempPath = "E:\\Temp\\GitLab\\";

		[Test]
		public void ShouldCreateBranchInGitLab()
		{
			var branches = _target.GetBranches();

			var testBranch = branches.Single(_ => _.Name == "master");

			_target.GetSources(testBranch.Id, TempPath);

			var result = _target.CreateBranch(TempPath, "DelMe", testBranch.Id);

			result.Should().NotBeNull();
		}

		[Test]
		public void ShouldGetCommitsList()
		{
			var commits = _target.GetCommits();

			commits.Should().BeNullOrEmpty();
		}

		[Test]
		public void ShouldCommitChanges()
		{
			var filePath = Path.Combine(TempPath, "DelMe", "Web.config");

			var content = File.ReadAllBytes(filePath);

			_target.Commit(TempPath, "DelMe", "Halo", "Web.config", content);
		}
	}
}