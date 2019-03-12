namespace Modules.Core.Contracts.UI.Dto
{
	using System.Runtime.Serialization;

	[DataContract]
	public enum SettingOwnerDto
	{
		/// <summary>
		///   The undefined level. For serialization purpose.
		/// </summary>
		[EnumMember]
		Undefined = 0,

		/// <summary>
		///   The system level.
		/// </summary>
		[EnumMember]
		System = 1,

		/// <summary>
		///   The user level.
		/// </summary>
		[EnumMember]
		User = 2,

		/// <summary>
		///   The project level.
		/// </summary>
		[EnumMember]
		Project = 3,

		/// <summary>
		///   The task level.
		/// </summary>
		[EnumMember]
		Task = 4
	}
}