namespace Repository.Repositories
{
	using System;
	using System.Linq;

	using JetBrains.Annotations;

	using Repository.Context;

	[UsedImplicitly]
	internal sealed class SettingGroupRepository: Repository<SettingGroups>, ISettingGroupRepository
	{
		private readonly IDbContextProvider _dbContextProvider;

		/// <summary>
		///   Initializes a new instance of the <see cref="Repository{T}"/> class.
		/// </summary>
		/// <param name="dbContextProvider">The database context provider.</param>
		/// <exception cref="ArgumentNullException"><paramref name="dbContextProvider"/> is <see langword="null"/>.</exception>
		public SettingGroupRepository(
			[NotNull] IDbContextProvider dbContextProvider)
			: base(dbContextProvider)
		{
			_dbContextProvider = dbContextProvider;
		}

		[CanBeNull]
		public SettingGroups Get(long pluginId, string code) => _dbContextProvider
			.GetContext()
			.Table<SettingGroups>()
			.FirstOrDefault(item => (item.OwnerPluginId == pluginId) && (item.Code == code));
	}
}