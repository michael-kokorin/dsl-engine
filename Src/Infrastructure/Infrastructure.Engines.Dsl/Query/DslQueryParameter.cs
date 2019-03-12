namespace Infrastructure.Engines.Dsl.Query
{
	using System;

	using JetBrains.Annotations;

	using Common.Extensions;
	using Infrastructure.Engines.Dsl.Query.Filter;

	public sealed class DslQueryParameter
	{
		public string Key { get; set; }

		public DslQueryParameter([NotNull] string key)
		{
			if (key == null) throw new ArgumentNullException(nameof(key));

			if (string.IsNullOrEmpty(key)) throw new ArgumentException(nameof(key));

			Key = key;

			RemoveParameterTags();
		}

		private void RemoveParameterTags()
		{
			Key = Key.Replace(FilterKeywords.ParameterOpenTag.GetDescription(), string.Empty);

			Key = Key.Replace(FilterKeywords.ParameterCloseTag.GetDescription(), string.Empty);
		}
	}
}