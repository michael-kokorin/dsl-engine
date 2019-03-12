namespace Plugins.GitHub.Vcs.Tests
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	using FluentAssertions;

	using JetBrains.Annotations;

	using NUnit.Framework;

	using Infrastructure.Plugins.Contracts;

	[TestFixture]
	[Ignore("Functional tests. Running on live GitHub environment")]
	public sealed class GitHubVcsPluginTest
	{
		[SetUp]
		public void SetUp()
		{
			_target = new GitHubVcsPlugin();

			_target.LoadSettingValues(GetParameters());
		}

		private const string TestBranchName = "HALO1";

		private IVersionControlPlugin _target;

		[NotNull]
		private static Dictionary<string, string> GetParameters() => new Dictionary<string, string>
																	{
																		{
																			GitHubItSettingKeys.ClientToken.ToString(),
																			"32382b93f4db72e4598e2b5a7a3bf4278352ce09"
																		},
																		{
																			GitHubItSettingKeys.RepositoryOwner.ToString(),
																			"msharonov"
																		},
																		{
																			GitHubItSettingKeys.RepositoryName.ToString(),
																			"MopasSharp"
																		}
																	};

		[Test]
		public void ShouldCommitChages()
		{
			const string folderPath = @"E:\Temp\GitHub\";

			const string fileName = @"E:\Temp\GitHub\ReadMe.txt";

			var filePath = Path.Combine(folderPath, fileName);

			var fileContent = File.ReadAllBytes(filePath);

			_target.Commit(folderPath, TestBranchName, "My great comment", fileName, fileContent);
		}

		[Test]
		public void ShouldCreateNewBranch()
		{
			const string folderPath = @"E:\Temp\GitHub\";

			var result = _target.CreateBranch(folderPath, TestBranchName, "master");

			result.Should().NotBeNull();
			result.Id.Should().BeEquivalentTo(TestBranchName);
			result.Name.Should().BeEquivalentTo(TestBranchName);
		}

		[Test]
		public void ShouldGetBranchesInfo()
		{
			var result = _target.GetBranches()?.ToArray();

			result.Should().NotBeNull();
			result.Should().HaveCount(3);
		}

		[Test]
		public void ShouldGetCommits()
		{
			var commits = _target.GetCommits(new DateTime(2015, 01, 01));

			commits.Should().NotBeNullOrEmpty();
		}

		[Test]
		public void ShouldGetSources() => _target.GetSources("HALO1", @"E:\Temp\GitHub\");
	}
}