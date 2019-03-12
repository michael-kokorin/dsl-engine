namespace Modules.Core.Helpers
{
	using System.Collections.Generic;
	using System.Linq;

	using Microsoft.Practices.Unity;

	using Common.Extensions;

	/// <summary>
	///   Provides methods to get condition validator.
	/// </summary>
	/// <seealso cref="Modules.Core.Helpers.ISettingConditionValidatorProvider"/>
	internal sealed class SettingConditionValidatorProvider: ISettingConditionValidatorProvider
	{
		private readonly Dictionary<string, ISettingConditionValidator> _validators;

		public SettingConditionValidatorProvider(IUnityContainer container)
		{
			_validators =
				container.ResolveAll(typeof(ISettingConditionValidator))
								.Cast<ISettingConditionValidator>()
								.ToDictionary(_ => _.Condition, _ => _);
		}

		/// <summary>
		///   Gets the condition validator for specified condition.
		/// </summary>
		/// <param name="condition">The condition.</param>
		/// <returns>The condition validator.</returns>
		public ISettingConditionValidator Get(string condition) =>
			_validators.Get(condition);
	}
}