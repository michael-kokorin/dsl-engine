namespace Modules.Core.Helpers
{
	using System;
	using System.Globalization;

	using JetBrains.Annotations;

	using Common.Enums;
	using Common.Extensions;
	using Common.Logging;
	using Infrastructure.Plugins.Common.Contracts;
	using Modules.Core.Contracts.UI.Dto;
	using Repository.Context;

	using static Properties.Resources;

	/// <summary>
	///     Provides methods to validate min condition for setting.
	/// </summary>
	/// <seealso cref="Modules.Core.Helpers.ISettingConditionValidator"/>
	internal sealed class MinSettingConditionValidator: ISettingConditionValidator
	{
		private readonly ILog _logger;

		public MinSettingConditionValidator([NotNull] ILog logger)
		{
			if(logger == null)
			{
				throw new ArgumentNullException(nameof(logger));
			}

			_logger = logger;
		}

		/// <summary>
		///     Gets the condition.
		/// </summary>
		/// <value>
		///     The condition.
		/// </value>
		public string Condition => "min";

		/// <summary>
		///     Determines whether the specified setting value is valid.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="setting">The setting.</param>
		/// <param name="conditionValue">The condition value.</param>
		/// <param name="errorMessage">The error message.</param>
		/// <returns>
		///     <see langword="true"/> if the setting is valid; otherwise, <see langword="false"/>.
		/// </returns>
		public bool IsValid(SettingValueDto value, Settings setting, string conditionValue, out string errorMessage)
		{
			try
			{
				switch((SettingType)setting.SettingType)
				{
					case SettingType.Number:
						var numberFormat = new NumberFormatInfo
											{
												NumberDecimalSeparator = "."
											};
						var valueNumber = decimal.Parse(value.Value, numberFormat);
						var limitValue = decimal.Parse(conditionValue, numberFormat);
						if(valueNumber < limitValue)
						{
							errorMessage = MinValidationSettingError.FormatWith(limitValue);
							return false;
						}

						errorMessage = string.Empty;
						return true;
					default:
						throw new InvalidOperationException(UnsupportedConditionSetting);
				}
			}
			catch(Exception exception)
			{
				_logger.Error(ValidationErrorOccurredSetting, exception);
				errorMessage = ValidationErrorOccurredSetting;
				return false;
			}
		}
	}
}