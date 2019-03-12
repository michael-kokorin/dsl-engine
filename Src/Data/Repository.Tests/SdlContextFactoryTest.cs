namespace Repository.Tests
{
	using FluentAssertions;

	using NUnit.Framework;

	using Repository.Context;

	[TestFixture]
	public sealed class SdlContextFactoryTest
	{
		[Test]
		public void CreateMethod()
		{
			var instance = new SdlContextFactory();

			var result = instance.GetContext();

			result.Should().NotBeNull();
			result.Should().BeOfType<SdlContext>();
		}
	}
}