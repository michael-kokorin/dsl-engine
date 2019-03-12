namespace Infrastructure.Engines.Tests
{
    using FluentAssertions;

    using NUnit.Framework;

    using Repository;
    using Repository.Context;
    using Repository.Repositories;

    [TestFixture]
    public sealed class DataQueryExecutorTest
    {
        [Test][Ignore("Not a unit test")]
        public void QueryTest()
        {
            var factory = new SdlContextFactory();
            var unitOfWork = new UnitOfWork(factory);
            var source = new TaskRepository(unitOfWork);

            var executor = new DataQueryExecutor();

            const string dataQuery = @"source.Query().Where(x => x.ProjectId == 10010)
                .AsEnumerable()
                .Select(x => new
                {
                    EntityId = x.Id,
                    Value = new
                    {
                        TaskStatus = x.TaskStatus.ToString(""g""),
                        x.Repository,
                        Created = x.Created.ToString(""yyyy-MM-dd HH:mm:ss.fffffff""),
                        ScanCore = x.ScanCores.DisplayName,
                        Finished = x.Finished.HasValue ? x.Finished.Value.ToString(""yyyy-MM-dd HH:mm:ss.fffffff"") : string.Empty
                    }
    }).ToList();";
            var result = executor.Execute(source, typeof(Tasks), dataQuery);

            result.Should().NotBeNull();
        }
    }
}