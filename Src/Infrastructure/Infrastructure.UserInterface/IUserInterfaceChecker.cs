namespace Infrastructure.UserInterface
{
	/// <summary>
	///   Ckecks the User interface module versiion
	/// </summary>
	public interface IUserInterfaceChecker
	{
		/// <summary>
		///   Checks the specified version.
		/// </summary>
		/// <param name="host">User interface host address</param>
		/// <param name="version">The User interface module version.</param>
		void Check(string host, string version);
	}
}