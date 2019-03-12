namespace Common.Enums
{
	using System;
	using System.Linq;
	using System.Reflection;

	using JetBrains.Annotations;

	[AttributeUsage(AttributeTargets.Field)]
	public sealed class ReportFileTypeExtensionAttribute : Attribute
	{
		private readonly string _extension;

		internal ReportFileTypeExtensionAttribute([NotNull] string extension)
		{
			if (extension == null) throw new ArgumentNullException(nameof(extension));

			_extension = extension;
		}

		public static string Get(ReportFileType value)
		{
			var type = value.GetType();

			var memInfo = type.GetMember(value.ToString()).SingleOrDefault();

			var attribute = memInfo.GetCustomAttribute<ReportFileTypeExtensionAttribute>();

			if (attribute == null)
				throw new ArgumentException(nameof(value));

			return attribute._extension;
		}
	}
}