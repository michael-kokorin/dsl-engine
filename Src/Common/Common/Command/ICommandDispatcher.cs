namespace Common.Command
{
	/// <summary>
	///   Provides handler for the specified type of command.
	/// </summary>
	public interface ICommandDispatcher
	{
		/// <summary>
		///   Handles the specified command.
		/// </summary>
		/// <typeparam name="T">Type of command.</typeparam>
		/// <param name="command">The command.</param>
		void Handle<T>(T command) where T: class, ICommand;
	}
}