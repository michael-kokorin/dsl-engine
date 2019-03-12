namespace Common.Command
{
	/// <summary>
	///   Provides method to find command handler of specific type.
	/// </summary>
	internal interface ICommandHandlerProvider
	{
		/// <summary>
		///   Resolves command handler of specific type.
		/// </summary>
		/// <typeparam name="T">Type of command to handle.</typeparam>
		/// <returns>Command handler.</returns>
		ICommandHandler<T> Resolve<T>() where T: class, ICommand;
	}
}