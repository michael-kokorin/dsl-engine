namespace Infrastructure.Templates
{
	public interface ITemplateProvider
	{
		ITemplateWithTitle Get(long templateId);

		ITemplateWithTitle Get(string templateName);
	}
}