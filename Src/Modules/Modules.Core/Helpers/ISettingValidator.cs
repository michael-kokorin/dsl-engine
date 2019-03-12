namespace Modules.Core.Helpers
{
	using Modules.Core.Contracts.UI.Dto;

	/// <summary>
	///   Provides methods to validate setting.
	/// </summary>
	public interface ISettingValidator
	{
		/// <summary>
		/// Determines whether the specified setting is valid.
		/// </summary>
		/// <param name="setting">The setting.</param>
		/// <param name="errorMessage">The error message.</param>
		/// <returns>
		///   <see langword="true" /> if the setting is valid; otherwise, <see langword="false" />.
		/// </returns>
		bool IsValid(SettingValueDto setting, out string errorMessage);
	}
}