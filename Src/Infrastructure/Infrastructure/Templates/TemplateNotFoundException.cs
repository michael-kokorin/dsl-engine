namespace Infrastructure.Templates
{
	using System;

	internal sealed class TemplateNotFoundException : Exception
	{
		public TemplateNotFoundException(long templateId) :
			base($"Template not found. Template Id='{templateId}'")
		{

		}

		public TemplateNotFoundException(string templateName) :
			base($"Template not found. Template name='{templateName}'")
		{

		}
	}
}