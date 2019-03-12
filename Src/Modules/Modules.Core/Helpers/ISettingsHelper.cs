namespace Modules.Core.Helpers
{
	using System.Collections.Generic;

	using JetBrains.Annotations;

	using Modules.Core.Contracts.UI.Dto;

	/// <summary>
	///     Provides methods to manage settings.
	/// </summary>
	public interface ISettingsHelper
	{
		/// <summary>
		///     Copies the project settings to task settings.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <param name="taskId">The task identifier.</param>
		void CopyProjectSettingsToTaskSettings(long projectId, long taskId);

		/// <summary>
		///     Gets settings at the specified owner.
		/// </summary>
		/// <param name="owner">The owner.</param>
		/// <param name="entityId">The entity identifier.</param>
		/// <returns>Settings.</returns>
		[NotNull]
		[ItemNotNull]
		SettingGroupDto[] Get(SettingOwnerDto owner, long entityId);

		/// <summary>
		///     Gets setting value at the specified owner and for the specified setting.
		/// </summary>
		/// <param name="owner">The owner.</param>
		/// <param name="entityId">The entity identifier.</param>
		/// <param name="settingKey">The setting key.</param>
		/// <returns>The setting value.</returns>
		[CanBeNull]
		SettingValueDto Get(SettingOwnerDto owner, long entityId, string settingKey);

		/// <summary>
		///     Saves settings.
		/// </summary>
		/// <param name="values">The values.</param>
		/// <returns>A collection of values with errors. Key is identifier of value. Value is error message.</returns>
		Dictionary<long, string> Save([NotNull] [ItemNotNull] SettingValueDto[] values);
	}
}