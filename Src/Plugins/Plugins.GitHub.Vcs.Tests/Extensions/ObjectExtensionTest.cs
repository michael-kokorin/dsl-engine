namespace Plugins.GitHub.Vcs.Tests.Extensions
{
	using System.Collections.Generic;

	using FluentAssertions;

	using NUnit.Framework;

	using Octokit;

	using Plugins.GitHub.Vcs.Extensions;

	[TestFixture]
	public sealed class ObjectExtensionTest
	{
		[Test]
		public void ShouldConvertBranchToBranchInfo()
		{
			const string branchname = "test_branch_name";

			var branch = new Branch(branchname,
															null,
															new BranchProtection(false, new RequiredStatusChecks(EnforcementLevel.Everyone, new List<string>())));

			var result = branch.ToModel();

			result.Should().NotBeNull();
			result.Name.ShouldBeEquivalentTo(branchname);
			result.Id.ShouldBeEquivalentTo(branchname);
		}
	}
}