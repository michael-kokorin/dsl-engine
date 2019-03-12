namespace Modules.Core.Tests.Services.UI.Renderers
{
	using System.Collections.Generic;

	using FluentAssertions;

	using NUnit.Framework;

	using Infrastructure.Engines.Query.Result;
	using Modules.Core.Contracts.UI.Dto.Data;
	using Modules.Core.Services.UI.Renderers;

	[TestFixture]
	public sealed class TableRendererTest
	{
		[SetUp]
		public void SetUp() => _target = new TableRenderer();

		private TableRenderer _target;

		private static IEnumerable<QueryResultItem> GetTestCollection(int entityId, object value0, object value1)
			=> new[]
			{
				new QueryResultItem
				{
					EntityId = entityId,
					Value = new
					{
						FirstColumn = value0,
						SecondColumn = value1
					}
				}
			};

		private static void AssertResult(TableDto result, int entityId, long value0, string value1)
		{
			result.Should().NotBeNull();
			result.Columns.Should().NotBeNull();
			result.Columns.Length.ShouldBeEquivalentTo(2);
			result.Rows.Should().NotBeNull();
			result.Rows.Length.ShouldBeEquivalentTo(1);

			AssertColumn(result.Columns[0], "FirstColumn", 1);

			var row = result.Rows[0];

			row.EntityId.ShouldBeEquivalentTo(entityId);
			row.Items.Length.ShouldBeEquivalentTo(2);

			AssertRowItem(row.Items[0], "FirstColumn", value0);
			AssertRowItem(row.Items[1], "SecondColumn", value1);
		}

		private static void AssertColumn(TableColumnDto column, string columnKey, int order)
		{
			column.Should().NotBeNull();
			column.Key.ShouldBeEquivalentTo(columnKey);
			column.Order.ShouldBeEquivalentTo(order);
		}

		private static void AssertRowItem(TableRowItemDto item0, string columnKey, object value0)
		{
			item0.Should().NotBeNull();
			item0.ColumnKey.ShouldBeEquivalentTo(columnKey);
			item0.Value.ShouldBeEquivalentTo(value0);
		}

		[Test]
		public void ShouldConvertTable()
		{
			const int entityId = 23;
			const long value0 = 5234L;
			const string value1 = "hello, world";

			var collection = GetTestCollection(entityId, value0, value1);

			var result = _target.Render(collection);

			AssertResult(result, entityId, value0, value1);
		}

		[Test]
		public void ShouldConvertTableWithNullValue()
		{
			const int entityId = 23;
			const long value0 = 5234L;

			var collection = GetTestCollection(entityId, value0, null);

			var result = _target.Render(collection);

			AssertResult(result, entityId, value0, null);
		}
	}
}