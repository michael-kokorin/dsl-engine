namespace Infrastructure.Tests
{
	using System.Linq;

	using FluentAssertions;

	using Moq;

	using NUnit.Framework;

	using Repository.Context;
	using Repository.Repositories;

	[TestFixture]
	public sealed class DatabaseConfigurationProviderTest
	{
		[SetUp]
		public void SetUp()
		{
			_configurationRepository = new Mock<IConfigurationRepository>();

			_target = new DatabaseConfigurationProvider(_configurationRepository.Object);
		}

		private const string Key = "key name";
		private const string Value = "key value";

		private IConfigurationProvider _target;

		private Mock<IConfigurationRepository> _configurationRepository;

		[Test]
		public void ShouldReturnConfigurationValue()
		{
			_configurationRepository
				.Setup(_ => _.GetByKey(Key))
				.Returns(new[]
				{
					new Configuration
					{
						Value = Value
					}
				}.AsQueryable());

			var result = _target.GetValue(Key);

			result.ShouldBeEquivalentTo(Value);
		}

		[Test]
		public void ShouldSaveNewConfigurationValue()
		{
			_target.SetValue(Key, Value);

			_configurationRepository.Verify(_ =>
				_.Insert(It.Is<Configuration>(c => (c.Name == Key) && (c.Value == Value))),
				Times.Once);

			_configurationRepository.Verify(_ => _.Save(), Times.Once);
		}

		[Test]
		public void ShouldUpdateExistsConfigurationValue()
		{
			var config = new Configuration();

			_configurationRepository
				.Setup(_ => _.GetByKey(Key))
				.Returns(new[]
				{
					config
				}.AsQueryable());

			_target.SetValue(Key, Value);

			config.Value.ShouldBeEquivalentTo(Value);

			_configurationRepository.Verify(_ => _.Save(), Times.Once);
		}
	}
}