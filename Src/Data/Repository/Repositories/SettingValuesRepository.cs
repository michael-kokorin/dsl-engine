namespace Repository.Repositories
{
	using System;
	using System.Data.Entity;
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Enums;
	using Repository.Context;

	[UsedImplicitly]
	internal sealed class SettingValuesRepository: Repository<SettingValues>, ISettingValuesRepository
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="Repository{T}"/> class.
		/// </summary>
		/// <param name="dbContextProvider">The database context provider.</param>
		/// <exception cref="ArgumentNullException"><paramref name="dbContextProvider"/> is <see langword="null"/>.</exception>
		public SettingValuesRepository([NotNull] IDbContextProvider dbContextProvider): base(dbContextProvider)
		{
		}

		public override IQueryable<SettingValues> Query() => base.Query().Include(_ => _.Settings);

		public IQueryable<SettingValues> Get(PluginType pluginType, int settingOwner, long entityId) =>
			Query()
				.Where(
					_ =>
						(_.Settings.SettingOwner == settingOwner) && (_.EntityId == entityId) && _.Settings.OwnerPluginId.HasValue &&
						(_.Settings.Plugins.Type == (int)pluginType) &&
						!_.Settings.IsArchived);

		public SettingValues Get(PluginType pluginType, int settingOwner, long entityId, string settingKey) =>
			Get(pluginType, settingOwner, entityId)
				.FirstOrDefault(_ => _.Settings.Code == settingKey);
	}
}