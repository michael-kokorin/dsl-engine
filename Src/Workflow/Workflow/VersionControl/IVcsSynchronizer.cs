namespace Workflow.VersionControl
{
	/// <summary>
	/// Provides methods to sync with VCS.
	/// </summary>
	internal interface IVcsSynchronizer
	{
		int Synchronize();
	}
}