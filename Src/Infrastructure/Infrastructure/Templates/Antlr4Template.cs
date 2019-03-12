namespace Infrastructure.Templates
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;

	using Antlr4.StringTemplate;

	using JetBrains.Annotations;

	public sealed class Antlr4Template : ITemplate
	{
		private const char DivideChar = '$';

		private readonly string _source;

		private readonly Template _template;

		public Antlr4Template(string source)
		{
			if (source == null) throw new ArgumentNullException(nameof(source));

			_source = source;

			_template = new Template(_source, DivideChar, DivideChar);
		}

		public void Add(IDictionary<string, object> parameters)
		{
			if ((parameters == null) ||
				(parameters.Count == 0))
				return;

			foreach (var value in parameters)
			{
				Add(value.Key, value.Value);
			}
		}

		public void Add([NotNull] string key, object value)
		{
			if (key == null) throw new ArgumentNullException(nameof(key));

			if (string.IsNullOrEmpty(key)) throw new ArgumentException(nameof(key));

			_template.Add(key, value);
		}

		public string Render() => _template.Render(CultureInfo.CurrentCulture);

		public string GetSource() => _source;
	}
}