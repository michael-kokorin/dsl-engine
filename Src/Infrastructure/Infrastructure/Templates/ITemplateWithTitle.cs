namespace Infrastructure.Templates
{
	using System.Collections.Generic;

	public interface ITemplateWithTitle
	{
		ITemplate Title { get; }

		ITemplate Body { get; }

		void Add(IDictionary<string, object> properties);
	}
}