namespace Infrastructure.Tests.Templates
{
	using NUnit.Framework;

	using Common.FileSystem;
	using Infrastructure.Templates;

	[TestFixture]
	public sealed class TemplateTest
	{
		[TestCase("DevPolicySucceed")]
		[TestCase("DevPolicyViolated")]
		[TestCase("DevScanFinished")]
		[TestCase("ManPolicySucceed")]
		[TestCase("ManPolicyViolated")]
		[TestCase("ManScanFinished")]
		public void RenderBody(string fileName)
		{
			var templateSource = FileLoader.FromResource($"{GetType().Namespace}.Body.{fileName}.html");

			var template = new Antlr4Template(templateSource);

			template.Render();
		}

		[TestCase("DevPolicySucceed")]
		[TestCase("DevPolicyViolated")]
		[TestCase("DevScanFinished")]
		[TestCase("ManPolicySucceed")]
		[TestCase("ManPolicyViolated")]
		[TestCase("ManScanFinished")]
		public void RenderTitle(string fileName)
		{
			var templateSource = FileLoader.FromResource($"{GetType().Namespace}.Title.{fileName}.txt");

			var template = new Antlr4Template(templateSource);

			template.Render();
		}
	}
}