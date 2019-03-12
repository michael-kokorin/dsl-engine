namespace Infrastructure.Tests.Templates
{
	using FluentAssertions;

	using Moq;

	using NUnit.Framework;

	using Infrastructure.Templates;
	using Repository.Context;
	using Repository.Repositories;

	[TestFixture]
	public sealed class TemplateProviderTest
	{
		private const string TemplateName = "template_name";

		private ITemplateProvider _target;

		private Mock<ITemplateBuilder> _templateBuilder;

		private Mock<ITemplateRepository> _templateRepository;

		[SetUp]
		public void SetUp()
		{
			_templateBuilder = new Mock<ITemplateBuilder>();
			_templateRepository = new Mock<ITemplateRepository>();

			_target = new TemplateProvider(_templateBuilder.Object, _templateRepository.Object);
		}

		[Test]
		public void ShouldGetTemplateById()
		{
			const long templateId = 123;

			const string titletemplate = "title template";

			const string bodyTemplate = "body template";

			var templateDb = new Templates
			{
				Title = titletemplate,
				Body = bodyTemplate,
				Id = templateId
			};

			_templateRepository
				.Setup(_ => _.GetById(templateId))
				.Returns(templateDb);

			var template = new Mock<ITemplateWithTitle>();

			_templateBuilder
				.Setup(_ => _.Build(titletemplate, bodyTemplate))
				.Returns(template.Object);

			var result = _target.Get(templateId);

			result.Should().NotBeNull();
			result.ShouldBeEquivalentTo(template.Object);
		}

		[Test]
		public void ShouldGetTemplateByName()
		{
			const string titletemplate = "title template";

			const string bodyTemplate = "body template";

			var templateDb = new Templates
			{
				Title = titletemplate,
				Body = bodyTemplate
			};

			_templateRepository
				.Setup(_ => _.GetByKey(TemplateName))
				.Returns(templateDb);

			var template = new Mock<ITemplateWithTitle>();

			_templateBuilder
				.Setup(_ => _.Build(titletemplate, bodyTemplate))
				.Returns(template.Object);

			var result = _target.Get(TemplateName);

			result.Should().NotBeNull();
			result.ShouldBeEquivalentTo(template.Object);
		}

		[Test]
		public void ShouldThrowExceptionWhenTemplateNotFound()
			=> Assert.Throws<TemplateNotFoundException>(() => _target.Get(TemplateName));
	}
}