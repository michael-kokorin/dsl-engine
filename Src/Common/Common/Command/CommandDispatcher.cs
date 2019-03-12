namespace Common.Command
{
	using System;

	using JetBrains.Annotations;

	internal sealed class CommandDispatcher: ICommandDispatcher
	{
		private readonly ICommandHandlerProvider _commandHandlerProvider;

		public CommandDispatcher([NotNull] ICommandHandlerProvider commandHandlerProvider)
		{
			if(commandHandlerProvider == null) throw new ArgumentNullException(nameof(commandHandlerProvider));

			_commandHandlerProvider = commandHandlerProvider;
		}

		/// <summary>
		///   Handles the specified command.
		/// </summary>
		/// <typeparam name="T">Type of command.</typeparam>
		/// <param name="command">The command.</param>
		public void Handle<T>(T command) where T: class, ICommand
		{
			if(command == null) throw new ArgumentNullException(nameof(command));

			var commandHandler = _commandHandlerProvider.Resolve<T>();

			if(commandHandler == null) throw new UnknownCommandException();

			commandHandler.Process(command);
		}
	}
}