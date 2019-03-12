namespace Plugins.SharedFolder.Tests
{
	using System.Collections.Generic;

	using FluentAssertions;

	using NUnit.Framework;

	using Infrastructure.Plugins.Contracts;

	[TestFixture]
	[Ignore]
	public sealed class SharedFolderVcsPluginTest
	{
		[SetUp]
		public void SetUp()
		{
			_target = new SharedFolderVcsPlugin();

			_target.LoadSettingValues(
				new Dictionary<string, string>
				{
					{
						SharedFolderSettings.FolderUri,
						@"\\msharonov.ptsecurity.ru\sharedfolder"
					}
				});
		}

		private IVersionControlPlugin _target;

		[Test]
		public void ShouldCreateBranchOnSharedFolder()
		{
			const string branchName = "Vulnerability-XSS";

			var result = _target.CreateBranch(null, branchName, @"\Sources\Mopas");

			result.Id.ShouldBeEquivalentTo(branchName);
			result.Name.ShouldBeEquivalentTo(branchName);
		}

		[Test]
		public void ShouldDownloadSources() => _target.GetSources(@"\Sources\Mopas", @"e:\download\mopas");

		[Test]
		public void ShouldGetSharedFolderSubfolders()
		{
			var branches = _target.GetBranches();

			branches.Should().NotBeEmpty();
		}
	}
}