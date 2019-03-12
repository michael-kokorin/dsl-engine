namespace Infrastructure.Engines.Tests.Query
{
	using FluentAssertions;

	using NUnit.Framework;

	using Infrastructure.Engines.Dsl.Query;
	using Infrastructure.Engines.Query;

	[TestFixture]
	public sealed class LimitBlockTranslatorTest
	{
		private IQueryBlockTranslator<DslLimitBlock> _target;

		[SetUp]
		public void SetUp() => _target = new LimitBlockTranslator();

		[TestCase(1, 2)]
		[TestCase(null, 2)]
		[TestCase(1, null)]
		public void ShouldTranslateQueryLimitBlock(int? skip, int? take)
		{
			var limitBlock = new DslLimitBlock
			{
				Skip = skip,
				Take = take
			};

			var available = string.Empty;

			if (skip != null)
			{
				available = ".Skip(" + skip + ")\r\n";
			}

			if (take != null)
			{
				available += ".Take(" + take + ")\r\n";
			}

			var result = _target.Translate(limitBlock);

			result.ShouldBeEquivalentTo(available);
		}

		[TestCase(1,2, "skip 1\r\ntake 2\r\n")]
		[TestCase(null, 2, "take 2\r\n")]
		[TestCase(1, null, "skip 1\r\n")]
		public void ShouldTranslateLimitBlockToDsl(int? skip, int? take, string available)
		{
			var result = _target.ToDsl(new DslLimitBlock
			{
				Skip = skip,
				Take = take
			});

			result.ShouldBeEquivalentTo(available);
		}
	}
}