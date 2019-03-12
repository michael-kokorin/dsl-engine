namespace Plugins.Tfs.Vcs.Tests
{
	using System;
	using System.Collections.Generic;
	using System.Text;

	using FluentAssertions;

	using NUnit.Framework;

	using Infrastructure.Plugins.Contracts;

	[TestFixture]
	[Ignore]
	public sealed class TfsVcsPluginTest
	{
		[SetUp]
		public void SetUp()
		{
			_target = new TfsVcsPlugin();

			_target.LoadSettingValues(
				new Dictionary<string, string>
				{
					{
						TfsSettings.UserName.ToString(),
						"msharonov"
					},
					{
						TfsSettings.UserPassword.ToString(),
						"P@ssw0rd"
					},
					{
						TfsSettings.HostName.ToString(),
						"https://veises.visualstudio.com/DefaultCollection/"
					}
				});
		}

		private IVersionControlPlugin _target;

		[Test]
		public void GetChanges()
		{
			var changes = _target.GetCommits(DateTime.Now.AddHours(-1), DateTime.Now);

			changes.Should().BeNullOrEmpty();
		}

		[Test]
		public void ShouldCommitChangesToTfs()
		{
			const string branch = "$/Mopas/sources/master";

			const string featureBranch = "vuln";

			const string folderPath = @"e:\temp\del_me_now\";

			_target.GetSources(branch, folderPath);

			var branchId = _target.CreateBranch(folderPath, featureBranch, branch).Id;

			var message = DateTime.UtcNow.ToString("F");

			var content = Encoding.UTF8.GetBytes(message);

			_target.Commit(folderPath, branchId, message, "Web.config", content);
		}

		[Test]
		public void ShouldCreateBranch()
		{
			var result = _target.CreateBranch("halo", "$/Mopas/sources/master");

			result.Should().NotBeNull();
		}

		[Test]
		public void ShouldGetBranchesList()
		{
			var branchesList = _target.GetBranches();

			branchesList.Should().NotBeNullOrEmpty();
		}

		[Test]
		public void ShouldGetUserNameFromTfs()
		{
			var userInfo = _target.GetCurrentUser();

			userInfo.Should().NotBeNull();
		}

		[Test]
		public void ShouldLoadSourcesFromTfs() => _target.GetSources("$/Mopas/sources/master", @"e:\temp\del_me_now\");
	}
}