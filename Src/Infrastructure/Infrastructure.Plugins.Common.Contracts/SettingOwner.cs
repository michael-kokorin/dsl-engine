namespace Infrastructure.Plugins.Common.Contracts
{
	using JetBrains.Annotations;

	/// <summary>
	///   Indicates setting level.
	/// </summary>
	public enum SettingOwner
	{
		/// <summary>
		///   The undefined level. For serialization purpose.
		/// </summary>
		[UsedImplicitly]
		Undefined = 0,

		/// <summary>
		///   The system level.
		/// </summary>
		System = 1,

		/// <summary>
		///   The user level.
		/// </summary>
		User = 2,

		/// <summary>
		///   The project level.
		/// </summary>
		Project = 3,

		/// <summary>
		///   The task level.
		/// </summary>
		Task = 4
	}
}