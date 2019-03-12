namespace Common.Tests.Data
{
	using System;
	using System.Linq;

	using FluentAssertions;

	using JetBrains.Annotations;

	using Microsoft.Practices.Unity;

	using NUnit.Framework;

	using Common.Container;
	using Common.Data;
	using Common.Extensions;

	[TestFixture]
	public sealed class DataSourceProviderTest
	{
		[SetUp]
		public void SetUp()
		{
			_unityContainer = new UnityContainer();

			_unityContainer.RegisterType<IDataSource<TestObject>, TestDataSource>(ReuseScope.Container);
			_unityContainer.RegisterType<IDataSource, TestDataSource>(typeof(TestObject).Name, ReuseScope.Container);

			_target = new DataSourceProvider(_unityContainer);
		}

		private IDataSourceProvider _target;

		private IUnityContainer _unityContainer;

		[UsedImplicitly]
		public sealed class TestObject
		{
		}

		public sealed class TestDataSource: IDataSource<TestObject>
		{
			public IQueryable<TestObject> Query()
			{
				throw new NotImplementedException();
			}

			/// <summary>
			///   Queries data.
			/// </summary>
			/// <returns>Data.</returns>
			IQueryable<object> IDataSource.Query() => Query();
		}

		[Test]
		public void ShouldGetDataSource()
		{
			var res = _target.GetDataSource(typeof(TestObject));

			res.Should().BeOfType<TestDataSource>();
		}
	}
}