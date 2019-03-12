namespace Infrastructure.Tests.Templates
{
    using System.Collections.Generic;

    using FluentAssertions;

    using JetBrains.Annotations;

    using NUnit.Framework;

    using Infrastructure.Templates;

    [TestFixture]
    public sealed class Antlr4TemplateTest
    {
        private ITemplate _target;

        [SetUp]
        public void SetUp() => _target = new Antlr4Template("Hello, $user.name$");

        [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
        public sealed class User
        {
            public string Name => "John Doe";
        }

        [Test]
        public void ShouldRenderClassProperty()
        {
            _target.Add(new Dictionary<string, object>
            {
                {"user", new User()}
            });

            var result = _target.Render();

            result.Should().NotBeNullOrEmpty();
            result.ShouldBeEquivalentTo("Hello, John Doe");
        }
    }
}