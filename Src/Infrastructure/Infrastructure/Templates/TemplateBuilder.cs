namespace Infrastructure.Templates
{
	using JetBrains.Annotations;

	internal sealed class TemplateBuilder : ITemplateBuilder
	{
		public ITemplateWithTitle Build(string title, string body) =>
			new TemplateWithTitle(
				Build(title),
				Build(body));

		public ITemplate Build([NotNull] string source) => new Antlr4Template(source);
	}
}