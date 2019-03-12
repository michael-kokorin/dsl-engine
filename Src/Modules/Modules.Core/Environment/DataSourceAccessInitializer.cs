namespace Modules.Core.Environment
{
	using System;
	using System.Linq;

	using JetBrains.Annotations;

	using Infrastructure.DataSource;
	using Infrastructure.Security;
	using Repository.Repositories;

	[UsedImplicitly]
	internal sealed class DataSourceAccessInitializer: IDataSourceAccessInitializer
	{
		private readonly IAuthorityProvider _authorityProvider;

		private readonly IDataSourceAuthorityProvider _dataSourceAuthorityProvider;

		private readonly IRoleRepository _roleRepository;

		public DataSourceAccessInitializer(
			[NotNull] IAuthorityProvider authorityProvider,
			[NotNull] IDataSourceAuthorityProvider dataSourceAuthorityProvider,
			[NotNull] IRoleRepository roleRepository)
		{
			if(authorityProvider == null) throw new ArgumentNullException(nameof(authorityProvider));
			if(dataSourceAuthorityProvider == null) throw new ArgumentNullException(nameof(dataSourceAuthorityProvider));
			if(roleRepository == null) throw new ArgumentNullException(nameof(roleRepository));

			_authorityProvider = authorityProvider;
			_dataSourceAuthorityProvider = dataSourceAuthorityProvider;
			_roleRepository = roleRepository;
		}

		public void Initialize()
		{
			var dataSourceAuthorities = _dataSourceAuthorityProvider.Get().ToArray();

			var roles = _roleRepository.GetActive().ToArray();

			foreach(var role in roles)
			{
				_authorityProvider.Grant(role.Id, dataSourceAuthorities);
			}
		}
	}
}