namespace Infrastructure.DataSource
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Enums;
	using Common.Security;
	using Repository.Context;
	using Repository.Repositories;

	[UsedImplicitly]
	internal sealed class DataSourceAccessValidator: IDataSourceAccessValidator
	{
		private readonly IDataSourceAuthorityNameBuilder _dataSourceAuthorityNameBuilder;

		private readonly ITableRepository _tableRepository;

		private readonly IUserAuthorityValidator _userAuthorityValidator;

		public DataSourceAccessValidator(
			[NotNull] IDataSourceAuthorityNameBuilder dataSourceAuthorityNameBuilder,
			[NotNull] ITableRepository tableRepository,
			[NotNull] IUserAuthorityValidator userAuthorityValidator)
		{
			if(dataSourceAuthorityNameBuilder == null)
				throw new ArgumentNullException(nameof(dataSourceAuthorityNameBuilder));
			if(tableRepository == null)
				throw new ArgumentNullException(nameof(tableRepository));
			if(userAuthorityValidator == null)
				throw new ArgumentNullException(nameof(userAuthorityValidator));

			_dataSourceAuthorityNameBuilder = dataSourceAuthorityNameBuilder;
			_tableRepository = tableRepository;
			_userAuthorityValidator = userAuthorityValidator;
		}

		public bool CanReadSource(long dataSourceId, long userId)
		{
			var dataSource = _tableRepository.GetAvailable(dataSourceId).SingleOrDefault();

			if(dataSource == null)
				throw new ArgumentException(nameof(dataSourceId));

			return ValidateDataSource(userId, dataSource);
		}

		private bool ValidateDataSource(long userId, Tables dataSource) =>
			(dataSource.Type != (int)DataSourceType.Closed) &&
			GetDataSourceProjects(dataSource.Name, userId).Any();

		public bool CanReadSource(string dataSourceKey, long userId)
		{
			if(dataSourceKey == null)
				throw new ArgumentNullException(nameof(dataSourceKey));

			var dataSource = _tableRepository.Get(dataSourceKey).SingleOrDefault();

			if(dataSource == null)
				throw new ArgumentException(nameof(dataSourceKey));

			return ValidateDataSource(userId, dataSource);
		}

		public IEnumerable<long> GetDataSourceProjects(string dataSourceKey, long userId)
		{
			if(dataSourceKey == null)
				throw new ArgumentNullException(nameof(dataSourceKey));

			var authorityName = _dataSourceAuthorityNameBuilder.GetDataSourceAuthorityName(dataSourceKey);

			return _userAuthorityValidator.GetProjects(userId, new[] {authorityName});
		}
	}
}