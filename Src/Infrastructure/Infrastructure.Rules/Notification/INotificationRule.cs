namespace Infrastructure.Rules.Notification
{
	using Common.Enums;
	using Infrastructure.Engines.Dsl;

	/// <summary>
	///     Represents notification rule.
	/// </summary>
	public interface INotificationRule : IRule
	{
		/// <summary>
		///     Gets a value indicating whether this instance is event triggered rule.
		/// </summary>
		/// <value>
		///     <see langword="true" /> if this instance is event triggered rule; otherwise, <see langword="false" />.
		/// </value>
		bool IsEventTriggered { get; }

		/// <summary>
		///     Gets a value indicating whether this instance is time triggered rule.
		/// </summary>
		/// <value>
		///     <see langword="true" /> if this instance is time triggered rule; otherwise, <see langword="false" />.
		/// </value>
		bool IsTimeTriggered { get; }

		/// <summary>
		///     Gets a value indicating whether this rule is for all subjects.
		/// </summary>
		/// <value>
		///     <see langword="true" /> if this rule is for all subjects; otherwise, <see langword="false" />.
		/// </value>
		bool IsForAllSubjects { get; }

		/// <summary>
		///     Gets the delivery protocol.
		/// </summary>
		/// <value>
		///     The delivery protocol.
		/// </value>
		NotificationProtocolType DeliveryProtocol { get; }

		NotificationRuleExpr Expression { get; }
	}
}