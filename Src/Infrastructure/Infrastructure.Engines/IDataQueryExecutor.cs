namespace Infrastructure.Engines
{
    using System;

    using Common.Data;

    /// <summary>
    ///     Provide method to execute dynamic queries to data.
    /// </summary>
    public interface IDataQueryExecutor
    {
        /// <summary>
        ///     Executes the specified query to data.
        /// </summary>
        /// <typeparam name="T">Type of entity to request.</typeparam>
        /// <param name="source">The data source.</param>
        /// <param name="dataQuery">The query.</param>
        /// <returns>Result data.</returns>
        object Execute<T>(IDataSource<T> source, string dataQuery);

        /// <summary>
        ///     Executes the specified query to data.
        /// </summary>
        /// <param name="source">The data source.</param>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="dataQuery">The query.</param>
        /// <returns>
        ///     Result data.
        /// </returns>
        object Execute(IDataSource<object> source, Type entityType, string dataQuery);
    }
}