namespace Workflow.Tests.Event
{
	using System.Linq;

	using FluentAssertions;

	using Moq;

	using NUnit.Framework;

	using Infrastructure.Events;
	using Repository.Repositories;
	using Workflow.Event;

	using Events = Repository.Context.Events;

	[TestFixture]
	public sealed class EventProcessorTest
	{
		private const string EventKey = "event_key";

		private const long EventId = 23452;

		private IEventProcessor _target;

		private Mock<IEventHandlerDispatcher> _eventHandlerDispatcher;

		private Mock<IEventProvider> _eventProvider;

		private Mock<IEventRepository> _eventRepository;

		private Event _event;

		private Events _eventDb;

		[SetUp]
		public void SetUp()
		{
			_eventHandlerDispatcher = new Mock<IEventHandlerDispatcher>();
			_eventProvider = new Mock<IEventProvider>();
			_eventRepository = new Mock<IEventRepository>();

			_event = new Event
			{
				Key = EventKey
			};

			_eventDb = new Events
			{
				Id = EventId
			};

			_target = new EventProcessor(
				_eventHandlerDispatcher.Object,
				_eventProvider.Object,
				_eventRepository.Object);
		}

		[Test]
		public void ShouldReturnFalseWhenNextEventNotFound()
		{
			var result = _target.ProcessNextEvent();

			result.Should().BeFalse();
		}

		[Test]
		public void ShouldThrowUnknownEventException()
		{
			SetupEventProvider();

			Assert.Throws<UnknownEventException>(() => _target.ProcessNextEvent());
		}

		private void SetupEventProvider() => _eventProvider
			.Setup(_ => _.GetNext())
			.Returns(_event);

		[Test]
		public void ShouldReturnTrueOnWhenEventRulesNotExists()
		{
			SetupEventProvider();

			SetupRepository();

			var result = _target.ProcessNextEvent();

			result.Should().BeTrue();
		}

		private void SetupRepository() => _eventRepository
			.Setup(_ => _.GetByKey(EventKey))
			.Returns(new[] { _eventDb }.AsQueryable());
	}
}