namespace Repository.Tests
{
	using System.Linq;

	using FluentAssertions;

	using NUnit.Framework;

	using Repository.Context;
	using Repository.Repositories;

	[TestFixture]
	[Ignore("Integration tests")]
	public sealed class UnitOfWorkTest
	{
		[SetUp]
		public void SetUp()
		{
			var contextFactory = new SdlContextFactory();

			_unitOfWork = new UnitOfWork(contextFactory);

			_configurationRepository = new ConfigurationRepository(_unitOfWork);

			var config = _configurationRepository.GetByKey(ConfigKey).SingleOrDefault();

			if(config == null) return;

			_configurationRepository.Delete(config);

			_unitOfWork.Commit();
		}

		private UnitOfWork _unitOfWork;

		private IConfigurationRepository _configurationRepository;

		private const string ConfigKey = "TransactionTestKey";

		private void AddRecordToTable()
		{
			var config = new Configuration
									{
										Name = ConfigKey,
										Value = "TransValue"
									};

			_configurationRepository.Insert(config);
		}

		[Test]
		public void ShouldNotSaveData()
		{
			using(_unitOfWork.BeginTransaction())
				AddRecordToTable();
		}

		[Test]
		public void ShouldSaveData()
		{
			using(var transaction = _unitOfWork.BeginTransaction())
			{
				var config = new Configuration
										{
											Name = ConfigKey,
											Value = "TransValue"
										};

				_configurationRepository.Insert(config);

				_unitOfWork.Commit();

				config.Id.Should().BeGreaterThan(0);

				transaction.Commit();
			}
		}

		[Test]
		public void ShouldSaveDataWithoutTransaction()
		{
			var config = new Configuration
									{
										Name = ConfigKey,
										Value = "TransValue"
									};

			_configurationRepository.Insert(config);

			_unitOfWork.Commit();

			config.Id.Should().BeGreaterThan(0);
		}
	}
}