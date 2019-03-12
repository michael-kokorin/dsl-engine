namespace Modules.Core.Helpers
{
	using System;
	using System.Collections.Generic;

	using JetBrains.Annotations;

	using Common.Extensions;
	using Modules.Core.Contracts.UI.Dto;
	using Repository.Repositories;

	/// <summary>
	/// Provides methods to validate setting value.
	/// </summary>
	/// <seealso cref="Modules.Core.Helpers.ISettingValidator" />
	internal sealed class SettingValidator : ISettingValidator
	{
		private readonly ISettingConditionValidatorProvider _validatorProvider;

		private readonly ISettingValuesRepository _settingValuesRepository;

		public SettingValidator(
			[NotNull] ISettingConditionValidatorProvider validatorProvider,
			[NotNull] ISettingValuesRepository settingValuesRepository)
		{
			if(validatorProvider == null) throw new ArgumentNullException(nameof(validatorProvider));
			if(settingValuesRepository == null) throw new ArgumentNullException(nameof(settingValuesRepository));

			_validatorProvider = validatorProvider;
			_settingValuesRepository = settingValuesRepository;
		}

		/// <summary>
		/// Determines whether the specified setting is valid.
		/// </summary>
		/// <param name="setting">The setting.</param>
		/// <param name="errorMessage">The error message.</param>
		/// <returns>
		///   <see langword="true" /> if the setting is valid; otherwise, <see langword="false" />.
		/// </returns>
		public bool IsValid(SettingValueDto setting, out string errorMessage)
		{
			var entity = _settingValuesRepository.GetById(setting.Id);
			var settingEntity = entity.Settings;

			if(string.IsNullOrWhiteSpace(settingEntity.Conditions))
			{
				errorMessage = string.Empty;
				return true;
			}

			var conditions = settingEntity.Conditions.FromJson<KeyValuePair<string, string>[]>();
			errorMessage = string.Empty;
			var isValid = true;
			foreach(var condition in conditions)
			{
				string currentError;
				var validator = _validatorProvider.Get(condition.Key);
				if(!validator.IsValid(setting, settingEntity, condition.Value, out currentError))
				{
					errorMessage += currentError + Environment.NewLine;
					isValid = false;
				}
			}

			return isValid;
		}
	}
}