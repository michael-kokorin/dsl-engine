namespace Infrastructure.Tests.Templates
{
	using System.Collections.Generic;

	using Moq;

	using NUnit.Framework;

	using Infrastructure.Templates;

	[TestFixture]
	public sealed class TemplateWirhTitleTest
	{
		private ITemplateWithTitle _target;

		private Mock<ITemplate> _titleTemplate;

		private Mock<ITemplate> _bodyTemplate;

		[SetUp]
		public void SetUp()
		{
			_titleTemplate = new Mock<ITemplate>();
			_bodyTemplate = new Mock<ITemplate>();

			_target = new TemplateWithTitle(_titleTemplate.Object, _bodyTemplate.Object);
		}

		[Test]
		public void ShouldAddParameters()
		{
			var parameters = new Dictionary<string, object>();

			_target.Add(parameters);

			_titleTemplate.Verify(_ => _.Add(parameters), Times.Once);
			_bodyTemplate.Verify(_ => _.Add(parameters), Times.Once);
		}
	}
}