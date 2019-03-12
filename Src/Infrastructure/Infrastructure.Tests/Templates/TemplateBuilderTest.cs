namespace Infrastructure.Tests.Templates
{
	using FluentAssertions;

	using NUnit.Framework;

	using Infrastructure.Templates;

	[TestFixture]
	public sealed class TemplateBuilderTest
	{
		private ITemplateBuilder _target;

		[SetUp]
		public void SetUp() => _target = new TemplateBuilder();

		[Test]
		public void ShouldReturnTemplateWithTitle()
		{
			const string titleTemplate = "title";

			const string bodyTemplate = "body";

			var result = _target.Build(titleTemplate, bodyTemplate);

			result.Should().NotBeNull();
			result.Should().BeOfType<TemplateWithTitle>();
		}
	}
}