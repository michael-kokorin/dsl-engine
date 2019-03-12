namespace Repository
{
	using System;

	using JetBrains.Annotations;

	public sealed class ProjectPropertyAttribute : Attribute
	{
		public readonly string PropertyName;

		internal ProjectPropertyAttribute([NotNull] string propertyName)
		{
			if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));

			if (string.IsNullOrEmpty(propertyName)) throw new ArgumentException(nameof(propertyName));

			PropertyName = propertyName;
		}
	}
}