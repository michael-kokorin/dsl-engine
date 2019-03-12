namespace Common.Enums
{
	using JetBrains.Annotations;

	/// <summary>
	///   Indicates notification protocol type.
	/// </summary>
	public enum NotificationProtocolType
	{
		/// <summary>
		///   The undefined notification protocol type.
		/// </summary>
		[UsedImplicitly]
		Undefined = 0,

		/// <summary>
		///   The email.
		/// </summary>
		Email = 1
	}
}