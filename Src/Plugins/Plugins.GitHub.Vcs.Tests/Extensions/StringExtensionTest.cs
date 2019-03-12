namespace Plugins.GitHub.Vcs.Tests.Extensions
{
    using FluentAssertions;

    using NUnit.Framework;

    using Plugins.GitHub.Vcs.Extensions;

    [TestFixture]
    public sealed class StringExtensionTest
    {
        [Test]
        public void ShouldReplaceBackwardToForwardSlash()
        {
            const string source = @"test\test";

            var result = source.ToGitPath();

            result.Should().NotBeNullOrEmpty();
            result.Should().BeEquivalentTo(source.Replace('\\', '/'));
        }
    }
}