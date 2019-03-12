namespace Modules.Core.Extensions
{
	using System;
	using System.Reflection;

	using Common.Extensions;
	using Modules.Core.Properties;

	internal static class PropertyInfoExtension
	{
		private const string LocalizationKey = "Property_{0}";

		public static string Localize(this PropertyInfo property)
		{
			if(property == null)
				throw new ArgumentNullException(nameof(property));

			var key = LocalizationKey.FormatWith(property.Name);

			return Resources.ResourceManager.GetString(key);
		}
	}
}