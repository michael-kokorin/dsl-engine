namespace Infrastructure.Query
{
	using System;

	using Infrastructure.DataSource;
	using Infrastructure.Engines.Dsl.Query;

	public interface IQueryProjectRestrictor
	{
		void Restrict(DslDataQuery query,
			Type entityType,
			DataSourceInfo dataSource,
			long userId);
	}
}