namespace Infrastructure.Tests.Tags
{
	using NUnit.Framework;

	using Infrastructure.Tags;

	[TestFixture]
	public sealed class TagValidatorTest
	{
		private ITagValidator _target;

		[SetUp]
		public void SetUp() => _target = new TagValidator();

		[TestCase("Halo")]
		[TestCase("Хало")]
		[TestCase("Halo1")]
		[TestCase("Ha lo")]
		[TestCase("Halo#", ExpectedException = typeof(IncorrectTagNameException))]
		[TestCase("Ha", ExpectedException = typeof(IncorrectTagLengthException))]
		[TestCase("HaloHaloHaloHaloHalo", ExpectedException = typeof(IncorrectTagLengthException))]
		public void ValidateTagName(string value) =>
			_target.Validate(value);
	}
}