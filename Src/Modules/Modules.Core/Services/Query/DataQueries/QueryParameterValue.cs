namespace Modules.Core.Services.Query.DataQueries
{
	using System;

	using JetBrains.Annotations;

	public sealed class QueryParameterValue
	{
		public readonly string Key;

		public readonly string Value;

		public QueryParameterValue([NotNull] string key, [NotNull] string value)
		{
			if (key == null) throw new ArgumentNullException(nameof(key));
			if (value == null) throw new ArgumentNullException(nameof(value));

			if (string.IsNullOrEmpty(key))
				throw new ArgumentException(nameof(key));

			if (string.IsNullOrEmpty(value))
				throw new ArgumentException(nameof(value));

			Key = key;
			Value = value;
		}
	}
}