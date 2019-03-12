namespace Infrastructure.Templates
{
	/// <summary>
	///     Represents contract for template builder.
	/// </summary>
	public interface ITemplateBuilder
	{
		/// <summary>
		///     Builds the template with specified title and body.
		/// </summary>
		/// <param name="title">The title template.</param>
		/// <param name="body">The body template.</param>
		/// <returns>The built template.</returns>
		ITemplateWithTitle Build(string title, string body);

		/// <summary>
		/// Builds template from the specified source.
		/// </summary>
		/// <param name="source">The template source.</param>
		/// <returns>Template from the source object</returns>
		ITemplate Build(string source);
	}
}