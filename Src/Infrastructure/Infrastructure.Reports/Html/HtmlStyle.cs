namespace Infrastructure.Reports.Html
{
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Linq;

	internal sealed class HtmlStyle
	{
		private readonly IDictionary<string, string> _properties;

		public HtmlStyle() : this(Enumerable.Empty<KeyValuePair<string, string>>())
		{

		}

		public HtmlStyle(IEnumerable<KeyValuePair<string, string>> source)
		{
			_properties = new ConcurrentDictionary<string, string>(source);
		}

		public HtmlStyle Set(string key, string value)
		{
			if (_properties.ContainsKey(key))
				throw new HtmlStyleAlreadyDefinedException(key);

			_properties.Add(key, value);

			return this;
		}

		public override string ToString() => string.Join("; ", _properties.Select(_ => $"{_.Key}: {_.Value}"));
	}
}