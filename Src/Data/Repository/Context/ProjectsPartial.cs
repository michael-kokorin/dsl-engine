namespace Repository.Context
{
	using System;

	/// <summary>
	///   Represents project.
	/// </summary>
	/// <seealso cref="Repository.IEntity"/>
	[ProjectProperty("Id")]
	public partial class Projects: IEntity
	{
		/// <summary>
		///   Sets the VCS last scan.
		/// </summary>
		/// <param name="vcsLastScanUtc">The VCS last scan UTC.</param>
		public void SetVcsLastScan(DateTime vcsLastScanUtc) => VcsLastSyncUtc = vcsLastScanUtc;
	}
}