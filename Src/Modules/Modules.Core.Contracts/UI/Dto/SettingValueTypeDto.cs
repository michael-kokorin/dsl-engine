namespace Modules.Core.Contracts.UI.Dto
{
	using System.Runtime.Serialization;

	/// <summary>
	/// Indicates type of setting value.
	/// </summary>
	[DataContract]
	public enum SettingValueTypeDto
	{
		/// <summary>
		///   The undefined type. For serialization purpose.
		/// </summary>
		[EnumMember]
		Undefined = 0,

		/// <summary>
		///   The text
		/// </summary>
		[EnumMember]
		Text = 1,

		/// <summary>
		///   The password
		/// </summary>
		[EnumMember]
		Password = 2,

		/// <summary>
		///   The boolean
		/// </summary>
		[EnumMember]
		Boolean = 3,

		/// <summary>
		///   The number
		/// </summary>
		[EnumMember]
		Number = 4,

		/// <summary>
		///   The list
		/// </summary>
		[EnumMember]
		List = 5
	}
}