namespace Modules.Core.Tests.Services.Query.DataQueries
{
	using FluentAssertions;

	using NUnit.Framework;

	using Common.Extensions;
	using Modules.Core.Services.Query.DataQueries;

	[TestFixture]
	public sealed class QueryParameterValueTest
	{
		[Test]
		public void Test()
		{
			var parameters = new[]
			{
				new QueryParameterValue("key1", "value1")
			};

			var serialized = parameters.ToJson(false);

			var deserialized = serialized.FromJson<QueryParameterValue[]>();

			deserialized.ShouldBeEquivalentTo(parameters);
		}
	}
}