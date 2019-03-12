namespace Repository.Repositories
{
	using System;
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Enums;
	using Repository.Context;
	using Repository.Localization;

	internal sealed class TableColumnsRepository: LocalizedRepository<TableColumns>, ITableColumnsRepository
	{
		private readonly IDbContextProvider _dbContextProvider;

		public TableColumnsRepository(
			[NotNull] IDbContextProvider dbContextProvider,
			[NotNull] IUserLocalizationProvider userLocalizationProvider)
			: base(dbContextProvider, userLocalizationProvider)
		{
			if(dbContextProvider == null) throw new ArgumentNullException(nameof(dbContextProvider));

			_dbContextProvider = dbContextProvider;
		}

		/// <summary>
		///   Gets all available table columns.
		/// </summary>
		/// <returns>
		///   Columns.
		/// </returns>
		public IQueryable<TableColumns> GetAvailable() =>
			LocalizedQuery()
				.Where(_ => _.FieldType != (int)DataSourceFieldType.Closed);

		/// <summary>
		///   Gets available table columns with the specified identifier.
		/// </summary>
		/// <param name="tableColumnId">The table column identifier.</param>
		/// <returns>
		///   Columns.
		/// </returns>
		public IQueryable<TableColumns> GetAvailable(long tableColumnId) =>
			GetAvailable().Where(_ => _.Id == tableColumnId);

		/// <summary>
		///   Gets columns by table.
		/// </summary>
		/// <param name="tableId">The table identifier.</param>
		/// <returns>
		///   Columns.
		/// </returns>
		public IQueryable<TableColumns> GetByTable(long tableId) =>
			LocalizedQuery().Where(_ => _.TableId == tableId);

		/// <summary>
		///   Gets available columns by table.
		/// </summary>
		/// <param name="tableId">The table identifier.</param>
		/// <returns></returns>
		public IQueryable<TableColumns> GetAvailableByTable(long tableId) =>
			GetAvailable().Where(_ => _.TableId == tableId);

		/// <summary>
		///   Gets available columns by table.
		/// </summary>
		/// <param name="tableKey">The table key.</param>
		/// <returns>
		///   Columns.
		/// </returns>
		public IQueryable<TableColumns> GetAvailableByTable(string tableKey) =>
			GetAvailable().Where(_ => _.Tables1.Name == tableKey);

		/// <summary>
		///   Gets table columns by the specified table name and field name.
		/// </summary>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="fieldName">Name of the field.</param>
		/// <returns>
		///   Columns.
		/// </returns>
		public IQueryable<TableColumns> Get(string tableName, string fieldName) =>
			LocalizedQuery().Where(_ => (_.Tables1.Name == tableName) && (_.Name == fieldName));

		/// <summary>
		///   Gets the localized query.
		/// </summary>
		/// <param name="cultureId">The culture identifier.</param>
		/// <returns></returns>
		protected override IQueryable<TableColumns> GetLocalizedQuery(long cultureId) =>
			_dbContextProvider.GetContext().GetTableColumns(cultureId);
	}
}