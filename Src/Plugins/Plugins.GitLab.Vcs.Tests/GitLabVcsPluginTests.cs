namespace Plugins.GitLab.Vcs.Tests
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.IO;

	using FluentAssertions;

	using NUnit.Framework;

	using Infrastructure.Plugins.Contracts;

	[TestFixture]
	[Ignore("Integration tests")]
	public sealed class GitLabVcsPluginTests
	{
		private IVersionControlPlugin _target;

		[SetUp]
		public void SetUp()
		{
			_target = new GitLabVcsPlugin();

			_target.LoadSettingValues(new Dictionary<string, string>
			{
				{
					GitLabSetting.Token.ToString(),
					"z_F1h_F3JRxPThbhYBoZ"
				},
				{
					GitLabSetting.RepositoryName.ToString(),
					"Test"
				},
				{
					GitLabSetting.RepositoryOwner.ToString(),
					"msharonov"
				},
				{
					GitLabSetting.Host.ToString(),
					"https://gitlab.com"
				}
			});
		}

		[Test]
		public void ShouldGetCurrentGitLabUser()
		{
			var currentUser = _target.GetCurrentUser();

			currentUser.Should().NotBeNull();
		}

		[Test]
		public void ShouldGetCommitsFromGitLab()
		{
			var commits = _target.GetCommits();

			commits.Should().NotBeNullOrEmpty();
		}

		[Test]
		public void ShouldGetBranchesFromGitLab()
		{
			var branches = _target.GetBranches();

			branches.Should().NotBeNullOrEmpty();
		}

		[Test]
		public void ShouldCreateBranchInGitLab()
		{
			const string branchName = "testBranch";

			var branch = _target.CreateBranch("c:\\e", branchName, "master");

			branch.Should().NotBeNull();

			branch.Name.Should().BeEquivalentTo(branchName);
		}

		const string TempPath = "e:\\Temp\\GitLab";

		[Test]
		public void ShouldDownloadSourcesFromGitLab() => _target.GetSources("master", TempPath);

		[Test]
		public void ShouldUpdateFile()
		{
			const string fileName = "Tests\\1 INPUT DATA VERIFICATION\\9 LDAP Injection\\Ldap.aspx.cs";

			var filePath = Path.Combine(TempPath, fileName);

			var fileContent = File.ReadAllBytes(filePath);

			_target.Commit(TempPath, "master", DateTime.UtcNow.ToString(CultureInfo.InvariantCulture), fileName, fileContent);
		}
	}
}