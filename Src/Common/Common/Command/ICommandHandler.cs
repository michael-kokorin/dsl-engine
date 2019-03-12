namespace Common.Command
{
	/// <summary>
	///   Handles the specified command.
	/// </summary>
	/// <typeparam name="T">Type of command.</typeparam>
	public interface ICommandHandler<in T> where T: ICommand
	{
		/// <summary>
		///   Processes the specified command.
		/// </summary>
		/// <param name="command">The command.</param>
		void Process(T command);
	}
}