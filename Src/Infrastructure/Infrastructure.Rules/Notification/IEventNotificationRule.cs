namespace Infrastructure.Rules.Notification
{
	using System.Collections.Generic;

	using Infrastructure.Engines.Dsl;

	/// <summary>
	///     Represents the event triggered notification rule.
	/// </summary>
	public interface IEventNotificationRule : INotificationRule
	{
		/// <summary>
		///     Gets the get dependent events.
		/// </summary>
		/// <value>
		///     The get dependent events.
		/// </value>
		IEnumerable<string> GetDependentEvents { get; }

		/// <summary>
		///     Gets a value indicating whether this instance has dependent events.
		/// </summary>
		/// <value>
		///     <see langword="true" /> if this instance has dependent events; otherwise, <see langword="false" />.
		/// </value>
		bool HasDependentEvents { get; }

		/// <summary>
		///     Gets the event group action.
		/// </summary>
		/// <value>
		///     The event group action.
		/// </value>
		GroupActionType EventGroupAction { get; }

		/// <summary>
		///     Gets a value indicating whether this rule is for all events.
		/// </summary>
		/// <value>
		///     <see langword="true" /> if this rule is for all events; otherwise, <see langword="false" />.
		/// </value>
		bool IsForAllEvents { get; }

		/// <summary>
		///     Verifies that event match the current rule.
		/// </summary>
		/// <param name="eventName">Name of the event.</param>
		/// <returns><see langword="true" /> if the event matches the current rule; otherwise, <see langword="false" />.</returns>
		bool IsEventMatch(string eventName);
	}
}