namespace Modules.Core.Contracts.UI.Dto
{
	using System.Runtime.Serialization;

	/// <summary>
	/// Represents status of task resolution.
	/// </summary>
	[DataContract(Name = "TaskResolutionStatus")]
	public enum TaskResolutionStatusDto
	{
		/// <summary>
		/// The new resolution.
		/// </summary>
		[EnumMember]
		New = 0,

		/// <summary>
		/// The in progress resolution.
		/// </summary>
		[EnumMember]
		InProgress = 1,

		/// <summary>
		/// The error resolution.
		/// </summary>
		[EnumMember]
		Error = 2,

		/// <summary>
		/// The cancelled resolution.
		/// </summary>
		[EnumMember]
		Cancelled = 3,

		/// <summary>
		/// The successful resolution.
		/// </summary>
		[EnumMember]
		Successful = 4
	}
}