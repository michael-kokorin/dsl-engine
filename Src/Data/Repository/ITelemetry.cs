namespace Repository
{
	using System;

	/// <summary>
	/// Telemetry entity interface
	/// </summary>
	/// <seealso cref="Repository.IEntity" />
	public interface ITelemetry : IEntity
	{
		/// <summary>
		/// Gets or sets the date time UTC.
		/// </summary>
		/// <value>
		/// Operation execution UTC date and time
		/// </value>
		DateTime DateTimeUtc { get; set; }

		/// <summary>
		/// Gets or sets the local date and time.
		/// </summary>
		/// <value>
		/// Operation execution local date and time
		/// </value>
		DateTime DateTimeLocal { get; set; }

		/// <summary>
		/// Gets or sets the entity identifier.
		/// </summary>
		/// <value>
		/// The entity identifier
		/// </value>
		long? EntityId { get; set; }

		/// <summary>
		/// Gets or sets the name of the operation.
		/// </summary>
		/// <value>
		/// The name of the operation.
		/// </value>
		string OperationName { get; set; }

		/// <summary>
		/// Gets or sets the operation duration.
		/// </summary>
		/// <value>
		/// The operation duration in milliseconds.
		/// </value>
		long? OperationDuration { get; set; }

		/// <summary>
		/// Gets or sets the name of the user.
		/// </summary>
		/// <value>
		/// The name of the windows identity user name.
		/// </value>
		string UserLogin { get; set; }

		/// <summary>
		/// Gets the user SID
		/// </summary>
		/// <value>
		/// The user SID
		/// </value>
		string UserSid { get; set; }

		/// <summary>
		/// Gets or sets the operation status.
		/// </summary>
		/// <value>
		/// The operation status.
		/// </value>
		int OperationStatus { get; set; }
	}
}