namespace Infrastructure.Engines.Dsl
{
	using System.Collections.Generic;

	/// <summary>
	///   Represents notification rule.
	/// </summary>
	public sealed class NotificationRuleExpr
	{
		/// <summary>
		///   Gets the attachments.
		/// </summary>
		/// <value>
		///   The attachments.
		/// </value>
		public IEnumerable<INotificationAttachBlock> Attachments { get; internal set; }

		/// <summary>
		///   Gets or sets the event.
		/// </summary>
		/// <value>
		///   The event.
		/// </value>
		public GroupExpr Event { get; internal set; }

		/// <summary>
		///   Gets or sets the protocol.
		/// </summary>
		/// <value>
		///   The protocol.
		/// </value>
		public string Protocol { get; internal set; }

		/// <summary>
		/// Gets or sets the report identifier.
		/// </summary>
		/// <value>
		/// The report identifier.
		/// </value>
		public long ReportId { get; internal set; }

		/// <summary>
		///   Gets or sets the subjects.
		/// </summary>
		/// <value>
		///   The subjects.
		/// </value>
		public SubjectsExpr Subjects { get; internal set; }

		/// <summary>
		///   Gets or sets the trigger.
		/// </summary>
		/// <value>
		///   The trigger.
		/// </value>
		public TimeTriggerExpr Trigger { get; internal set; }
	}
}