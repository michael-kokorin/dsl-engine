namespace Infrastructure.Plugins.Common.Contracts
{
	using JetBrains.Annotations;

	/// <summary>
	///   Indicates type of setting.
	/// </summary>
	public enum SettingType
	{
		/// <summary>
		///   The undefined type. For serialization purpose.
		/// </summary>
		[UsedImplicitly]
		Undefined = 0,

		/// <summary>
		///   The text
		/// </summary>
		Text = 1,

		/// <summary>
		///   The password
		/// </summary>
		Password = 2,

		/// <summary>
		///   The boolean
		/// </summary>
		Boolean = 3,

		/// <summary>
		///   The number
		/// </summary>
		Number = 4,

		/// <summary>
		///   The list
		/// </summary>
		List = 5
	}
}