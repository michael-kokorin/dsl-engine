namespace Infrastructure.Templates
{
	using JetBrains.Annotations;

	using Repository.Context;
	using Repository.Repositories;

	internal sealed class TemplateProvider : ITemplateProvider
	{
		private readonly ITemplateBuilder _templateBuilder;

		private readonly ITemplateRepository _templateRepository;

		public TemplateProvider(
			[NotNull] ITemplateBuilder templateBuilder,
			[NotNull] ITemplateRepository templateRepository)
		{
			_templateBuilder = templateBuilder;
			_templateRepository = templateRepository;
		}

		public ITemplateWithTitle Get(long templateId)
		{
			var template = _templateRepository.GetById(templateId);

			if (template == null)
				throw new TemplateNotFoundException(templateId);

			return Get(template);
		}

		public ITemplateWithTitle Get(string templateName)
		{
			var template = _templateRepository.GetByKey(templateName);

			if (template == null)
				throw new TemplateNotFoundException(templateName);

			return Get(template);
		}

		private ITemplateWithTitle Get(Templates template) =>
			_templateBuilder.Build(template.Title, template.Body);
	}
}