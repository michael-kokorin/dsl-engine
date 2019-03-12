namespace Common.Tests.Extensions
{
	using FluentAssertions;

	using JetBrains.Annotations;

	using NUnit.Framework;

	using Common.Extensions;

	[TestFixture]
	public sealed class EnumExtensionTest
	{
		public enum Enum1
		{
			One = 1,

			Two = 2
		}

		[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
		private enum Enum2
		{
			One = 1,

			Two = 2
		}

		[Test]
		[TestCase(Enum1.One)]
		[TestCase(Enum1.Two)]
		public void ShouldReturnEqualsByValueEnum(Enum1 enum1)
		{
			var enum2 = enum1.GetEqualByValue<Enum2>();

			enum2.ShouldBeEquivalentTo(enum1);
		}
	}
}