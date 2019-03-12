namespace Common.Tests.Command
{
	using System;

	using JetBrains.Annotations;

	using Moq;

	using NUnit.Framework;

	using Common.Command;

	[TestFixture]
	public sealed class CommandDispatcherTest
	{
		[SetUp]
		public void SetUp()
		{
			_commandHandlerProvider = new Mock<ICommandHandlerProvider>();

			_commandDispatcher = new CommandDispatcher(_commandHandlerProvider.Object);
		}

		private ICommandDispatcher _commandDispatcher;

		private Mock<ICommandHandlerProvider> _commandHandlerProvider;

		[UsedImplicitly]
		public sealed class TestCommand: ICommand
		{
		}

		[Test]
		public void ProcessCommandHandler()
		{
			var commandHandler = new Mock<ICommandHandler<TestCommand>>();

			_commandHandlerProvider
				.Setup(_ => _.Resolve<TestCommand>())
				.Returns(commandHandler.Object);

			var command = new TestCommand();

			_commandDispatcher.Handle(command);

			_commandHandlerProvider.Verify(_ => _.Resolve<TestCommand>(), Times.Once);
			commandHandler.Verify(_ => _.Process(command), Times.Once);
		}

		[Test]
		public void ShouldThrowExceptionOnHandleCommandEqNull() =>
			Assert.Throws<ArgumentNullException>(() => _commandDispatcher.Handle<TestCommand>(null));

		[Test]
		public void ShouldThrowExceptionOnHandleUnknownCommand()
		{
			_commandHandlerProvider
				.Setup(_ => _.Resolve<TestCommand>())
				.Throws<UnknownCommandException>();

			Assert.Throws<UnknownCommandException>(() => _commandDispatcher.Handle(new TestCommand()));
		}
	}
}