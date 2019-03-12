namespace Modules.Core.Helpers
{
	/// <summary>
	///   Provides methods to get condition validator.
	/// </summary>
	public interface ISettingConditionValidatorProvider
	{
		/// <summary>
		///   Gets the condition validator for specified condition.
		/// </summary>
		/// <param name="condition">The condition.</param>
		/// <returns>The condition validator.</returns>
		ISettingConditionValidator Get(string condition);
	}
}