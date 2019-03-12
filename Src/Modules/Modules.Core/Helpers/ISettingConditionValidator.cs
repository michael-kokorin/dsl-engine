namespace Modules.Core.Helpers
{
	using Modules.Core.Contracts.UI.Dto;
	using Repository.Context;

	/// <summary>
	///   Represents validator for a specified condition.
	/// </summary>
	public interface ISettingConditionValidator
	{
		/// <summary>
		///   Gets the condition.
		/// </summary>
		/// <value>
		///   The condition.
		/// </value>
		string Condition { get; }

		/// <summary>
		/// Determines whether the specified setting value is valid.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="setting">The setting.</param>
		/// <param name="conditionValue">The condition value.</param>
		/// <param name="errorMessage">The error message.</param>
		/// <returns>
		///   <see langword="true" /> if the setting is valid; otherwise, <see langword="false" />.
		/// </returns>
		bool IsValid(SettingValueDto value, Settings setting, string conditionValue, out string errorMessage);
	}
}