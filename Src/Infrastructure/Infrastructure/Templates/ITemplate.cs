namespace Infrastructure.Templates
{
	using System.Collections.Generic;

	public interface ITemplate
	{
		void Add(IDictionary<string, object> parameters);

		void Add(string key, object value);

		string Render();

	    string GetSource();
	}
}