namespace Modules.Core.Environment
{
	/// <summary>
	///   Provides methods to prepare environment.
	/// </summary>
	internal interface IEnvironmentProvider
	{
		/// <summary>
		///   Prepares environment.
		/// </summary>
		void Prepare();
	}
}